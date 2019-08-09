using ConfigMgmt;
using ConfigMgmt.Entities;
using FluentAssertions;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemConfigStateService;

namespace ConfigMgmtUnitTest.AppServices
{
    [TestClass]
    public class WorkbenchAppServiceTest : TheAppServiceTestBase
    {
        [TestMethod]
        public async Task AddOrUpdateEntrancesAsync()
        {
            var dictClients = await stateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            var client = Client.Default();

            using (var tx = stateManager.CreateTransaction())
            {
                await dictClients.AddAsync(tx, client.Id, client);
                await tx.CommitAsync();
            }

            var target = new WorkbenchAppService(statefulServiceContext, stateManager, CreateMapper());
            var result = await target.AddOrUpdateEntrancesAsync(new ConfigMgmt.AddEntrancesInput
            {
                ClientId = client.Id,
                AppEntrances = new List<AppEntrance>
                {
                    new AppEntrance
                    {
                        Id=Guid.NewGuid(),
                        ClientId=client.Id,
                    },
                    new AppEntrance
                    {
                        Id=Guid.NewGuid(),
                        ClientId=client.Id,
                    }
                }
            });

            result.Should().BeTrue();
            Assert.AreEqual(2, client.AppEntranceIds.Count);

        }

        [TestMethod]
        public async Task GetAppEntrancesAsync()
        {
            var dictClients = await stateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            var dictAppEntrances = await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, AppEntrance>>(Service.DictionaryName_AppEntrance);
            var client = Client.Default();
            var entrances = new List<AppEntrance>();
            for (int i = 0; i < 3; i++)
            {
                entrances.Add(new AppEntrance
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    Uris = new Dictionary<ClientPlatform, string>
                    {
                        [ClientPlatform.Android] = "http://android" + i
                    }
                });
            }
            for (int i = 0; i < 6; i++)
            {
                entrances.Add(new AppEntrance
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    Uris = new Dictionary<ClientPlatform, string>
                    {
                        [ClientPlatform.Desktop] = "http://desktop" + i
                    }
                });
            }
            for (int i = 0; i < 9; i++)
            {
                entrances.Add(new AppEntrance
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    Uris = new Dictionary<ClientPlatform, string>
                    {
                        [ClientPlatform.iOS] = "http://ios" + i
                    }
                });
            }
            for (int i = 0; i < 10; i++)
            {
                entrances.Add(new AppEntrance
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    Uris = new Dictionary<ClientPlatform, string>
                    {
                        [ClientPlatform.Android] = "http://android_desktop_ios_" + i,
                        [ClientPlatform.Desktop] = "http://android_desktop_ios_" + i,
                        [ClientPlatform.iOS] = "http://android_desktop_ios_" + i
                    }
                });
            }

            using (var tx = stateManager.CreateTransaction())
            {
                foreach (var item in entrances)
                {
                    await dictAppEntrances.AddAsync(tx, item.Id, item);
                    client.AppEntranceIds.Add(item.Id);
                }
                client.WorkbenchConfigTimestamp = DateTimeOffset.UtcNow;
                await dictClients.AddAsync(tx, client.Id, client);
                await tx.CommitAsync();
            }

            var appService = new WorkbenchAppService(statefulServiceContext, stateManager, CreateMapper());
            var result = await appService.GetAppEntrancesAsync(new GetAppEntrancesInput
            {
                ClientId = client.Id,
                ClientPlatform = ClientPlatform.Android
            });
            Assert.AreEqual(13, result.Count);
            result = await appService.GetAppEntrancesAsync(new GetAppEntrancesInput
            {
                ClientId = client.Id,
                ClientPlatform = ClientPlatform.Desktop
            });
            Assert.AreEqual(16, result.Count);
            result = await appService.GetAppEntrancesAsync(new GetAppEntrancesInput
            {
                ClientId = client.Id,
                ClientPlatform = ClientPlatform.iOS
            });
            Assert.AreEqual(19, result.Count);
        }

        [TestMethod]
        public async Task GetAppEntrancesAsync_WhenRoleCheck()
        {
            var dictClients = await stateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            var dictAppEntrances = await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, AppEntrance>>(Service.DictionaryName_AppEntrance);
            var client = Client.Default();
            var entrances = new List<AppEntrance>();
            for (int i = 0; i < 3; i++)
            {
                entrances.Add(new AppEntrance
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    Uris = new Dictionary<ClientPlatform, string>
                    {
                        [ClientPlatform.Android] = "http://android_desktop_ios_" + i,
                        [ClientPlatform.Desktop] = "http://android_desktop_ios_" + i,
                        [ClientPlatform.iOS] = "http://android_desktop_ios_" + i
                    }
                });
            }
            for (int i = 0; i < 6; i++)
            {
                entrances.Add(new AppEntrance
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    Uris = new Dictionary<ClientPlatform, string>
                    {
                        [ClientPlatform.Android] = "http://android_desktop_ios_" + i,
                        [ClientPlatform.Desktop] = "http://android_desktop_ios_" + i,
                        [ClientPlatform.iOS] = "http://android_desktop_ios_" + i
                    },
                    NeedRoles = new[] { "ABC" }
                });
            }
            for (int i = 0; i < 9; i++)
            {
                entrances.Add(new AppEntrance
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    Uris = new Dictionary<ClientPlatform, string>
                    {
                        [ClientPlatform.Android] = "http://android_desktop_ios_" + i,
                        [ClientPlatform.Desktop] = "http://android_desktop_ios_" + i,
                        [ClientPlatform.iOS] = "http://android_desktop_ios_" + i
                    },
                    NeedRoles = new[] { "ABC", "EFG" }
                });
            }

            using (var tx = stateManager.CreateTransaction())
            {
                foreach (var item in entrances)
                {
                    await dictAppEntrances.AddAsync(tx, item.Id, item);
                    client.AppEntranceIds.Add(item.Id);
                }
                client.WorkbenchConfigTimestamp = DateTimeOffset.UtcNow;
                await dictClients.AddAsync(tx, client.Id, client);
                await tx.CommitAsync();
            }

            var appService = new WorkbenchAppService(statefulServiceContext, stateManager, CreateMapper());
            //everyone
            var result = await appService.GetAppEntrancesAsync(new GetAppEntrancesInput
            {
                ClientId = client.Id,
                ClientPlatform = ClientPlatform.Android
            });
            Assert.AreEqual(3, result.Count);
            //everyone without right needroles
            result = await appService.GetAppEntrancesAsync(new GetAppEntrancesInput
            {
                ClientId = client.Id,
                ClientPlatform = ClientPlatform.Desktop,
                UserRoles = new List<string> { "EFG" }
            });
            Assert.AreEqual(3, result.Count);
            //someone with ABC
            result = await appService.GetAppEntrancesAsync(new GetAppEntrancesInput
            {
                ClientId = client.Id,
                ClientPlatform = ClientPlatform.Desktop,
                UserRoles = new List<string> { "ABC" }
            });
            Assert.AreEqual(9, result.Count);
            //someone with ABC and EFG
            result = await appService.GetAppEntrancesAsync(new GetAppEntrancesInput
            {
                ClientId = client.Id,
                ClientPlatform = ClientPlatform.iOS,
                UserRoles = new List<string> { "ABC", "EFG" }
            });
            Assert.AreEqual(18, result.Count);
        }

        [TestMethod]
        public async Task GetComponentConfigAsync_ReturnNull()
        {
            var dictClients = await stateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            var dictAppEntrances = await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, AppEntrance>>(Service.DictionaryName_AppEntrance);
            var client = Client.Default();
            var entrances = new List<AppEntrance>();
            for (int i = 0; i < 3; i++)
            {
                entrances.Add(new AppEntrance
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    Uris = new Dictionary<ClientPlatform, string>
                    {
                        [ClientPlatform.Android] = "http://android" + i
                    }
                });
            }
            using (var tx = stateManager.CreateTransaction())
            {
                foreach (var item in entrances)
                {
                    await dictAppEntrances.AddAsync(tx, item.Id, item);
                    client.AppEntranceIds.Add(item.Id);
                }
                client.WorkbenchConfigTimestamp = DateTimeOffset.UtcNow;
                await dictClients.AddAsync(tx, client.Id, client);
                await tx.CommitAsync();
            }

            var appService = new WorkbenchAppService(statefulServiceContext, stateManager, CreateMapper());
            var result = await appService.GetComponentConfigAsync(Guid.NewGuid());
            Assert.IsNull(result);
            result = await appService.GetComponentConfigAsync(entrances[0].Id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetComponentConfigAsync()
        {
            var dictClients = await stateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            var dictAppEntrances = await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, AppEntrance>>(Service.DictionaryName_AppEntrance);
            var client = Client.Default();
            var entrances = new List<AppEntrance>();
            for (int i = 0; i < 3; i++)
            {
                entrances.Add(new AppEntrance
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    Uris = new Dictionary<ClientPlatform, string>
                    {
                        [ClientPlatform.Android] = "http://android" + i
                    },
                    ComponentConfig = new ComponentConfig
                    {
                        AuthType = ComponentDataSourceAuthType.None
                    }
                });
            }
            using (var tx = stateManager.CreateTransaction())
            {
                foreach (var item in entrances)
                {
                    await dictAppEntrances.AddAsync(tx, item.Id, item);
                    client.AppEntranceIds.Add(item.Id);
                }
                client.WorkbenchConfigTimestamp = DateTimeOffset.UtcNow;
                await dictClients.AddAsync(tx, client.Id, client);
                await tx.CommitAsync();
            }

            var appService = new WorkbenchAppService(statefulServiceContext, stateManager, CreateMapper());
            var result = await appService.GetComponentConfigAsync(entrances[0].Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(ComponentDataSourceAuthType.None, result.AuthType);
        }
    }
}
