using EnterpriseContact;
using EnterpriseContact.Entities;
using EnterpriseContact.MsgConstracts;
using FluentAssertions;
using InstantMessage;
using MassTransit;
using MassTransit.Saga;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseContactUnitTest.AppServices
{
    [TestClass]
    public class GroupAppServiceTest : TheAppServiceTestBase
    {
        //TODO 需要覆盖IConversationCtrlAppService的远程调用

        [TestMethod]
        public async Task TestCheckSameWhiteListGroupAsync_TargetNotInGroup()
        {
            var (connection, options) = OpenDb();

            try
            {
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "cxo",
                    Type = GroupType.WhiteListChat,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };
                gp1.Members = new List<GroupMember>
                {
                    new GroupMember
                    {
                        EmployeeId = emp1.Id,
                        GroupId = gp1.Id,
                        Joined = DateTimeOffset.UtcNow,
                    },
                    //new GroupMember
                    //{
                    //    EmployeeId = emp2.Id,
                    //    GroupId = gp1.Id,
                    //    Joined = DateTimeOffset.UtcNow,
                    //}
                };
                //var gp2 = new Group
                //{
                //    Id = Guid.NewGuid(),
                //    Name = "gm",
                //    Type = GroupType.WhiteListChat,
                //    Created = DateTimeOffset.UtcNow,
                //    Updated = DateTimeOffset.UtcNow,
                //};
                //gp2.Members = new List<GroupMember>
                //{
                //    new GroupMember
                //    {
                //        EmployeeId = emp1.Id,
                //        GroupId = gp2.Id,
                //        Joined = DateTimeOffset.UtcNow
                //    }
                //};
                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(emp1, emp2);
                    await db.Groups.AddRangeAsync(gp1);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.CheckSameWhiteListGroupAsync(emp1.UserId.Value, emp2.Id);
                result.Should().BeTrue();
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestCheckSameWhiteListGroupAsync_TargetInSameGroup()
        {
            var (connection, options) = OpenDb();

            try
            {
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "cxo",
                    Type = GroupType.WhiteListChat,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };
                gp1.Members = new List<GroupMember>
                {
                    new GroupMember
                    {
                        EmployeeId = emp1.Id,
                        GroupId = gp1.Id,
                        Joined = DateTimeOffset.UtcNow,
                    },
                    new GroupMember
                    {
                        EmployeeId = emp2.Id,
                        GroupId = gp1.Id,
                        Joined = DateTimeOffset.UtcNow,
                    }
                };
                var gp2 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "gm",
                    Type = GroupType.WhiteListChat,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };
                gp2.Members = new List<GroupMember>
                {
                    new GroupMember
                    {
                        EmployeeId = emp1.Id,
                        GroupId = gp2.Id,
                        Joined = DateTimeOffset.UtcNow
                    }
                };
                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(emp1, emp2);
                    await db.Groups.AddRangeAsync(gp1, gp2);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.CheckSameWhiteListGroupAsync(emp1.Id, emp2.Id);
                result.Should().BeTrue();
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestCheckSameWhiteListGroupAsync_TargetInDiffGroup()
        {
            var (connection, options) = OpenDb();

            try
            {
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "cxo",
                    Type = GroupType.WhiteListChat,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };
                gp1.Members = new List<GroupMember>
                {
                    new GroupMember
                    {
                        EmployeeId = emp1.Id,
                        GroupId = gp1.Id,
                        Joined = DateTimeOffset.UtcNow,
                    }
                };
                var gp2 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "gm",
                    Type = GroupType.WhiteListChat,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };
                gp2.Members = new List<GroupMember>
                {
                    new GroupMember
                    {
                        EmployeeId = emp2.Id,
                        GroupId = gp2.Id,
                        Joined = DateTimeOffset.UtcNow
                    }
                };
                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(emp1, emp2);
                    await db.Groups.AddRangeAsync(gp1, gp2);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.CheckSameWhiteListGroupAsync(emp1.UserId.Value, emp2.Id);
                result.Should().BeFalse();
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestGetListByEmployeeIdAsync()
        {
            var (connection, options) = OpenDb();

            try
            {
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                    Type = GroupType.CustomChat,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };
                gp1.Members = new List<GroupMember>
                {
                    new GroupMember
                    {
                        EmployeeId = emp1.Id,
                        GroupId = gp1.Id,
                        Joined = DateTimeOffset.UtcNow,
                    }
                };
                var gp2 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "bbb",
                    Type = GroupType.CustomChat,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };
                gp2.Members = new List<GroupMember>
                {
                    new GroupMember
                    {
                        EmployeeId = emp1.Id,
                        GroupId = gp2.Id,
                        Joined = DateTimeOffset.UtcNow
                    }
                };
                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(emp1);
                    await db.Groups.AddRangeAsync(gp1, gp2);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.GetListByEmployeeIdAsync(emp1.Id);
                result.Count.Should().Be(2);
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
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                    Type = GroupType.CustomChat,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };
                var gp2 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "bbb",
                    Type = GroupType.CustomChat,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };
                using (var db = new ServiceDbContext(options))
                {
                    await db.Groups.AddRangeAsync(gp1, gp2);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.GetListByIdsAsync(new List<Guid> { gp1.Id, gp2.Id });
                result.Count.Should().Be(2);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestCreateCustomAsync()
        {
            var (connection, options) = OpenDb();

            try
            {
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(emp1, emp2);
                    await db.SaveChangesAsync();
                }

                var bus = Substitute.For<IBusControl>();
                bus.Publish(Arg.Any<GroupAdded>())
                    .Returns(Task.CompletedTask);

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), bus);

                var result = await target.CreateCustomAsync(new GroupInput
                {
                    CurrentUserId = emp1.UserId.Value,
                    CurrentEmployeeId = emp1.Id,
                    Name = "aaa",
                    AddingMemberIds = new HashSet<Guid> { emp1.Id, emp2.Id }
                });
                result.Should().NotBe(Guid.Empty);

                await bus.Received(1).Publish(Arg.Is<GroupAdded>(x => x.Id == result));
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestUpdateAsync_Success()
        {
            var (connection, options) = OpenDb();

            try
            {
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var emp3 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "3",
                    IdCardNo = "3",
                    Name = "ccc",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                    Type = GroupType.CustomChat,
                    CreatedBy = emp1.UserId.Value,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                    Members = new List<GroupMember>
                    {
                        new GroupMember
                        {
                            EmployeeId = emp1.Id,
                            IsOwner = true,
                            Joined = DateTimeOffset.UtcNow,
                        },
                        new GroupMember
                        {
                            EmployeeId = emp2.Id,
                            IsOwner = false,
                            Joined = DateTimeOffset.UtcNow,
                        }
                    }
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(emp1, emp2, emp3);
                    await db.Groups.AddRangeAsync(gp1);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.UpdateAsync(new GroupInput
                {
                    Id = gp1.Id,
                    Name = "bbb",
                    Remark = "ccc",
                    CurrentUserId = emp1.UserId.Value,
                    CurrentEmployeeId = emp1.Id,
                    AddingMemberIds = new HashSet<Guid> { emp3.Id },
                    RemovingMemberIds = new HashSet<Guid> { emp2.Id }
                });
                result.IsSuccess.Should().BeTrue();

                using (var db = new ServiceDbContext(options))
                {
                    var entity = await db.Groups
                        .Include(o => o.Members)
                        .FirstOrDefaultAsync(o => o.Id == gp1.Id);

                    entity.Name.Should().Be("bbb");
                    entity.Remark.Should().Be("ccc");
                    entity.Members.Count.Should().Be(2);
                    entity.Members.Exists(o => o.EmployeeId == emp3.Id).Should().BeTrue();
                    entity.Members.Exists(o => o.EmployeeId == emp2.Id).Should().BeFalse();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestUpdateAsync_Failed_NotFound()
        {
            var (connection, options) = OpenDb();

            try
            {
                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                     CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.UpdateAsync(new GroupInput
                {
                    Id = Guid.NewGuid(),
                    CurrentUserId = Guid.NewGuid(),
                });
                result.IsSuccess.Should().BeFalse();
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestUpdateAsync_Failed_CreatedByNotMatch()
        {
            var (connection, options) = OpenDb();

            try
            {
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                    Type = GroupType.CustomChat,
                    CreatedBy = Guid.NewGuid(),
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Groups.AddRangeAsync(gp1);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.UpdateAsync(new GroupInput
                {
                    Id = gp1.Id,
                    CurrentUserId = Guid.NewGuid(),
                });
                result.IsSuccess.Should().BeFalse();
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestDeleteAsync_Success()
        {
            var (connection, options) = OpenDb();

            try
            {
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                    Type = GroupType.CustomChat,
                    CreatedBy = Guid.NewGuid(),
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Groups.AddRangeAsync(gp1);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.DeleteAsync(new GroupInput
                {
                    Id = gp1.Id,
                    CurrentUserId = gp1.CreatedBy,
                });
                result.IsSuccess.Should().BeTrue();

                using (var db = new ServiceDbContext(options))
                {
                    var entity = await db.Groups.FindAsync(gp1.Id);
                    entity.Should().BeNull();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestDeleteAsync_Failed_NotFound()
        {
            var (connection, options) = OpenDb();

            try
            {
                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.DeleteAsync(new GroupInput
                {
                    Id = Guid.NewGuid(),
                    CurrentUserId = Guid.NewGuid(),
                });
                result.IsSuccess.Should().BeFalse();
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestDeleteAsync_Failed_CreatedByNotMatch()
        {
            var (connection, options) = OpenDb();

            try
            {
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                    Type = GroupType.CustomChat,
                    CreatedBy = Guid.NewGuid(),
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Groups.AddRangeAsync(gp1);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.DeleteAsync(new GroupInput
                {
                    Id = gp1.Id,
                    CurrentUserId = Guid.NewGuid(),
                });
                result.IsSuccess.Should().BeFalse();
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestGetByIdAsync_Success()
        {
            var (connection, options) = OpenDb();

            try
            {
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                    Type = GroupType.CustomChat,
                    CreatedBy = emp1.UserId.Value,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                    Members = new List<GroupMember>
                    {
                        new GroupMember
                        {
                            EmployeeId = emp1.Id,
                            IsOwner = true,
                            Joined = DateTimeOffset.UtcNow,
                        },
                        new GroupMember
                        {
                            EmployeeId = emp2.Id,
                            IsOwner = false,
                            Joined = DateTimeOffset.UtcNow,
                        }
                    }
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(emp1, emp2);
                    await db.Groups.AddRangeAsync(gp1);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.GetByIdAsync(gp1.Id);
                result.Should().NotBeNull();
                result.Name.Should().Be("aaa");
                result.Members.Count.Should().Be(2);
                var m1 = result.Members.Find(o => o.EmployeeId == emp1.Id);
                m1.Should().NotBeNull();
                m1.EmployeeName.Should().Be("aaa");
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestQuitAsync_Success()
        {
            var (connection, options) = OpenDb();

            try
            {
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                    Type = GroupType.CustomChat,
                    CreatedBy = emp1.UserId.Value,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                    Members = new List<GroupMember>
                    {
                        new GroupMember
                        {
                            EmployeeId = emp1.Id,
                            IsOwner = true,
                            Joined = DateTimeOffset.UtcNow,
                        },
                        new GroupMember
                        {
                            EmployeeId = emp2.Id,
                            IsOwner = false,
                            Joined = DateTimeOffset.UtcNow,
                        }
                    }
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(emp1, emp2);
                    await db.Groups.AddRangeAsync(gp1);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                     CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.QuitAsync(new GroupInput
                {
                    Id = gp1.Id,
                    CurrentEmployeeId = emp2.Id,
                });
                result.IsSuccess.Should().BeTrue();
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task TestQuitAsync_Failed_OwnerCannotQuit()
        {
            var (connection, options) = OpenDb();

            try
            {
                var emp1 = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Number = "1",
                    IdCardNo = "1",
                    Name = "aaa",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var emp2 = new Employee
                {
                    Id = Guid.NewGuid(),
                    Number = "2",
                    IdCardNo = "2",
                    Name = "bbb",
                    PrimaryDepartmentId = Guid.Empty,
                    PrimaryPositionId = Guid.Empty,
                };
                var gp1 = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "aaa",
                    Type = GroupType.CustomChat,
                    CreatedBy = emp1.UserId.Value,
                    Created = DateTimeOffset.UtcNow,
                    Updated = DateTimeOffset.UtcNow,
                    Members = new List<GroupMember>
                    {
                        new GroupMember
                        {
                            EmployeeId = emp1.Id,
                            IsOwner = true,
                            Joined = DateTimeOffset.UtcNow,
                        },
                        new GroupMember
                        {
                            EmployeeId = emp2.Id,
                            IsOwner = false,
                            Joined = DateTimeOffset.UtcNow,
                        }
                    }
                };

                using (var db = new ServiceDbContext(options))
                {
                    await db.Employees.AddRangeAsync(emp1, emp2);
                    await db.Groups.AddRangeAsync(gp1);
                    await db.SaveChangesAsync();
                }

                var target = new GroupAppService(statelessServiceContext, options, CreateMapper(),
                    CreateMockSimpleKeyValueService(), Substitute.For<IBusControl>());

                var result = await target.QuitAsync(new GroupInput
                {
                    Id = gp1.Id,
                    CurrentEmployeeId = emp1.Id,
                });
                result.IsSuccess.Should().BeFalse();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
