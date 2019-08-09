using ConfigMgmt.Entities;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConfigMgmt
{
    public interface IBizSystemAppService : IService
    {
        Task<bool> AddOrUpdateForTodoCenterAsync(TodoCenterBizSystem input);
        Task<bool> RemoveForTodoCenterAsync(string id);
        Task<List<TodoCenterBizSystem>> GetAllForTodoCenterAsync();

        Task<bool> AddOrUpdateForMsgNotifyAsync(MsgNotifyBizSystem input);
        Task<bool> RemoveForMsgNotifyAsync(string id);
        Task<List<MsgNotifyBizSystem>> GetAllForMsgNotifyAsync();
        Task<MsgNotifyBizSystem> GetByIdForNotifyMsgAsync(string id);
    }
}
