using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseContact;
using GroupFile;
using GroupFile.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GroupFileServiceTest.ControllerApi
{
    [TestClass]
    public class GroupFileControllerTest : TheGroupFileAppServiceTestBase
    {
        private const string Md5 = "xxxxxxx";
        private const string Location = "http://xxxx";

        [TestMethod]
        public async Task GetGroupFilesAsync_Test()
        {

            //var controller = new GroupFileController();
        }


        //private Mock<IControlAppService> MockControlAppService()
        //{
        //    var mock = new Mock<IControlAppService>();
        //    mock.Setup(u => u.CheckFileExistAsync(It.IsAny<string>())).ReturnsAsync(true);
        //    mock.Setup(u => u.DownloadFileAsync(It.IsAny<Guid>())).ReturnsAsync(Location);
        //    return mock;
        //}

        //private Mock<IFileStorageService> MockFileStorageService()
        //{
        //    return new Mock<IFileStorageService>();
        //}

        private Mock<IGroupAppService> MockGroupAppService()
        {
            return new Mock<IGroupAppService>();
        }

        private Mock<IEmployeeAppService> MockEmployeeAppService()
        {
            return new Mock<IEmployeeAppService>();
        }
    }
}
