using Common.Services;
using ConfigMgmt;
using ConfigMgmt.Controllers;
using ConfigMgmt.Entities;
using ConfigMgmt.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTestCommon;

namespace ConfigMgmtUnitTest.Controllers
{
    [TestClass]
    public class WorkbenchControllerTest : ControllerTestBase
    {
        [TestMethod]
        public async Task GetAll()
        {
            var workbenchAppService = Substitute.For<IWorkbenchAppService>();
            workbenchAppService.GetAppEntrancesAsync(Arg.Is<GetAppEntrancesInput>(o =>
                o.ClientId == Client.DefaultId && o.ClientPlatform == ClientPlatform.Android))
            .Returns(Task.FromResult(new List<AppEntranceDto>
            {
                new AppEntranceDto
                {
                    Id=Guid.NewGuid(),
                    AppId="aaa",
                },
                new AppEntranceDto
                {
                    Id=Guid.NewGuid(),
                    AppId="bbb"
                }
            }));

            var target = new WorkbenchController(
                workbenchAppService,
                Substitute.For<IClientAppService>(),
                Substitute.For<IBizSystemAppService>(),
                Substitute.For<IBadgeApiClient>(),
                Substitute.For<IMobileCodeSender>(),
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.GetAll(ClientPlatform.Android);
            result.Value.Should().NotBeNull();
            result.Value.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task GetBadgeAmount()
        {
            var config = new ComponentConfig
            {
                AuthType = ComponentDataSourceAuthType.None,
                DataSources = new List<ComponentDataSource>
                {
                    new ComponentDataSource
                    {
                        Key=ComponentDataSourceKeys.BadgeValue
                    }
                }
            };

            var workbenchAppService = Substitute.For<IWorkbenchAppService>();
            workbenchAppService.GetComponentConfigAsync(Arg.Any<Guid>())
                .Returns(Task.FromResult(config));

            var apiClient = Substitute.For<IBadgeApiClient>();
            apiClient.GetAmountAsync(config)
                .Returns(Task.FromResult<int?>(1));

            var target = new WorkbenchController(
                workbenchAppService,
                Substitute.For<IClientAppService>(),
                Substitute.For<IBizSystemAppService>(),
                apiClient,
                Substitute.For<IMobileCodeSender>(),
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.GetBadgeAmount(Guid.NewGuid());
            result.Value.Should().Be(1);
        }

        [TestMethod]
        public async Task SendAuthCode()
        {
            var mobileCodeSender = Substitute.For<IMobileCodeSender>();
            mobileCodeSender.SendAsync(Arg.Any<string>())
                .Returns("1234");
            var target = new WorkbenchController(
                Substitute.For<IWorkbenchAppService>(),
                Substitute.For<IClientAppService>(),
                Substitute.For<IBizSystemAppService>(),
                Substitute.For<IBadgeApiClient>(),
                mobileCodeSender,
              _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.SendAuthCode();
            result.Value.Should().NotBeNull();
            result.Value.Data.Should().Contain("5678");
        }

        [TestMethod]
        public async Task CheckAuthCode()
        {
            var deviceCode = Guid.NewGuid().ToString("N");
            var appEntranceId = Guid.NewGuid();

            var mobileCodeSender = Substitute.For<IMobileCodeSender>();
            mobileCodeSender.CheckAsync(Arg.Any<string>(), "1234")
                .Returns(true);

            var userSettingAppService = Substitute.For<IUserSettingAppService>();
            userSettingAppService.SetAppEntranceAuthStateAsync(User_Id, Arg.Any<AppEntranceAuthStateInput>())
                .Returns(Task.CompletedTask);

            var target = new WorkbenchController(
                Substitute.For<IWorkbenchAppService>(),
                Substitute.For<IClientAppService>(),
                Substitute.For<IBizSystemAppService>(),
                Substitute.For<IBadgeApiClient>(),
                mobileCodeSender,
                _ => userSettingAppService);
            target.ControllerContext = CreateMockContext();

            var result = await target.CheckAuthCode(new CheckAuthCodeVm
            {
                AppEntranceId = appEntranceId,
                DeviceCode = deviceCode,
                MobileCode = "1234"
            });

            result.Value.Should().BeTrue();
            await userSettingAppService.Received().SetAppEntranceAuthStateAsync(User_Id,
                Arg.Is<AppEntranceAuthStateInput>(x => x.DeviceCode == deviceCode && x.AppEntranceId == appEntranceId));
        }

        [TestMethod]
        public async Task CheckAuthStatus()
        {
            var deviceCode = Guid.NewGuid().ToString("N");
            var appEntranceId = Guid.NewGuid();

            var userSettingAppService = Substitute.For<IUserSettingAppService>();
            userSettingAppService.GetAppEntranceAuthStateAsync(User_Id, deviceCode, appEntranceId)
                .Returns(true);

            var target = new WorkbenchController(
                Substitute.For<IWorkbenchAppService>(),
                Substitute.For<IClientAppService>(),
                Substitute.For<IBizSystemAppService>(),
                Substitute.For<IBadgeApiClient>(),
                Substitute.For<IMobileCodeSender>(),
                _ => userSettingAppService);
            target.ControllerContext = CreateMockContext();

            var result = await target.CheckAuthStatus(appEntranceId, deviceCode);

            result.Value.Should().BeTrue();
            await userSettingAppService.Received(1).GetAppEntranceAuthStateAsync(User_Id, deviceCode, appEntranceId);
        }
    }
}
