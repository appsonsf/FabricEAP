using Common;
using EnterpriseContact;
using EnterpriseContact.Controllers;
using EnterpriseContact.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseContactUnitTest.Controllers
{
    [TestClass]
    public class GroupControllerTest : TheControllerTestBase
    {
        [TestMethod]
        public async Task TestGetMy()
        {
            var groups = new List<GroupListOutput>
            {
                new GroupListOutput
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                },
                new GroupListOutput
                {
                    Id = Guid.NewGuid(),
                    Name = "bbb",
                },
                new GroupListOutput
                {
                    Id = Guid.NewGuid(),
                    Name = "ccc",
                },
            };

            var groupAppService = Substitute.For<IGroupAppService>();
            groupAppService.GetListByEmployeeIdAsync(User_EmployeeMdmId)
                .Returns(Task.FromResult(groups));

            var target = new GroupController(
                CreateMemoryCache(),
                CreateMapper(),
                Substitute.For<IDepartmentAppService>(),
                Substitute.For<IPositionAppService>(),
                groupAppService);
            target.ControllerContext = CreateMockContext();

            var result = await target.GetMy();
            var data = result.Value;
            data.Count.Should().Be(3);
        }

        [TestMethod]
        public async Task TestCreate()
        {
            var groupAppService = Substitute.For<IGroupAppService>();
            groupAppService.CreateCustomAsync(Arg.Any<GroupInput>())
                .ReturnsForAnyArgs(Guid.NewGuid());

            var target = new GroupController(
                CreateMemoryCache(),
                CreateMapper(),
                Substitute.For<IDepartmentAppService>(),
                Substitute.For<IPositionAppService>(),
                groupAppService);
            target.ControllerContext = CreateMockContext();

            var result = await target.Create(new GroupEditVm
            {
                Name = "aaa",
                AddingMemberIds = new HashSet<Guid> { User_EmployeeMdmId, Guid.NewGuid() },
                AddingMemberNames = new List<string> { User_EmployeeName, "李四" }
            });

            var createdResult = result.Result.As<CreatedAtActionResult>();
            createdResult.Should().NotBeNull();
            var data = createdResult.Value.As<ResponseData<Guid>>();
            data.Should().NotBe(Guid.Empty);

            await groupAppService.ReceivedWithAnyArgs().CreateCustomAsync(
                Arg.Is<GroupInput>(o => o.CurrentUserId == User_Id && o.CurrentEmployeeId == User_EmployeeMdmId));
        }

        [TestMethod]
        public async Task TestGetById()
        {
            var output = new GroupOutput
            {
                Id = Guid.NewGuid(),
                Name = "aaa",
                Members = new List<GroupMemberOutput>
                {
                    new GroupMemberOutput
                    {
                        EmployeeId = Guid.NewGuid(),
                        EmployeeName = "aaa",
                        IsOwner = true,
                        Joined = DateTimeOffset.UtcNow,
                    },
                    new GroupMemberOutput
                    {
                        EmployeeId = Guid.NewGuid(),
                        EmployeeName = "bbb",
                        IsOwner = false,
                        Joined = DateTimeOffset.UtcNow,
                    },
                }
            };
            var groupAppService = Substitute.For<IGroupAppService>();
            groupAppService.GetByIdAsync(output.Id)
                .ReturnsForAnyArgs(output);

            var target = new GroupController(
                CreateMemoryCache(),
                CreateMapper(),
                Substitute.For<IDepartmentAppService>(),
                Substitute.For<IPositionAppService>(),
                groupAppService);
            target.ControllerContext = CreateMockContext();

            var result = await target.GetById(output.Id);
            var data = result.Value;
            data.Should().NotBeNull();
            data.Name.Should().Be("aaa");
            data.Members.Count.Should().Be(2);
            data.Members[0].EmployeeName.Should().Be("aaa");
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            var groupAppService = Substitute.For<IGroupAppService>();

            var target = new GroupController(
                CreateMemoryCache(),
                CreateMapper(),
                Substitute.For<IDepartmentAppService>(),
                Substitute.For<IPositionAppService>(),
                groupAppService);
            target.ControllerContext = CreateMockContext();

            groupAppService.UpdateAsync(Arg.Any<GroupInput>())
                .ReturnsForAnyArgs(RemotingResult.Success());
            var result = await target.Update(Guid.NewGuid(), new GroupEditVm());
            result.Value.Should().NotBeNull();
            result.Value.Status.Should().Be(0);

            groupAppService.UpdateAsync(Arg.Any<GroupInput>())
               .ReturnsForAnyArgs(RemotingResult.Fail());
            result = await target.Update(Guid.NewGuid(), new GroupEditVm());
            result.Value.Status.Should().NotBe(0);

            groupAppService.UpdateAsync(Arg.Any<GroupInput>())
              .ReturnsForAnyArgs(RemotingResult.Fail(FailedCodes.Group_NotCreatedBy));
            result = await target.Update(Guid.NewGuid(), new GroupEditVm());
            result.Value.Status.Should().Be(FailedCodes.Group_NotCreatedBy);
        }

        [TestMethod]
        public async Task TestDelete()
        {
            var groupAppService = Substitute.For<IGroupAppService>();

            var target = new GroupController(
                CreateMemoryCache(),
                CreateMapper(),
                Substitute.For<IDepartmentAppService>(),
                Substitute.For<IPositionAppService>(),
                groupAppService);
            target.ControllerContext = CreateMockContext();

            groupAppService.DeleteAsync(Arg.Any<GroupInput>())
                .ReturnsForAnyArgs(RemotingResult.Success());
            var result = await target.Delete(Guid.NewGuid());
            result.Value.Should().NotBeNull();
            result.Value.Status.Should().Be(0);

            groupAppService.DeleteAsync(Arg.Any<GroupInput>())
               .ReturnsForAnyArgs(RemotingResult.Fail());
            result = await target.Delete(Guid.NewGuid());
            result.Value.Status.Should().NotBe(0);

            groupAppService.DeleteAsync(Arg.Any<GroupInput>())
              .ReturnsForAnyArgs(RemotingResult.Fail(FailedCodes.Group_NotCreatedBy));
            result = await target.Delete(Guid.NewGuid());
            result.Value.Status.Should().Be(FailedCodes.Group_NotCreatedBy);
        }

        [TestMethod]
        public async Task TestQuit()
        {
            var groupAppService = Substitute.For<IGroupAppService>();

            var target = new GroupController(
                CreateMemoryCache(),
                CreateMapper(),
                Substitute.For<IDepartmentAppService>(),
                Substitute.For<IPositionAppService>(),
                groupAppService);
            target.ControllerContext = CreateMockContext();

            groupAppService.QuitAsync(Arg.Any<GroupInput>())
                .ReturnsForAnyArgs(RemotingResult.Success());
            var result = await target.Quit(Guid.NewGuid());
            result.Value.Should().NotBeNull();
            result.Value.Status.Should().Be(0);

            groupAppService.QuitAsync(Arg.Any<GroupInput>())
               .ReturnsForAnyArgs(RemotingResult.Fail());
            result = await target.Quit(Guid.NewGuid());
            result.Value.Status.Should().NotBe(0);
        }
    }
}
