using EnterpriseContact;
using EnterpriseContact.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace EnterpriseContactUnitTest.AppServices
{

    [TestClass]
    public class DepartmentAppServiceTest : TheAppServiceTestBase
    {
        [TestMethod]
        public async Task TestGetRootListAsync()
        {
            var (connection, options) = OpenDb();

            try
            {
                using (var db = new ServiceDbContext(options))
                {
                    var dep1 = new Department
                    {
                        Id = Guid.NewGuid(),
                        Number = "1",
                        Name = "sclq"
                    };
                    var dep2 = new Department
                    {
                        Id = Guid.NewGuid(),
                        ParentId = dep1.Id,
                        Number = "2",
                        Name = "gs"
                    };
                    var dep3 = new Department
                    {
                        Id = Guid.NewGuid(),
                        ParentId = dep1.Id,
                        Number = "3",
                        Name = "jt"
                    };
                    await db.Departments.AddRangeAsync(dep1, dep2, dep3);

                    await db.SaveChangesAsync();
                }


                var target = new DepartmentAppService(statelessServiceContext, options, CreateMapper());

                var result = await target.GetRootListAsync();

                result.Count.Should().Be(2);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestGetListByParentIdAsync()
        {
            var (connection, options) = OpenDb();

            try
            {
                var dep1 = new Department
                {
                    Id = Guid.NewGuid(),
                    Number = "1",
                    Name = "sclq"
                };
                var dep2 = new Department
                {
                    Id = Guid.NewGuid(),
                    ParentId = dep1.Id,
                    Number = "2",
                    Name = "gs"
                };
                var dep3 = new Department
                {
                    Id = Guid.NewGuid(),
                    ParentId = dep1.Id,
                    Number = "3",
                    Name = "jt"
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Departments.AddRangeAsync(dep1, dep2, dep3);

                    await db.SaveChangesAsync();
                }


                var target = new DepartmentAppService(statelessServiceContext, options, CreateMapper());

                var result = await target.GetListByParentIdAsync(dep1.Id);

                result.Count.Should().Be(2);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestSearchByKeywordAsync()
        {
            var (connection, options) = OpenDb();

            try
            {
                var dep1 = new Department
                {
                    Id = Guid.NewGuid(),
                    Number = "1",
                    Name = "sclq"
                };
                var dep2 = new Department
                {
                    Id = Guid.NewGuid(),
                    ParentId = dep1.Id,
                    Number = "2",
                    Name = "sclqgs"
                };
                var dep3 = new Department
                {
                    Id = Guid.NewGuid(),
                    ParentId = dep1.Id,
                    Number = "3",
                    Name = "sclqjt"
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Departments.AddRangeAsync(dep1, dep2, dep3);

                    await db.SaveChangesAsync();
                }


                var target = new DepartmentAppService(statelessServiceContext, options, CreateMapper());

                var result = await target.SearchByKeywordAsync("sclq");
                result.Count.Should().Be(3);

                result = await target.SearchByKeywordAsync("gs");
                result.Count.Should().Be(1);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
