using Common;
using EnterpriseContact;
using EnterpriseContact.Services;
using InstantMessage.Entities;
using InstantMessage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Notification;
using Serilog;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstantMessage.Controllers
{
    [Route("api/v1/im")]
    public partial class InstantMessageController : EapBaseController
    {
        private readonly Func<Guid, IConversationMsgAppService> _conversationMsgAppServiceFactory;
        private readonly ILogger _logger;
        private readonly IConversationCtrlAppService _conversationCtrlAppService;
        private readonly Func<Task<IEnumerable<IConversationMsgAppService>>> _allConversationMsgAppServiceFactory;
        private readonly IEmployeeCacheService _employeeMappingService;
        private readonly IGroupAppService _groupAppService;
        private readonly IDepartmentAppService _departmentAppService;

        public InstantMessageController(
            IConversationCtrlAppService conversationCtrlAppService,
            Func<Guid, IConversationMsgAppService> conversationMsgAppServiceFactory,
            Func<Task<IEnumerable<IConversationMsgAppService>>> allConversationMsgAppServiceFactory,
            IEmployeeCacheService employeeMappingService,
            IGroupAppService groupAppService,
            IDepartmentAppService departmentAppService
            )
        {
            _conversationCtrlAppService = conversationCtrlAppService;
            _conversationMsgAppServiceFactory = conversationMsgAppServiceFactory;
            _logger = Log.ForContext<InstantMessageController>();
            _allConversationMsgAppServiceFactory = allConversationMsgAppServiceFactory;
            _employeeMappingService = employeeMappingService;
            _groupAppService = groupAppService;
            _departmentAppService = departmentAppService;
        }

        /// <summary>
        /// 消息记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("message/getHistory")]
        public async Task<ActionResult<List<MessageVm>>> GetMessageHistory(MessageNotifyDto model)
        {
            try
            {
                var service = _conversationMsgAppServiceFactory(model.TargetId.ToGuid());
                var messages = await service.GetOldMessagesAsync(new GetOldMessagesInput()
                {
                    Id = model.TargetId.ToGuid(),
                    OldestMsgId = model.LatestMsgId,
                    //Type = Enum.Parse<ConversationType>(model.TargetCategory.ToString())
                });

                var tasks = new List<Task<MessageVm>>();
                foreach (var msg in messages)
                {
                    tasks.Add(BuildMessageVmAsync(msg));
                }
                await Task.WhenAll(tasks);
                return tasks.Select(o => o.Result).ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        private async Task<SendMessageResult> SendAsync(ConversationMsg msg, ConversationType conversationType)
        {
            var entity = await _conversationCtrlAppService.GetByIdAsync(msg.ConversationId);
            var conversationMsgAppService = _conversationMsgAppServiceFactory(msg.ConversationId);
            if (entity.HasValue && !entity.Value.IsDeleted)
                if (entity.Value.Participants.Contains(msg.SenderId))
                {
                    await conversationMsgAppService.SendMessageAsync(msg);

                    return new SendMessageResult()
                    {
                        Success = true,
                        Message = "发送消息成功",
                        MessageId = msg.Id,
                        SendTime = msg.Time
                    };
                }
                else
                    return new SendMessageResult()
                    {
                        Success = false,
                        Message = conversationType == ConversationType.P2P ? "用户不属于此会话..." : "用户不在群组中..."
                    };
            else
                return new SendMessageResult()
                {
                    Success = false,
                    Message = "对话不存在"
                };
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("message/sendText")]
        public async Task<ActionResult<SendMessageResult>> SendTextMessageAsync(TextMessageVm model)
        {
            try
            {
                var msg = ConversationMsg.Create(model.ConversationId, model.SenderId, model.Content, ConversationMsgType.Text);

                var result = await SendAsync(msg, model.ConversationType);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("message/sendImage")]
        public async Task<ActionResult<SendMessageResult>> SendImageMessageAsync(ImageMessageVm model)
        {
            try
            {
                var msg = ConversationMsg.Create(model.ConversationId, model.SenderId, model.GetJsonContent(),
                    ConversationMsgType.Image);

                var result = await SendAsync(msg, model.ConversationType);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 发送文件消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("message/sendFile")]
        public async Task<ActionResult<SendMessageResult>> SendFileMessageAsync(FileMessageVm model)
        {
            try
            {
                var msg = ConversationMsg.Create(model.ConversationId, model.SenderId, model.GetJsonContent(),
                    ConversationMsgType.File);

                var result = await SendAsync(msg, model.ConversationType);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("message/sendVoice")]
        public async Task<ActionResult<SendMessageResult>> SendVoiceMessageAsync(VoiceMessageVm model)
        {
            try
            {
                var msg = ConversationMsg.Create(model.ConversationId, model.SenderId, model.GetJsonContent(),
                    ConversationMsgType.Voice);

                var result = await SendAsync(msg, model.ConversationType);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 创建P2P会话
        /// </summary>
        /// <param name="param">参数</param>
        /// <remarks>参数中的用户Id均为单点登录返回的Sub</remarks>
        /// <returns>返回数据中的data就是会话Id</returns>
        [HttpPost("p2p")]
        public async Task<ActionResult<Guid>> CreateP2PConverationAsync(CreateP2PConversationVm model)
        {
            try
            {
                //NOTE 此处暂时不验证白名单，依赖客户端的控制
                var conv = await _conversationCtrlAppService.GetOrAddP2PAsync(model.SenderId, model.RecieverId);
                return conv.Id;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 删除现有的P2P会话，在白名单验证失效后，可以避免被打扰
        /// </summary>
        [HttpDelete("p2p/{id}")]
        public async Task<IActionResult> DeleteP2PConverationAsync(Guid id)
        {
            try
            {
                await _conversationCtrlAppService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 同步客户端和服务器的时间差
        /// </summary>
        /// <returns></returns>
        [HttpGet("time")]
        public ActionResult<TimeSpan> SyncTimeDiff(DateTimeOffset current)
        {
            var diff = DateTimeOffset.UtcNow - current;
            return diff;
        }

        /// <summary>
        /// 创建部门会话
        /// </summary>
        /// <param name="departmentId">部门Id，即为会话Id</param>
        /// <returns>Status 1:不是现存会话的成员 2:不是部门成员 3:只有一个成员无法创建会话 4:现存的会话不是部门会话</returns>
        [HttpPost("dep")]
        public async Task<ActionResult<ResponseData>> CreateDepConverationAsync([FromQuery]Guid departmentId)
        {
            try
            {
                var userId = GetUserId();
                var conv = await _conversationCtrlAppService.GetByIdAsync(departmentId);
                if (conv.HasValue)
                {
                    if (conv.Value.Type != ConversationType.DepartmentGroup)
                        return BuildFaild(4);
                    if (conv.Value.Participants.Contains(userId))
                        return BuildSuccess();
                    return BuildFaild(1);
                }
                else
                {
                    var result = await _conversationCtrlAppService.AddDepAsync(new AddDepConversationInput
                    {
                        UserId = userId,
                        DepartmentId = departmentId
                    });

                    if (result.FailedCode == 1)
                        return BuildFaild(2);
                    if (result.FailedCode == 2)
                        return BuildFaild(3);
                    return BuildSuccess();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }
    }
}
