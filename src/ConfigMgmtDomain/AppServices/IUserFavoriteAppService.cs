using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConfigMgmt
{
    public interface IUserFavoriteAppService : IService
    {
        Task<bool> AddEmployeeAsync(Guid userId, Guid employeeId);

        Task<bool> RemoveEmployeeAsync(Guid userId, Guid employeeId);

        Task<Guid[]> GetEmployeesAsync(Guid userId);
        Task<bool> IsFavoritedAsync(Guid userId, Guid employeeId);
    }
}
