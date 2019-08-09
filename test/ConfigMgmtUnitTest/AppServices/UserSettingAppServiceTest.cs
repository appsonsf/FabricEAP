using ConfigMgmt;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using UserConfigStateService;

namespace ConfigMgmtUnitTest.AppServices
{
    [TestClass]
    public class UserSettingAppServiceTest : TheAppServiceTestBase
    {
        [TestMethod]
        public async Task SetAndGetInfoVisibilityAsync()
        {
            var target = new UserSettingAppService(statefulServiceContext, stateManager);
            var userId = Guid.NewGuid();
            await target.SetInfoVisibilityAsync(userId, new InfoVisibility
            {
                Mobile = true
            });
            var result = await target.GetInfoVisibilityAsync(userId);
            result.Should().NotBeNull();
            result.Mobile.Should().BeTrue();

            result = await target.GetInfoVisibilityAsync(Guid.NewGuid());
            result.Should().NotBeNull();
            result.Mobile.Should().BeFalse();
        }

        [TestMethod]
        public async Task SetAndGetAppEntranceAuthStateAsync()
        {
            var target = new UserSettingAppService(statefulServiceContext, stateManager);
            var userId = Guid.NewGuid();
            var input = new AppEntranceAuthStateInput
            {
                AppEntranceId = Guid.NewGuid(),
                DeviceCode = Guid.NewGuid().ToString("N")
            };
            var result = await target.GetAppEntranceAuthStateAsync(userId, input.DeviceCode, input.AppEntranceId);
            result.Should().BeTrue();

            await target.SetAppEntranceAuthStateAsync(userId, input);
            result = await target.GetAppEntranceAuthStateAsync(userId, input.DeviceCode, input.AppEntranceId);
            result.Should().BeFalse();

            await target.SetAppEntranceAuthStateAsync(userId, input);
            await target.SetAppEntranceAuthStateAsync(userId, new AppEntranceAuthStateInput
            {
                AppEntranceId = Guid.NewGuid(),
                DeviceCode = input.DeviceCode
            });
            result = await target.GetAppEntranceAuthStateAsync(userId, input.DeviceCode, Guid.NewGuid());
            result.Should().BeTrue();

            await target.SetAppEntranceAuthStateAsync(userId, new AppEntranceAuthStateInput
            {
                AppEntranceId = Guid.NewGuid(),
                DeviceCode = Guid.NewGuid().ToString("N")
            });
            result = await target.GetAppEntranceAuthStateAsync(userId, Guid.NewGuid().ToString("N"), Guid.NewGuid());
            result.Should().BeTrue();
        }
    }
}
