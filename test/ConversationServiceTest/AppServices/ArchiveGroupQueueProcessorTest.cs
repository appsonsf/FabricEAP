using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConversationMsgStateService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTestCommon;

namespace ConversationServiceTest.AppServices
{
    [TestClass]
    public class ArchiveGroupQueueProcessorTest : TheAppServiceTestBase
    {
        //[TestMethod]
        //public async Task ProcessAsync_Test()
        //{
        //    //Arrage:
        //    await InitDicAsync();
        //    await InitMessageQueueAsync();
        //    await InitArchivedAndPendingMessageQueueAsync();
        //    var source = new CancellationTokenSource();
        //    var mock_config = new Mock<IProcessorConfigurator>();
        //    mock_config.Setup(u => u.IsArchiveGroupProcessTime()).Returns(true);

        //    //Act:
        //    ThreadPool.QueueUserWorkItem(u =>
        //    {
        //        ArchiveGroupQueueProcessor.ProcessAsync(stateManager, mock_config.Object, source.Token).GetAwaiter()
        //            .GetResult();
        //    });
        //    Thread.Sleep(3000);
        //    source.Cancel();
        //    Thread.Sleep(3000);
        //    //Assert:
        //    using (var tx = stateManager.CreateTransaction())
        //    {
        //        var pendingCount = await pendingArchiveQueue.GetCountAsync(tx);
        //        Assert.AreEqual(0, pendingCount);
        //    }
        //}
    }
}
