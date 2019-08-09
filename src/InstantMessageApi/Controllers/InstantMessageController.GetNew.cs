using Common;
using EnterpriseContact;
using InstantMessage.Entities;
using InstantMessage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Notification;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace InstantMessage.Controllers
{
    public partial class InstantMessageController
    {

        /// <summary>
        /// 拉取新消息
        /// </summary>
        /// <param name="model">拉取消息参数</param>
        /// <returns></returns>
        [HttpPost("message/getNew")]
        public async Task<ActionResult<List<ConversationVm>>> GetNewMessageListAsync(List<MessageNotifyDto> model)
        {
            try
            {
                if (model == null || model.Count == 0)
                {
                    return BadRequest();
                }

                //去重 唯一conversation
                var dictNotifies = new Dictionary<Guid, MessageNotifyDto>();

                foreach (var item in model)
                {
                    var id = item.TargetId.ToGuid();
                    if (!dictNotifies.ContainsKey(id))
                    {
                        dictNotifies.Add(id, item);
                    }
                }

                var convVms = await GenerateConversationVmsAsync(dictNotifies.Keys.ToList());

                var convMsgs = await GetConversationMsgsAsync(dictNotifies.Values.ToList());

                await GenerateMessagesAsync(convVms, convMsgs);

                return convVms;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        private async Task GenerateMessagesAsync(List<ConversationVm> convVms, List<ConversationMsg> convMsgs)
        {
            var tasks = new List<Task<MessageVm>>();
            convMsgs.ForEach(o => tasks.Add(BuildMessageVmAsync(o)));
            await Task.WhenAll(tasks);

            tasks.ForEach(o =>
            {
                var msgVm = o.Result;
                var convVm = convVms.Find((r) => r.Id == msgVm.ConversationId);
                convVm?.MessageList?.Add(msgVm);
            });
        }

        private async Task<MessageVm> BuildMessageVmAsync(ConversationMsg msg)
        {
            var dtos = await _employeeMappingService.ByUserIdAsync(msg.SenderId);

            var name = (dtos == null || !dtos.Any()) ? "未知用户" : dtos.First().Name;
            var gender = (dtos == null || !dtos.Any()) ? 0 : dtos.First().Gender;
            var message = MessageVm.Create(msg).SetSenderInfo(name, gender);
            return message;
        }

        private async Task<List<ConversationVm>> GenerateConversationVmsAsync(List<Guid> input)
        {
            var currentUserId = GetUserId();

            var result = new List<ConversationVm>();

            var conversations = await _conversationCtrlAppService.GetByIdsAsync(input);

            #region generate p2p vm
            foreach (var c in conversations.Where(x => x.Type == ConversationType.P2P))
            {
                var vm = ConversationVm.Create(c);
                var dest = c.Participants.FirstOrDefault(o => !o.Equals(currentUserId));
                var dtos = await _employeeMappingService.ByUserIdAsync(dest);
                vm.Topic = (dtos == null || !dtos.Any()) ? "未知用户" : dtos.First().Name;
                result.Add(vm);
            }
            #endregion

            #region generate customgroup vm
            var ids = new List<Guid>();
            conversations.Where(x => x.Type == ConversationType.CustomGroup).ForEach(x =>
            {
                var vm = ConversationVm.Create(x);
                vm.Topic = "[未获取到群组名称]";
                result.Add(vm);
                ids.Add(x.Id);
            });
            if (ids.Count > 0)
            {
                var groups = await _groupAppService.GetListByIdsAsync(ids);
                groups.RemoveIfContains(null);
                foreach (var item in groups)
                {
                    var vm = result.Find(x => x.Id == item.Id);
                    vm.Topic = item.Name;
                }
            }
            #endregion

            #region generate departmentgroup vm
            ids = new List<Guid>();
            conversations.Where(x => x.Type == ConversationType.DepartmentGroup).ForEach(x =>
            {
                var vm = ConversationVm.Create(x);
                vm.Topic = "[未获取到群组名称]";
                result.Add(vm);
                ids.Add(x.Id);
            });
            if (ids.Count > 0)
            {
                var departments = await _departmentAppService.GetListByIdsAsync(ids);
                departments.RemoveIfContains(null);
                foreach (var item in departments)
                {
                    var vm = result.Find(x => x.Id == item.Id);
                    vm.Topic = item.Name;
                }
            }
            #endregion

            return result;
        }

        private async Task<List<ConversationMsg>> GetConversationMsgsAsync(List<MessageNotifyDto> input)
        {
            var tasks = new List<Task<List<ConversationMsg>>>();
            foreach (var appService in await _allConversationMsgAppServiceFactory())
            {
                tasks.Add(appService.GetMessagesAsync(input));
            }
            await Task.WhenAll(tasks);
            var result = new List<ConversationMsg>();
            tasks.ForEach(o => result.AddRange(o.Result));
            return result;
        }

    }
}
