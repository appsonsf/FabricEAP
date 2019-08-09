using ConfigMgmt.Entities;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using SystemConfigStateService;

namespace ConfigMgmtUnitTest.AppServices
{
    [TestClass]
    public class ClientAppServiceTest : TheAppServiceTestBase
    {
        [TestMethod]
        public async Task TestAddOrUpdateClientAsync()
        {
            var dictClients = await stateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            var client = Client.Default();

            var appService = new ClientAppService(statefulServiceContext, stateManager);
            var result = await appService.AddOrUpdateClientAsync(client);
            Assert.IsTrue(result);
        }
    }
}
