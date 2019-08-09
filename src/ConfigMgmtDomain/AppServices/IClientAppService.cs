using ConfigMgmt.Entities;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConfigMgmt
{
    /// <summary>
    /// 移动平台客户端AppService
    /// </summary>
    public interface IClientAppService : IService
    {
        Task<bool> AddOrUpdateClientAsync(Client input);
    }
}
