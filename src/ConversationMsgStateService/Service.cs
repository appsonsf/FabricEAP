using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Common;
using InstantMessage;
using InstantMessage.Entities;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using ServiceFabricContrib;

namespace ConversationMsgStateService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    public sealed class Service : StatefulServiceEapBase
    {
        public Service(StatefulServiceContext context,
           DiagnosticPipeline diagnosticPipeline)
           : base(context, diagnosticPipeline)
        {
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener((c)=>
                    new FabricTransportServiceRemotingListener(c,new ConversationMsgAppService(Context,StateManager)))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                ServiceEventSource.Current.ServiceMessage(Context, "inside RunAsync for ConversationMsgStateService");

                return Task.WhenAll(
                    ConversationMsgQueueProcessor.ProcessQueueAsync(
                        StateManager, RemotingClient.CreateConversationCtrlAppService(), Notification.RemotingClient.CreateNotifySessionActor, cancellationToken)
                    //ArchiveGroupQueueProcessor.ProcessAsync(
                    //    StateManager, new DefaultProcessorConfigurator(), cancellationToken)
                        );
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceMessage(Context, "RunAsync Failed, {0}", e);
                throw;
            }
        }

        public const string MessageListDictNamePrefix = "MessageListDict";
        public const string MessageIndexDictNamePrefix = "MessageIndexDict";//NOTE 数据达到100万的时候，插入会接近2.5s，到时候应该提供归档的处理（超过10万就归档）
        public const string MessageProcessQueueName = "MessageProcessQueue";
        public const string PendingArchiveGroupQueueName = "PendingArchiveGroupQueue";
        public const string ArchivedGroupMessageDictName = "ArchivedGroupMessageDict";

        public static async Task<IReliableDictionary2<Guid, ConversationMsg>> GetMessageListDictByIdAsync(IReliableStateManager stateManager, Guid msgId)
        {
            var key = msgId.ToString().Substring(0, 1).ToLower();
            return await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, ConversationMsg>>(MessageListDictNamePrefix + "/" + key);
        }

        public static async Task<List<IReliableDictionary2<Guid, ConversationMsg>>> GetAllMessageListDictsAsync(IReliableStateManager stateManager)
        {
            var lst = new List<IReliableDictionary2<Guid, ConversationMsg>>();
            foreach (var key in StringExtensions.PartitionKeys)
            {
                lst.Add(await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, ConversationMsg>>(MessageListDictNamePrefix + "/" + key));
            }
            return lst;
        }

        public static async Task<IReliableDictionary2<Guid, List<Guid>>> GetMessageIndexDictByIdAsync(IReliableStateManager stateManager, Guid conversationId)
        {
            var key = conversationId.ToString().Substring(0, 1).ToLower();
            return await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, List<Guid>>>(MessageIndexDictNamePrefix + "/" + key);
        }

        public static async Task<List<IReliableDictionary2<Guid, List<Guid>>>> GetAllMessageIndexDictsAsync(IReliableStateManager stateManager)
        {
            var lst = new List<IReliableDictionary2<Guid, List<Guid>>>();
            foreach (var key in StringExtensions.PartitionKeys)
            {
                lst.Add(await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, List<Guid>>>(MessageIndexDictNamePrefix + "/" + key));
            }
            return lst;
        }

        public static async Task<IReliableConcurrentQueue<ConversationMsg>> GetMessageProcessQueue(IReliableStateManager stateManager)
        {
            return await stateManager.GetOrAddAsync<IReliableConcurrentQueue<ConversationMsg>>(MessageProcessQueueName);
        }

        public static async Task<IReliableConcurrentQueue<Guid>> GetPendingArchiveGroupQueue(IReliableStateManager stateManager)
        {
            return await stateManager.GetOrAddAsync<IReliableConcurrentQueue<Guid>>(PendingArchiveGroupQueueName);
        }
    }
}
