using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    public interface IPositionAppService : IService
    {
        Task<List<PositionListOutput>> GetAllListAsync();
    }
}
