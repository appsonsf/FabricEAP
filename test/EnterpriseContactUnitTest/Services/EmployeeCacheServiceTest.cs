using Common;
using EnterpriseContact;
using EnterpriseContact.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContactUnitTest.Services
{
    [TestClass]
    public class EmployeeCacheServiceTest
    {
        [TestMethod]
        public async Task ByUserIdAsync()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var cache = Substitute.For<IMemoryCache>();

            var employeeAppService = Substitute.For<IEmployeeAppService>();
            employeeAppService.GetCacheDataAsync()
                .Returns(new List<EmployeeCacheOutput>
                {
                    new EmployeeCacheOutput
                    {
                        Id=Guid.NewGuid(),
                        UserId=id1,
                    },
                    new EmployeeCacheOutput
                    {
                        Id=Guid.NewGuid(),
                        UserId=id2
                    }
                });
            var target = new EmployeeCacheService(cache, employeeAppService);
            var result = await target.ByUserIdAsync(id1, id2);

            result.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public async Task ByNumberAsync()
        {
            var id1 = Guid.NewGuid().ToString("N");
            var id2 = Guid.NewGuid().ToString("N");

            var cache = Substitute.For<IMemoryCache>();
            var employeeAppService = Substitute.For<IEmployeeAppService>();
            employeeAppService.GetCacheDataAsync()
                .Returns(new List<EmployeeCacheOutput>
                {
                    new EmployeeCacheOutput
                    {
                        Id=Guid.NewGuid(),
                        Number=id1,
                    },
                    new EmployeeCacheOutput
                    {
                        Id=Guid.NewGuid(),
                        Number=id2
                    }
                });

            var target = new EmployeeCacheService(cache, employeeAppService);
            var result = await target.ByNumberAsync(id1, id2);

            result.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public async Task ByIdAsync()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var cache = Substitute.For<IMemoryCache>();
            var employeeAppService = Substitute.For<IEmployeeAppService>();
            employeeAppService.GetCacheDataAsync()
                .Returns(new List<EmployeeCacheOutput>
                {
                    new EmployeeCacheOutput
                    {
                        Id=id1,
                    },
                    new EmployeeCacheOutput
                    {
                        Id=id2,
                    }
                });

            var target = new EmployeeCacheService(cache, employeeAppService);
            var result = await target.ByIdAsync(id1, id2);

            result.Should().NotBeNullOrEmpty();
        }
    }
}
