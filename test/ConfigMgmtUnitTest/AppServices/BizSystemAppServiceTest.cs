using ConfigMgmt;
using ConfigMgmt.Entities;
using FluentAssertions;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemConfigStateService;

namespace ConfigMgmtUnitTest.AppServices
{
    [TestClass]
    public class BizSystemAppServiceTest : TheAppServiceTestBase
    {
        [TestMethod]
        public async Task TestAddOrUpdateForTodoCenterAsync()
        {
            var dictBizSystem = await stateManager.GetOrAddAsync<IReliableDictionary2<string, TodoCenterBizSystem>>(Service.DictionaryName_TodoCenterBizSystem);

            var target = new BizSystemAppService(statefulServiceContext, stateManager);
            var result = await target.AddOrUpdateForTodoCenterAsync(new TodoCenterBizSystem
            {
                Id = "test",
                Name = "测试",
                PendingListUrl = "about:",
                PendingListAmountUrl = "about:",
                DoneListUrl = "about:",
                OpenDetailUris = new Dictionary<ClientPlatform, string>
                {
                    [ClientPlatform.Android] = "about:"
                }
            });
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Test_GetAllForTodoCenterAsync_RemoveForTodoCenterAsync()
        {
            var dictBizSystem = await stateManager.GetOrAddAsync<IReliableDictionary2<string, TodoCenterBizSystem>>(Service.DictionaryName_TodoCenterBizSystem);

            using (var tx = stateManager.CreateTransaction())
            {
                await dictBizSystem.AddAsync(tx, "aaa", new TodoCenterBizSystem
                {
                    Id = "aaa",
                    Name = "aaa"
                });
                await dictBizSystem.AddAsync(tx, "bbb", new TodoCenterBizSystem
                {
                    Id = "bbb",
                    Name = "bbb"
                });
                await tx.CommitAsync();
            }

            var target = new BizSystemAppService(statefulServiceContext, stateManager);
            var result = await target.GetAllForTodoCenterAsync();
            result.Should().NotBeNull();
            result.Count.Should().Be(2);

            var removeResult = await target.RemoveForTodoCenterAsync("bbb");
            removeResult.Should().BeTrue();

            result = await target.GetAllForTodoCenterAsync();
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
        }

        [TestMethod]
        public async Task TestAddOrUpdateForMsgNotifyAsync()
        {
            var dict = await stateManager.GetOrAddAsync<IReliableDictionary2<string, MsgNotifyBizSystem>>(Service.DictionaryName_MsgNotifyBizSystem);

            var target = new BizSystemAppService(statefulServiceContext, stateManager);
            var result = await target.AddOrUpdateForMsgNotifyAsync(new MsgNotifyBizSystem
            {
                Id = "test",
                Name = "测试",
            });
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Test_GetAllForMsgNotifyAsync_RemoveForMsgNotifyAsync()
        {
            var dict = await stateManager.GetOrAddAsync<IReliableDictionary2<string, MsgNotifyBizSystem>>(Service.DictionaryName_MsgNotifyBizSystem);

            using (var tx = stateManager.CreateTransaction())
            {
                await dict.AddAsync(tx, "aaa", new MsgNotifyBizSystem
                {
                    Id = "aaa",
                    Name = "aaa"
                });
                await dict.AddAsync(tx, "bbb", new MsgNotifyBizSystem
                {
                    Id = "bbb",
                    Name = "bbb"
                });
                await tx.CommitAsync();
            }

            var target = new BizSystemAppService(statefulServiceContext, stateManager);
            var result = await target.GetAllForMsgNotifyAsync();
            result.Should().NotBeNull();
            result.Count.Should().Be(2);

            var removeResult = await target.RemoveForMsgNotifyAsync("bbb");
            removeResult.Should().BeTrue();

            result = await target.GetAllForMsgNotifyAsync();
            result.Should().NotBeNull();
            result.Count.Should().Be(1);

            var singleResult = await target.GetByIdForNotifyMsgAsync("aaa");
            singleResult.Should().NotBeNull();
            singleResult.Name.Should().Be("aaa");
        }

    }
}
