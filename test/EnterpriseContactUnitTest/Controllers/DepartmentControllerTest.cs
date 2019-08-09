using EnterpriseContact;
using EnterpriseContact.Controllers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseContactUnitTest.Controllers
{
    [TestClass]
    public class DepartmentControllerTest : TheControllerTestBase
    {
        [TestMethod]
        public async Task TestSearch()
        {
            var dep0 = new DepartmentListOutput
            {
                Id = Guid.NewGuid(),
                Name = "sclq",
            };
            var dep1 = new DepartmentListOutput
            {
                Id = Guid.NewGuid(),
                Name = "sclq-jt",
                ParentId = dep0.Id,
            };
            var dep2 = new DepartmentListOutput
            {
                Id = Guid.NewGuid(),
                Name = "sclq-gs",
                ParentId = dep0.Id,
            };
            var dep3 = new DepartmentListOutput
            {
                Id = Guid.NewGuid(),
                Name = "sclq-jt-jg",
                ParentId = dep1.Id,
            };
            var departments = new List<DepartmentListOutput>(new[] { dep0, dep1, dep2, dep3 });

            var departmentAppService = Substitute.For<IDepartmentAppService>();
            departmentAppService.GetAllListAsync()
                .Returns(Task.FromResult(departments));
            var searchResult = departments.FindAll(o => o.Name.Contains("jt"));
            departmentAppService.SearchByKeywordAsync("jt")
                .Returns(Task.FromResult(searchResult));


            var target = new DepartmentController(
                CreateMemoryCache(),
                CreateMapper(),
                departmentAppService,
                Substitute.For<IPositionAppService>());

            var result = await target.Search("jt");

            var data = result.Value;
            data.Count.Should().Be(2);
            data[0].Id.Should().Be(dep1.Id);
            data[1].Id.Should().Be(dep3.Id);
        }
    }
}
