using EnterpriseContact;
using EnterpriseContact.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseContactUnitTest.AppServices
{
    [TestClass]
    public class EmployeeAppServiceTest : TheAppServiceTestBase
    {
        [TestMethod]
        public async Task TestGetRootListAsync()
        {
            var (connection, options) = OpenDb();

            try
            {
                var dep1 = new Department
                {
                    Id = Guid.NewGuid(),
                    Number = "1",
                    Name = "sclq",
                };
                var pos1 = new Position
                {
                    DepartmentId = dep1.Id,
                    Id = Guid.NewGuid(),
                    Name = "ceo"
                };
                var dep2 = new Department
                {
                    Id = Guid.NewGuid(),
                    ParentId = dep1.Id,
                    Number = "2",
                    Name = "sclqjt"
                };
                var pos2 = new Position
                {
                    DepartmentId = dep2.Id,
                    Id = Guid.NewGuid(),
                    Name = "gm"
                };
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = dep1.Id,
                    PrimaryPositionId = pos1.Id,
                };
                emp1.Positions = new List<EmployeePosition>
                {
                    new EmployeePosition
                    {
                        EmployeeId = emp1.Id,
                        PositionId = pos1.Id,
                        IsPrimary = true
                    },
                    new EmployeePosition
                    {
                        EmployeeId = emp1.Id,
                        PositionId = pos2.Id,
                        IsPrimary = false
                    }
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = dep2.Id,
                    PrimaryPositionId = pos2.Id,
                };
                emp2.Positions = new List<EmployeePosition>
                {
                    new EmployeePosition
                    {
                        EmployeeId = emp2.Id,
                        PositionId = pos1.Id,
                        IsPrimary = false
                    },
                    new EmployeePosition
                    {
                        EmployeeId = emp2.Id,
                        PositionId = pos2.Id,
                        IsPrimary = true
                    }
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Departments.AddRangeAsync(dep1, dep2);
                    await db.Positions.AddRangeAsync(pos1, pos2);
                    await db.Employees.AddRangeAsync(emp1, emp2);
                    await db.SaveChangesAsync();
                }

                var target = new EmployeeAppService(statelessServiceContext, options, CreateMapper());

                var result = await target.GetRootListAsync();

                result.Count.Should().Be(2);
                result[0].Id.Should().Be(emp1.Id);
                result[0].PrimaryDepartmentId.Should().Be(dep1.Id);
                result[0].PrimaryPositionId.Should().Be(pos1.Id);
                result[1].Id.Should().Be(emp2.Id);
                result[1].PrimaryDepartmentId.Should().Be(dep2.Id);
                result[1].PrimaryPositionId.Should().Be(pos2.Id);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestGetListByDepartmentIdAsync()
        {
            var (connection, options) = OpenDb();

            try
            {
                var dep1 = new Department
                {
                    Id = Guid.NewGuid(),
                    Number = "1",
                    Name = "sclq",
                };
                var pos1 = new Position
                {
                    DepartmentId = dep1.Id,
                    Id = Guid.NewGuid(),
                    Name = "ceo"
                };
                var dep2 = new Department
                {
                    Id = Guid.NewGuid(),
                    ParentId = dep1.Id,
                    Number = "2",
                    Name = "sclqjt"
                };
                var pos2 = new Position
                {
                    DepartmentId = dep2.Id,
                    Id = Guid.NewGuid(),
                    Name = "gm"
                };
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = dep2.Id,
                    PrimaryPositionId = pos2.Id,
                };
                emp1.Positions = new List<EmployeePosition>
                {
                    new EmployeePosition
                    {
                        EmployeeId = emp1.Id,
                        PositionId = pos2.Id,
                        IsPrimary = true
                    },
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = dep2.Id,
                    PrimaryPositionId = pos2.Id,
                };
                emp2.Positions = new List<EmployeePosition>
                {
                    new EmployeePosition
                    {
                        EmployeeId = emp2.Id,
                        PositionId = pos2.Id,
                        IsPrimary = true
                    }
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Departments.AddRangeAsync(dep1, dep2);
                    await db.Positions.AddRangeAsync(pos1, pos2);
                    await db.Employees.AddRangeAsync(emp1, emp2);
                    await db.SaveChangesAsync();
                }

                var target = new EmployeeAppService(statelessServiceContext, options, CreateMapper());

                var result = await target.GetListByDepartmentIdAsync(dep1.Id);
                result.Count.Should().Be(0);

                result = await target.GetListByDepartmentIdAsync(dep2.Id);
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
                var employess = new List<Employee>()
                {
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        Number = "1",
                        IdCardNo = "1",
                        Name = "aaa",
                        PrimaryDepartmentId = Guid.NewGuid(),
                        PrimaryPositionId = Guid.NewGuid(),
                    },
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        Number = "2",
                        IdCardNo = "2",
                        Name = "aabb",
                        PrimaryDepartmentId = Guid.NewGuid(),
                        PrimaryPositionId = Guid.NewGuid(),
                    },
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        Number = "3",
                        IdCardNo = "3",
                        Name = "bbcc",
                        PrimaryDepartmentId = Guid.NewGuid(),
                        PrimaryPositionId = Guid.NewGuid(),
                    },
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(employess);

                    await db.SaveChangesAsync();
                }


                var target = new EmployeeAppService(statelessServiceContext, options, CreateMapper());

                var result = await target.SearchByKeywordAsync("aa");
                result.Count.Should().Be(2);

                result = await target.SearchByKeywordAsync("bb");
                result.Count.Should().Be(2);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task Test_GetByIdAsync_GetByUserIdAsync()
        {
            var (connection, options) = OpenDb();

            try
            {
                var dep0 = new Department
                {
                    Id = Guid.NewGuid(),
                    Number = "0",
                    Name = "sclq",
                };
                var dep1 = new Department
                {
                    Id = Guid.NewGuid(),
                    ParentId = dep0.Id,
                    Number = "1",
                    Name = "sclq-jt",
                };
                var pos1 = new Position
                {
                    DepartmentId = dep1.Id,
                    Id = Guid.NewGuid(),
                    Name = "gm"
                };
                var dep2 = new Department
                {
                    Id = Guid.NewGuid(),
                    ParentId = dep0.Id,
                    Number = "2",
                    Name = "sclq-gs"
                };
                var pos2 = new Position
                {
                    DepartmentId = dep2.Id,
                    Id = Guid.NewGuid(),
                    Name = "ceo"
                };
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = dep1.Id,
                    PrimaryPositionId = pos1.Id,
                };
                emp1.Positions = new List<EmployeePosition>
                {
                    new EmployeePosition
                    {
                        EmployeeId = emp1.Id,
                        PositionId = pos1.Id,
                        IsPrimary = true
                    },
                    new EmployeePosition
                    {
                        EmployeeId = emp1.Id,
                        PositionId = pos2.Id,
                        IsPrimary = false,
                    },
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Departments.AddRangeAsync(dep0, dep1, dep2);
                    await db.Positions.AddRangeAsync(pos1, pos2);
                    await db.Employees.AddRangeAsync(emp1);
                    await db.SaveChangesAsync();
                }

                var target = new EmployeeAppService(statelessServiceContext, options, CreateMapper());

                var result = await target.GetByIdAsync(emp1.Id);
                result.Name.Should().Be("aaa");
                result.ParttimePositionIds.Count.Should().Be(1);
                result.ParttimePositionIds[0].Should().Be(pos2.Id);

                result = await target.GetByUserIdAsync(emp1.UserId.Value);
                result.Name.Should().Be("aaa");
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestGetListByIdsAsync()
        {
            var (connection, options) = OpenDb();

            try
            {
                var employess = new List<Employee>()
                {
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        Number = "1",
                        IdCardNo = "1",
                        Name = "aaa",
                        PrimaryDepartmentId = Guid.NewGuid(),
                        PrimaryPositionId = Guid.NewGuid(),
                    },
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        Number = "2",
                        IdCardNo = "2",
                        Name = "aabb",
                        PrimaryDepartmentId = Guid.NewGuid(),
                        PrimaryPositionId = Guid.NewGuid(),
                    },
                    new Employee
                    {
                        Id = Guid.NewGuid(),
                        Number = "3",
                        IdCardNo = "3",
                        Name = "bbcc",
                        PrimaryDepartmentId = Guid.NewGuid(),
                        PrimaryPositionId = Guid.NewGuid(),
                    },
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(employess);

                    await db.SaveChangesAsync();
                }


                var target = new EmployeeAppService(statelessServiceContext, options, CreateMapper());

                var result = await target.GetListByIdsAsync(new Guid[] { employess[0].Id, employess[1].Id, employess[2].Id });
                result.Count.Should().Be(3);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task GetUserIdsByDepartmentIdAsync()
        {
            var (connection, options) = OpenDb();

            try
            {
                var dep1 = new Department
                {
                    Id = Guid.NewGuid(),
                    Number = "1",
                    Name = "sclq",
                };
                var pos1 = new Position
                {
                    DepartmentId = dep1.Id,
                    Id = Guid.NewGuid(),
                    Name = "ceo"
                };
                var pos2 = new Position
                {
                    DepartmentId = dep1.Id,
                    Id = Guid.NewGuid(),
                    Name = "gm"
                };
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = dep1.Id,
                    PrimaryPositionId = pos1.Id,
                };
                emp1.Positions = new List<EmployeePosition>
                {
                    new EmployeePosition
                    {
                        EmployeeId = emp1.Id,
                        PositionId = pos1.Id,
                        IsPrimary = true
                    },
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = dep1.Id,
                    PrimaryPositionId = pos2.Id,
                };
                emp2.Positions = new List<EmployeePosition>
                {
                    new EmployeePosition
                    {
                        EmployeeId = emp2.Id,
                        PositionId = pos2.Id,
                        IsPrimary = true
                    }
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Departments.AddRangeAsync(dep1);
                    await db.Positions.AddRangeAsync(pos1, pos2);
                    await db.Employees.AddRangeAsync(emp1, emp2);
                    await db.SaveChangesAsync();
                }

                var target = new EmployeeAppService(statelessServiceContext, options, CreateMapper());

                var result = await target.GetUserIdsByDepartmentIdAsync(dep1.Id);
                result.Count.Should().Be(0);

                emp1.UserId = Guid.NewGuid();
                emp2.UserId = Guid.NewGuid();
                using (var db = new ServiceDbContext(options))
                {
                    db.Employees.UpdateRange(emp1, emp2);
                    await db.SaveChangesAsync();
                }

                result = await target.GetUserIdsByDepartmentIdAsync(dep1.Id);
                result.Count.Should().Be(2);

            }
            finally
            {
                connection.Close();
            }
        }
    }
}
