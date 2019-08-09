using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceFabric.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConfigStateService;

namespace ConfigMgmtUnitTest.AppServices
{
    [TestClass]
    public class UserFavoriteAppServiceTest:TheAppServiceTestBase
    {
        [TestMethod]
        public async Task TestAddEmployeeAsync_Correct()
        {
            var target = new UserFavoriteAppService(statefulServiceContext, stateManager);
            var userId = Guid.NewGuid();
            var empId = Guid.NewGuid();
            var result = await target.AddEmployeeAsync(userId, empId);
            var empIdFonud = (await target.GetEmployeesAsync(userId)).FirstOrDefault();
            Assert.AreEqual(empIdFonud, empId);
        }

        [TestMethod]
        public async Task TestAddEmployeeAsync_SameUsers()
        {
            var target = new UserFavoriteAppService(statefulServiceContext, stateManager);
            var userId = Guid.NewGuid();
            var empId = Guid.NewGuid();
            var result1 = await target.AddEmployeeAsync(userId, empId);
            var result2 = await target.AddEmployeeAsync(userId, empId);
            var addResult = await target.GetEmployeesAsync(userId);

            Assert.AreEqual(true, result1);
            Assert.AreEqual(true, result2);
            Assert.AreEqual(true, addResult != null);
            Assert.AreEqual(1, addResult.Count());
        }

        [TestMethod]
        public async Task TestAddEmployeeAsync_NotSameUsers()
        {
            var target = new UserFavoriteAppService(statefulServiceContext, stateManager);
            var userId = Guid.NewGuid();
            var empIds = new Guid[] { Guid.NewGuid(),Guid.NewGuid() };
            var results = new List<bool>();
            foreach (var empId in empIds)
            {
                results.Add(await target.AddEmployeeAsync(userId, empId));
            }
            var addResult = await target.GetEmployeesAsync(userId);
            Assert.AreEqual(true, !results.Contains(false));
            Assert.AreEqual(true, addResult != null);
            Assert.AreEqual(empIds.Length, addResult.Count());
        }

        [TestMethod]
        public async Task TestRemoveEmployeeAsync_Correct()
        {
            var target = new UserFavoriteAppService(statefulServiceContext, stateManager);
            var userId = Guid.NewGuid();
            var empId = Guid.NewGuid();
            await target.AddEmployeeAsync(userId, empId);
            var beforRemoveResult = await target.GetEmployeesAsync(userId);
            await target.RemoveEmployeeAsync(userId, empId);
            var afterRemoveResult = await target.GetEmployeesAsync(userId);

            Assert.AreEqual(true, beforRemoveResult.Any());
            Assert.AreEqual(true, !afterRemoveResult.Any());
        }

        [TestMethod]
        public async Task TestRemoveEmployeeAsync_NotHasInFavorites()
        {
            var target = new UserFavoriteAppService(statefulServiceContext, stateManager);
            var userId = Guid.NewGuid();
            var result = await target.RemoveEmployeeAsync(userId, Guid.NewGuid());
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task TestRemoveEmployeeAsync_Batch()
        {
            var target = new UserFavoriteAppService(statefulServiceContext, stateManager);
            var userId = Guid.NewGuid();
            var empIds = new Guid[] { Guid.NewGuid(), Guid.NewGuid() };
            var results = new List<bool>();
            foreach (var empId in empIds)
            {
                results.Add(await target.AddEmployeeAsync(userId, empId));
            }

            var beforRemoveResult = await target.GetEmployeesAsync(userId);
            foreach (var empId in empIds)
            {
                await target.RemoveEmployeeAsync(userId, empId);
            }

            var afterRemoveResult = await target.GetEmployeesAsync(userId);

            Assert.AreEqual(true, beforRemoveResult.Any());
            Assert.AreEqual(true, !afterRemoveResult.Any());
        }
    }
}
