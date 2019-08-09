using Common.Services;
using ConfigMgmt;
using ConfigMgmt.Controllers;
using ConfigMgmt.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTestCommon;

namespace ConfigMgmtUnitTest.Controllers
{
    [TestClass]
    public class WorkbenchControllerInitDataTest : ControllerTestBase
    {
        [TestMethod]
        public async Task TestAddAppEntrancesAsync()
        {
            var model = new List<AppEntrance>();

            var workbenchAppService = Substitute.For<IWorkbenchAppService>();
            workbenchAppService.AddOrUpdateEntrancesAsync(Arg.Is<AddEntrancesInput>(
                o => o.ClientId == Client.DefaultId && o.AppEntrances == model
                ))
                .Returns(Task.FromResult(true));

            var target = new WorkbenchController(
                workbenchAppService,
                Substitute.For<IClientAppService>(),
                Substitute.For<IBizSystemAppService>(),
                Substitute.For<IBadgeApiClient>(),
                Substitute.For<IMobileCodeSender>(),
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.AddAppEntrancesAsync(model);
            result.Should().BeOfType<OkObjectResult>();
            var realResult = result.As<OkObjectResult>();
            realResult.Value.Should().Be(true);
        }

        [TestMethod]
        public async Task TestRemoveAppEntranceAsync()
        {
            var workbenchAppService = Substitute.For<IWorkbenchAppService>();
            workbenchAppService.RemoveEntranceAsync(Client.DefaultId, Arg.Any<Guid>())
                .Returns(Task.FromResult(true));

            var target = new WorkbenchController(
                workbenchAppService,
                Substitute.For<IClientAppService>(),
                Substitute.For<IBizSystemAppService>(),
                Substitute.For<IBadgeApiClient>(),
                Substitute.For<IMobileCodeSender>(),
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.RemoveAppEntranceAsync(Guid.NewGuid());
            result.Should().BeOfType<OkObjectResult>();
            var realResult = result.As<OkObjectResult>();
            realResult.Value.Should().Be(true);
        }

        [TestMethod]
        public async Task TestUpdateAppEntranceAsync()
        {
            var model = new AppEntrance() { Id = Guid.NewGuid() };

            var workbenchAppService = Substitute.For<IWorkbenchAppService>();
            workbenchAppService.AddOrUpdateEntrancesAsync(Arg.Is<AddEntrancesInput>(
              o => o.ClientId == Client.DefaultId && o.AppEntrances[0] == model
              ))
              .Returns(Task.FromResult(true));

            var target = new WorkbenchController(
                workbenchAppService,
                Substitute.For<IClientAppService>(),
                Substitute.For<IBizSystemAppService>(),
                Substitute.For<IBadgeApiClient>(),
                Substitute.For<IMobileCodeSender>(),
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.UpdateAppEntranceAsync(model);
            result.Should().BeOfType<OkObjectResult>();
            var realResult = result.As<OkObjectResult>();
            realResult.Value.Should().Be(true);
        }

        [TestMethod]
        public async Task TestGetAppEntrancesAsync()
        {
            var workbenchAppService = Substitute.For<IWorkbenchAppService>();
            workbenchAppService.GetAppEntrancesRawByClientIdAsync(Client.DefaultId)
              .Returns(Task.FromResult(new List<AppEntrance>(){
                  new AppEntrance { Id = new Guid("0459738c0233483cabb53295e3bae783") }
              }
              ));

            var target = new WorkbenchController(
                workbenchAppService,
                Substitute.For<IClientAppService>(),
                Substitute.For<IBizSystemAppService>(),
                Substitute.For<IBadgeApiClient>(),
                Substitute.For<IMobileCodeSender>(),
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.GetAppEntrancesAsync();
            var lst = result.Value.As<List<AppEntrance>>();
            lst.Should().NotBeNullOrEmpty();
            lst[0].Id.Should().Be(new Guid("0459738c0233483cabb53295e3bae783"));
        }

        [TestMethod]
        public async Task TestAddOrUpdateTodoCenterBizSystemAsync()
        {
            var todoCenterAppService = Substitute.For<IBizSystemAppService>();
            todoCenterAppService.AddOrUpdateForTodoCenterAsync(Arg.Any<TodoCenterBizSystem>())
                .Returns(Task.FromResult(true));

            var target = new WorkbenchController(
                Substitute.For<IWorkbenchAppService>(),
                Substitute.For<IClientAppService>(),
                todoCenterAppService,
                Substitute.For<IBadgeApiClient>(),
                Substitute.For<IMobileCodeSender>(),
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.AddOrUpdateTodoCenterBizSystemAsync(new TodoCenterBizSystem
            {
                Id = "aaa"
            });
            result.Should().BeOfType<OkObjectResult>();
            var realResult = result.As<OkObjectResult>();
            realResult.Value.Should().Be(true);
        }

        [TestMethod]
        public async Task TestRemoveTodoCenterBizSystemAsync()
        {
            var todoCenterAppService = Substitute.For<IBizSystemAppService>();
            todoCenterAppService.RemoveForTodoCenterAsync(Arg.Any<string>())
                .Returns(Task.FromResult(true));

            var target = new WorkbenchController(
                Substitute.For<IWorkbenchAppService>(),
                Substitute.For<IClientAppService>(),
                todoCenterAppService,
                Substitute.For<IBadgeApiClient>(),
                Substitute.For<IMobileCodeSender>(),
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.RemoveTodoCenterBizSystemAsync("aaa");
            result.Should().BeOfType<OkObjectResult>();
            var realResult = result.As<OkObjectResult>();
            realResult.Value.Should().Be(true);
        }

        [TestMethod]
        public async Task TestGetTodoCenterBizSystemsAsync()
        {
            var model = new List<TodoCenterBizSystem>
            {
               new TodoCenterBizSystem
               {
                   Id="aaa"
               }
            };
            var todoCenterAppService = Substitute.For<IBizSystemAppService>();
            todoCenterAppService.GetAllForTodoCenterAsync()
                .Returns(Task.FromResult(model));

            var target = new WorkbenchController(
                Substitute.For<IWorkbenchAppService>(),
                Substitute.For<IClientAppService>(),
                todoCenterAppService,
                Substitute.For<IBadgeApiClient>(),
                Substitute.For<IMobileCodeSender>(),
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.GetTodoCenterBizSystemsAsync();
            var lst = result.Value.As<List<TodoCenterBizSystem>>();
            lst.Should().NotBeNullOrEmpty();
            lst[0].Id.Should().Be("aaa");
        }
    }
}
