using Base.Eap.Notify.MsgContracts;
using BizSystemNotifyService.EventBus;
using ConfigMgmt;
using ConfigMgmt.Entities;
using EnterpriseContact;
using EnterpriseContact.Services;
using MassTransit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notification;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotifyUnitTest
{
    [TestClass]
    public class BizSystemNotifyMsgConsumerTest
    {
        [TestMethod]
        public async Task Consume_UseContent()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var bizSystemAppService = Substitute.For<IBizSystemAppService>();
            bizSystemAppService.GetByIdForNotifyMsgAsync("aaa")
                .Returns(new MsgNotifyBizSystem
                {
                    Id = "aaa",
                    Approachs = new MsgNotifyApproach[] { MsgNotifyApproach.APP },
                    Name = "AAA System",
                });

            var notifySessionActor = Substitute.For<INotifySessionActor>();
            var employeeCacheService = Substitute.For<IEmployeeCacheService>();
            var context = Substitute.For<ConsumeContext<BizSystemNotifyMsg>>();
            context.Message.Returns(new BizSystemNotifyMsg
            {
                SystemId = "aaa",
                Category = 1,
                Content = "show me the money",
                ReceiverUserIds = new Guid[] { id1, id2 }
            });

            var target = new BizSystemNotifyMsgConsumer(bizSystemAppService,
               _ => notifySessionActor, employeeCacheService);

            await target.Consume(context);

            await notifySessionActor.Received(2).PushEventNotifyAsync(Arg.Is<EventNotifyDto>(
                x => x.TargetId == "aaa" && x.Title == "AAA System"));
        }

        [TestMethod]
        public async Task Consume_UseParameter()
        {
            var num1 = Guid.NewGuid().ToString("N");
            var num2 = Guid.NewGuid().ToString("N");

            var bizSystemAppService = Substitute.For<IBizSystemAppService>();
            bizSystemAppService.GetByIdForNotifyMsgAsync("aaa")
                .Returns(new MsgNotifyBizSystem
                {
                    Id = "aaa",
                    Approachs = new MsgNotifyApproach[] { MsgNotifyApproach.APP },
                    Name = "AAA System",
                    PatternOfCategory = new Dictionary<int, string> {
                        {1,"$(p1)+$(p2)" }
                    }
                });

            var notifySessionActor = Substitute.For<INotifySessionActor>();

            var employeeCacheService = Substitute.For<IEmployeeCacheService>();
            employeeCacheService.ByNumberAsync(num1, num2)
                .Returns(new List<EmployeeCacheOutput>
                {
                    new EmployeeCacheOutput
                    {
                        Id=Guid.NewGuid(),
                        Number=num1,
                        UserId=Guid.NewGuid(),
                    },
                    new EmployeeCacheOutput
                    {
                        Id=Guid.NewGuid(),
                        Number=num2,
                        UserId=Guid.NewGuid(),
                    }
                });

            var context = Substitute.For<ConsumeContext<BizSystemNotifyMsg>>();
            context.Message.Returns(new BizSystemNotifyMsg
            {
                SystemId = "aaa",
                Category = 1,
                Parameters = new Dictionary<string, string>
                {
                    {"p1","xyz" },
                    {"p2","123" }
                },
                ReceiverNumbers = new string[] { num1, num2 }
            });

            var target = new BizSystemNotifyMsgConsumer(bizSystemAppService,
               _ => notifySessionActor, employeeCacheService);

            await target.Consume(context);

            await notifySessionActor.Received(2).PushEventNotifyAsync(Arg.Is<EventNotifyDto>(
                x => x.TargetId == "aaa" && x.Title == "AAA System" && x.Text == "xyz+123"));
        }
    }
}
