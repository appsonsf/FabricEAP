using Base.Eap.Notify.MsgContracts;
using ConfigMgmt;
using ConfigMgmt.Entities;
using EnterpriseContact.Services;
using MassTransit;
using Nest;
using Notification;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizSystemNotifyService.EventBus
{
    public class BizSystemNotifyMsgConsumer : IConsumer<BizSystemNotifyMsg>
    {
        private readonly IBizSystemAppService _bizSystemAppService;
        private readonly Func<Guid, INotifySessionActor> _notifySessionActorFactory;
        private readonly IEmployeeCacheService _employeeCacheService;
        private readonly ILogger _logger;

        public BizSystemNotifyMsgConsumer(IBizSystemAppService bizSystemAppService,
           Func<Guid, INotifySessionActor> notifySessionActorFactory,
           IEmployeeCacheService employeeCacheService)
        {
            _bizSystemAppService = bizSystemAppService;
            _notifySessionActorFactory = notifySessionActorFactory;
            _employeeCacheService = employeeCacheService;
            _logger = Log.ForContext<BizSystemNotifyMsgConsumer>();
        }

        private BizSystemNotifyMsg msg;
        private MsgNotifyBizSystem bizSystem;

        public async Task Consume(ConsumeContext<BizSystemNotifyMsg> context)
        {
            if (string.IsNullOrEmpty(context.Message.SystemId))
                throw new ArgumentNullException(nameof(context.Message.SystemId));

            msg = context.Message;

            bizSystem = await _bizSystemAppService.GetByIdForNotifyMsgAsync(msg.SystemId);

            if (bizSystem == null)
            {
                _logger.Warning("BizSystem is not found: {BizSystemId}", msg.SystemId);
                return;
            }

            if (bizSystem.Approachs == null || bizSystem.Approachs.Length == 0) return;

            if (bizSystem.UseAllApproachs)
            {
                foreach (var approach in bizSystem.Approachs)
                {
                    await ProcessApproachAsync(context, approach);
                }
            }
            else
            {
                var approach = bizSystem.Approachs[0];
                await ProcessApproachAsync(context, approach);
            }

            /* 1 get bizsystem
             * 2 approach is APP:
             * 2.1 get userid for receiver
             * 2.2 get session actor for user
             * 2.3 pushnotify into actor
             * 3 approach is SMS:
             * 3.1 get mobile for receiver
             * 3.2 send sms msg into CEF queue
             * 4 user is online
             * 4.1 send notify into notify queue
             * 5 user is offline
             * 5.1 get device
             * 5.1 send push into push queue
             * 6 notify queue get msg
             * 6.1 send via SignalR
             * 7 push queue get msg
             * 7.1 send via Azure Notification Hubs
            */
        }

        private async Task ProcessApproachAsync(ISendEndpointProvider sendEndpoint, MsgNotifyApproach approach)
        {
            if (approach == MsgNotifyApproach.APP)
            {
                var actors = await GetUserSessionActorsAsync();
                var notify = CreateNotify();
                var tasks = actors.Select(o => o.PushEventNotifyAsync(notify));
                await Task.WhenAll(tasks);
            }
            else if (approach == MsgNotifyApproach.SMS)
            {
                throw new NotSupportedException("SMS notification coming soon");
                string[] numbers = GetUserMobileNumbers();
                var sms = CreateSms();
                await sendEndpoint.Send(sms);
            }
        }

        private SendNotifyCommandImpl CreateSms()
        {
            throw new NotImplementedException();
        }

        private string[] GetUserMobileNumbers()
        {
            throw new NotImplementedException();
        }

        private EventNotifyDto CreateNotify()
        {
            var dto = new EventNotifyDto
            {
                Target = NotifyTargetType.BizSystem,
                TargetId = msg.SystemId,
                TargetCategory = msg.Category,
                Created = DateTimeOffset.UtcNow,
                Title = bizSystem.Name
            };
            if (string.IsNullOrEmpty(msg.Content) && bizSystem.PatternOfCategory.ContainsKey(msg.Category))
                dto.Text = CreateTextFromPattern(bizSystem.PatternOfCategory[msg.Category], msg.Parameters);
            else
            {
                dto.Text = msg.Content;
                dto.Parameters = msg.Parameters;
            }
            return dto;
        }

        private string CreateTextFromPattern(string pattern, Dictionary<string, string> parameters)
        {
            foreach (var kvp in parameters)
            {
                var p = "$(" + kvp.Key + ")";
                pattern = pattern.Replace(p, kvp.Value);
            }
            return pattern;
        }

        private async Task<IEnumerable<INotifySessionActor>> GetUserSessionActorsAsync()
        {
            var userIds = msg.ReceiverUserIds?.Length > 0
                ? msg.ReceiverUserIds
                : msg.ReceiverNumbers?.Length > 0
                    ? (await _employeeCacheService.ByNumberAsync(msg.ReceiverNumbers)).Where(o => o.UserId.HasValue).Select(o => o.UserId.Value)
                    : new Guid[0];
            return userIds.Select(o => _notifySessionActorFactory(o));
        }
    }

    public class SendNotifyCommandImpl : SendNotifyCommand
    {
        public string[] PhoneNumbers { get; set; }
        public string TemplateName { get; set; }
        public Dictionary<string, string> TemplateParam { get; set; }
    }

    interface SendNotifyCommand
    {
        string[] PhoneNumbers { get; set; }
        string TemplateName { get; set; }
        Dictionary<string, string> TemplateParam { get; set; }
    }
}
