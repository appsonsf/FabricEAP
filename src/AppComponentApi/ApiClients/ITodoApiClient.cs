using AppComponent.ViewModels;
using ConfigMgmt.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppComponent
{
    //TODO 未来重构移动到AppComponentDomain项目中

    public interface ITodoApiClient
    {
        Task<List<TodoListVM>> GetPendingListAsync(ComponentConfig config);
        Task<List<TodoListVM>> GetDoneListAsync(ComponentConfig config);
    }
}
