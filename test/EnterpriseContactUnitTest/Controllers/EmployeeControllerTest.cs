using ConfigMgmt;
using EnterpriseContact;
using EnterpriseContact.Controllers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseContactUnitTest.Controllers
{
    [TestClass]
    public class EmployeeControllerTest : TheControllerTestBase
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
            var pos1 = new PositionListOutput
            {
                Id = Guid.NewGuid(),
                Name = "pos1",
                DepartmentId = dep1.Id,
            };
            var pos2 = new PositionListOutput
            {
                Id = Guid.NewGuid(),
                Name = "pos2",
                DepartmentId = dep2.Id,
            };
            var emp0 = new EmployeeListOutput
            {
                Id = Guid.NewGuid(),
                Name = "aaa",
                PrimaryDepartmentId = dep1.Id,
                PrimaryPositionId = pos1.Id,
            };
            var emp1 = new EmployeeListOutput
            {
                Id = Guid.NewGuid(),
                Name = "aabb",
                PrimaryDepartmentId = dep1.Id,
                PrimaryPositionId = pos1.Id,
            };
            var emp2 = new EmployeeListOutput
            {
                Id = Guid.NewGuid(),
                Name = "bbcc",
                PrimaryDepartmentId = dep2.Id,
                PrimaryPositionId = pos2.Id,
            };

            var departmentAppService = Substitute.For<IDepartmentAppService>();
            departmentAppService.GetAllListAsync()
                .Returns(Task.FromResult((new[] { dep0, dep1, dep2 }).ToList()));

            var positionAppService = Substitute.For<IPositionAppService>();
            positionAppService.GetAllListAsync()
                .Returns(Task.FromResult((new[] { pos1, pos2 }).ToList()));

            var employeeAppService = Substitute.For<IEmployeeAppService>();
            employeeAppService.SearchByKeywordAsync("aa")
                .Returns(Task.FromResult((new[] { emp0, emp1 }).ToList()));
            employeeAppService.SearchByKeywordAsync("bb")
                .Returns(Task.FromResult((new[] { emp1, emp2 }).ToList()));

            var target = new EmployeeController(
                CreateMemoryCache(),
                CreateMapper(),
                departmentAppService,
                positionAppService,
                employeeAppService,
                Substitute.For<IGroupAppService>(),
                _ => Substitute.For<IUserFavoriteAppService>(),
                _ => Substitute.For<IUserSettingAppService>());

            var result = await target.Search("aa");
            var data = result.Value;
            data.Count.Should().Be(2);
            data[0].Id.Should().Be(emp0.Id);
            data[0].PositionName.Should().Be(pos1.Name);
            data[0].DepartmentNames.Count.Should().Be(2);
            data[0].DepartmentNames[0].Should().Be(dep1.Name);
            data[0].DepartmentNames[1].Should().Be(dep0.Name);

            result = await target.Search("bb");
            data = result.Value;
            data.Count.Should().Be(2);
            data[1].Id.Should().Be(emp2.Id);
            data[1].PositionName.Should().Be(pos2.Name);
            data[1].DepartmentNames.Count.Should().Be(2);
            data[1].DepartmentNames[0].Should().Be(dep2.Name);
            data[1].DepartmentNames[1].Should().Be(dep0.Name);
        }

        [TestMethod]
        public async Task Test_GetById_GetByUserId()
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
            var pos1 = new PositionListOutput
            {
                Id = Guid.NewGuid(),
                Name = "pos1",
                DepartmentId = dep1.Id,
            };
            var pos2 = new PositionListOutput
            {
                Id = Guid.NewGuid(),
                Name = "pos2",
                DepartmentId = dep2.Id,
            };
            var emp0 = new EmployeeOutput
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "aaa",
                PrimaryDepartmentId = dep1.Id,
                PrimaryPositionId = pos1.Id,
                ParttimePositionIds = new List<Guid> { pos2.Id },
            };

            var departmentAppService = Substitute.For<IDepartmentAppService>();
            departmentAppService.GetAllListAsync()
                .Returns(Task.FromResult((new[] { dep0, dep1, dep2 }).ToList()));

            var positionAppService = Substitute.For<IPositionAppService>();
            positionAppService.GetAllListAsync()
                .Returns(Task.FromResult((new[] { pos1, pos2 }).ToList()));

            var employeeAppService = Substitute.For<IEmployeeAppService>();
            employeeAppService.GetByIdAsync(emp0.Id)
                .Returns(Task.FromResult(emp0));
            employeeAppService.GetByUserIdAsync(emp0.UserId.Value)
                .Returns(Task.FromResult(emp0));

            var groupAppService = Substitute.For<IGroupAppService>();
            groupAppService.CheckSameWhiteListGroupAsync(User_EmployeeMdmId, emp0.Id)
                .Returns(Task.FromResult(true));

            var userFavoriteAppService = Substitute.For<IUserFavoriteAppService>();
            userFavoriteAppService.IsFavoritedAsync(User_Id, emp0.Id)
                .Returns(Task.FromResult(true));

            var userSettingAppService = Substitute.For<IUserSettingAppService>();
            userSettingAppService.GetInfoVisibilityAsync(emp0.UserId.Value)
                .Returns(Task.FromResult(new InfoVisibility { Mobile = false }));

            var target = new EmployeeController(
                CreateMemoryCache(),
                CreateMapper(),
                departmentAppService,
                positionAppService,
                employeeAppService,
                groupAppService,
                _ => userFavoriteAppService,
                _ => userSettingAppService
                );
            target.ControllerContext = CreateMockContext();

            var result = await target.GetById(emp0.Id);
            var data = result.Value;
            data.SameWhiteListGroup.Should().BeTrue();
            data.IsFavorited.Should().BeTrue();
            data.Name.Should().Be(emp0.Name);
            data.Mobile.Should().Be("***");
            data.PositionName.Should().Be(pos1.Name);

            data.DepartmentNames.Count.Should().Be(2);
            data.DepartmentNames[0].Should().Be(dep1.Name);

            data.ParttimeJobs.Count.Should().Be(1);
            data.ParttimeJobs[0].PositionName.Should().Be(pos2.Name);
            data.ParttimeJobs[0].DepartmentNames.Count.Should().Be(2);
            data.ParttimeJobs[0].DepartmentNames[0].Should().Be(dep2.Name);

            result = await target.GetByUserId(emp0.UserId.Value);
            data = result.Value;
            data.Should().NotBeNull();
        }

        [TestMethod]
        public async Task TestGetMy()
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
            var pos1 = new PositionListOutput
            {
                Id = Guid.NewGuid(),
                Name = "pos1",
                DepartmentId = dep1.Id,
            };
            var pos2 = new PositionListOutput
            {
                Id = Guid.NewGuid(),
                Name = "pos2",
                DepartmentId = dep2.Id,
            };
            var emp0 = new EmployeeListOutput
            {
                Id = Guid.NewGuid(),
                Name = "aaa",
                PrimaryDepartmentId = dep1.Id,
                PrimaryPositionId = pos1.Id,
            };
            var emp1 = new EmployeeListOutput
            {
                Id = Guid.NewGuid(),
                Name = "aabb",
                PrimaryDepartmentId = dep1.Id,
                PrimaryPositionId = pos1.Id,
            };
            var emp2 = new EmployeeListOutput
            {
                Id = Guid.NewGuid(),
                Name = "bbcc",
                PrimaryDepartmentId = dep2.Id,
                PrimaryPositionId = pos2.Id,
            };

            var departmentAppService = Substitute.For<IDepartmentAppService>();
            departmentAppService.GetAllListAsync()
                .Returns(Task.FromResult((new[] { dep0, dep1, dep2 }).ToList()));

            var positionAppService = Substitute.For<IPositionAppService>();
            positionAppService.GetAllListAsync()
                .Returns(Task.FromResult((new[] { pos1, pos2 }).ToList()));

            var employeeIds = new[] { emp0.Id, emp1.Id, emp2.Id };
            var employeeAppService = Substitute.For<IEmployeeAppService>();
            employeeAppService.GetListByIdsAsync(employeeIds)
                .Returns(Task.FromResult((new[] { emp0, emp1, emp2 }).ToList()));

            var userFavoriteAppService = Substitute.For<IUserFavoriteAppService>();
            userFavoriteAppService.GetEmployeesAsync(User_Id)
                .Returns(Task.FromResult(employeeIds));

            var target = new EmployeeController(
                CreateMemoryCache(),
                CreateMapper(),
                departmentAppService,
                positionAppService,
                employeeAppService,
                Substitute.For<IGroupAppService>(),
                _ => userFavoriteAppService,
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.GetMy();
            var data = result.Value;
            data.Count.Should().Be(3);
        }

        [TestMethod]
        public async Task TestAddFavorite()
        {
            var userFavoriteAppService = Substitute.For<IUserFavoriteAppService>();
            userFavoriteAppService.AddEmployeeAsync(User_Id, Arg.Any<Guid>())
                .Returns(Task.FromResult(true));

            var target = new EmployeeController(
                CreateMemoryCache(),
                CreateMapper(),
                Substitute.For<IDepartmentAppService>(),
                Substitute.For<IPositionAppService>(),
                Substitute.For<IEmployeeAppService>(),
                Substitute.For<IGroupAppService>(),
                _ => userFavoriteAppService,
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.AddFavorite(Guid.NewGuid());
            result.Value.Status.Should().Be(0);
        }

        [TestMethod]
        public async Task TestRemoveFavorite()
        {
            var userFavoriteAppService = Substitute.For<IUserFavoriteAppService>();
            userFavoriteAppService.RemoveEmployeeAsync(User_Id, Arg.Any<Guid>())
                .Returns(Task.FromResult(true));

            var target = new EmployeeController(
                CreateMemoryCache(),
                CreateMapper(),
                Substitute.For<IDepartmentAppService>(),
                Substitute.For<IPositionAppService>(),
                Substitute.For<IEmployeeAppService>(),
                Substitute.For<IGroupAppService>(),
                _ => userFavoriteAppService,
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.RemoveFavorite(Guid.NewGuid());
            result.Value.Status.Should().Be(0);
        }

        [TestMethod]
        public async Task TestCheckSameWhiteListGroup()
        {
            var groupAppService = Substitute.For<IGroupAppService>();
            groupAppService.CheckSameWhiteListGroupAsync(User_EmployeeMdmId, Arg.Any<Guid>())
                .Returns(Task.FromResult(true));

            var target = new EmployeeController(
                CreateMemoryCache(),
                CreateMapper(),
                Substitute.For<IDepartmentAppService>(),
                Substitute.For<IPositionAppService>(),
                Substitute.For<IEmployeeAppService>(),
                groupAppService,
                _ => Substitute.For<IUserFavoriteAppService>(),
                _ => Substitute.For<IUserSettingAppService>());
            target.ControllerContext = CreateMockContext();

            var result = await target.CheckSameWhiteListGroup(Guid.NewGuid());
            result.Value.Should().BeTrue();
        }
    }
}
