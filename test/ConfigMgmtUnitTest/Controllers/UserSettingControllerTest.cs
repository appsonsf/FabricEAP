using Common;
using ConfigMgmt;
using ConfigMgmt.Controllers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;
using UnitTestCommon;

namespace ConfigMgmtUnitTest.Controllers
{
    [TestClass]
    public class UserSettingControllerTest : ControllerTestBase
    {
        [TestMethod]
        public async Task TestSetInfoVisibility()
        {
            var value = new InfoVisibility() { Mobile = true };
            var userSettingAppService = Substitute.For<IUserSettingAppService>();
            userSettingAppService.SetInfoVisibilityAsync(User_Id, value)
               .Returns(Task.CompletedTask);

            var target = new UserSettingController(_ => userSettingAppService);
            target.ControllerContext = CreateMockContext();

            var result = await target.SetInfoVisibility(value);
            var data = result.Value;
            data.Should().BeOfType<ResponseData>();
            data.Status.Should().Be(0);
        }

        [TestMethod]
        public async Task TestGetInfoVisibility()
        {
            var value = new InfoVisibility() { Mobile = true };
            var userSettingAppService = Substitute.For<IUserSettingAppService>();
            userSettingAppService.GetInfoVisibilityAsync(User_Id)
               .Returns(Task.FromResult(value));

            var target = new UserSettingController(_ => userSettingAppService);
            target.ControllerContext = CreateMockContext();

            var result = await target.GetInfoVisibility();
            var data = result.Value;
            data.Should().BeOfType<InfoVisibility>();
            data.Mobile.Should().BeTrue();
        }
    }
}
