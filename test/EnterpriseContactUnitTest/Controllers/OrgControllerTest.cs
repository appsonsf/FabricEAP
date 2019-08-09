using EnterpriseContact;
using EnterpriseContact.Controllers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseContactUnitTest.Controllers
{
    [TestClass]
    public class OrgControllerTest : TheControllerTestBase
    {
        [TestMethod]
        public async Task TestGetRoots()
        {
            var departments = new List<DepartmentListOutput>
            {
                new DepartmentListOutput
                {
                    Id=Guid.NewGuid(),
                    Name="sclqjt",
                    ParentId=new Guid("7e4dcb5d91d24cd9923d965224c2f396"),
                },
                new DepartmentListOutput
                {
                    Id=Guid.NewGuid(),
                    Name="sclqgs",
                    ParentId=new Guid("7e4dcb5d91d24cd9923d965224c2f396"),
                }
            };

            var mockDepartmentAppService = new Mock<IDepartmentAppService>();
            mockDepartmentAppService.Setup(o => o.GetRootListAsync())
                .ReturnsAsync(departments);

            var mockEmployeeAppService = new Mock<IEmployeeAppService>();
            mockEmployeeAppService.Setup(o => o.GetRootListAsync())
                .ReturnsAsync(new List<EmployeeListOutput>());

            var target = new OrgController(
                CreateMemoryCache(),
                CreateMapper(),
                mockDepartmentAppService.Object,
                new Mock<IPositionAppService>().Object,
                mockEmployeeAppService.Object
                );

            var result = await target.GetRoots();

            var data = result.Value;
            data.Departments.Count.Should().Be(2);
            data.Departments[0].Id.Should().Be(departments[0].Id);
            data.Departments[0].Name.Should().Be(departments[0].Name);
        }
    }
}
