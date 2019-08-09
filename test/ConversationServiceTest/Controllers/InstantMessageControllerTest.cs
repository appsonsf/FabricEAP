using Common;
using EnterpriseContact;
using EnterpriseContact.Services;
using FluentAssertions;
using InstantMessage;
using InstantMessage.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Routing.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitTestCommon;
using System.Linq;
using InstantMessage.Entities;
using ServiceFabricContrib;

namespace ConversationServiceTest.Controllers
{
    [TestClass]
    public class InstantMessageControllerTest : ControllerTestBase
    {
        [TestMethod]
        public async Task CreateDepConverationAsync()
        {
            var conversationCtrlAppService = Substitute.For<IConversationCtrlAppService>();
            conversationCtrlAppService.GetByIdAsync(Arg.Any<Guid>())
                .Returns(default(Conversation?));
            conversationCtrlAppService.AddDepAsync(Arg.Any<AddDepConversationInput>())
                .Returns(RemotingResult.Success());

            Task<IEnumerable<IConversationMsgAppService>> getConversationMsgAppServices()
            {
                var conversationMsgAppServices = new List<IConversationMsgAppService> { Substitute.For<IConversationMsgAppService>() };
                return Task.FromResult(conversationMsgAppServices.AsEnumerable());
            }

            var target = new InstantMessageController(conversationCtrlAppService,
                _ => Substitute.For<IConversationMsgAppService>(),
                getConversationMsgAppServices,
                Substitute.For<IEmployeeCacheService>(),
                Substitute.For<IGroupAppService>(),
                Substitute.For<IDepartmentAppService>())
            {
                ControllerContext = CreateMockContext()
            };

            var result = await target.CreateDepConverationAsync(Guid.NewGuid());
            result.Value.Should().BeOfType<ResponseData>();
            result.Value.Status.Should().Be(0);

            var depId = Guid.NewGuid();
            conversationCtrlAppService.GetByIdAsync(depId)
             .Returns(new Conversation
             {
                 Id = depId,
                 Participants = new List<Guid> { User_Id },
                 Type = ConversationType.DepartmentGroup
             });
            result = await target.CreateDepConverationAsync(depId);
            result.Value.Status.Should().Be(0);

            conversationCtrlAppService.GetByIdAsync(depId)
            .Returns(new Conversation
            {
                Id = depId,
                Participants = new List<Guid> { Guid.NewGuid() },
                Type = ConversationType.DepartmentGroup
            });
            result = await target.CreateDepConverationAsync(depId);
            result.Value.Status.Should().Be(1);

            conversationCtrlAppService.GetByIdAsync(depId)
               .Returns(default(Conversation?));
            conversationCtrlAppService.AddDepAsync(Arg.Is<AddDepConversationInput>(x => x.DepartmentId == depId && x.UserId == User_Id))
               .Returns(RemotingResult.Fail(1));
            result = await target.CreateDepConverationAsync(depId);
            result.Value.Status.Should().Be(2);

            conversationCtrlAppService.AddDepAsync(Arg.Is<AddDepConversationInput>(x => x.DepartmentId == depId && x.UserId == User_Id))
               .Returns(RemotingResult.Fail(2));
            result = await target.CreateDepConverationAsync(depId);
            result.Value.Status.Should().Be(3);
        }
    }
}
