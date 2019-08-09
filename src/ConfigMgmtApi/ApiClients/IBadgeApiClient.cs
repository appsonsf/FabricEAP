using System.Threading.Tasks;
using ConfigMgmt.Entities;
using Microsoft.AspNetCore.Http;

namespace ConfigMgmt
{
    //TODO 未来重构移动到AppComponentDomain项目中

    public interface IBadgeApiClient
    {
        Task<int?> GetAmountAsync(ComponentConfig config);
    }
}