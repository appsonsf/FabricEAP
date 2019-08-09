using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceFabricContrib;
using System.Fabric;

namespace EnterpriseContact
{
    public abstract class BaseAppService : StatelessRemotingService
    {
        protected readonly IMapper _mapper;
        protected readonly DbContextOptions<ServiceDbContext> _dbOptions;

        public BaseAppService(StatelessServiceContext serviceContext,
            DbContextOptions<ServiceDbContext> dbOptions,
            IMapper mapper)
            : base(serviceContext)
        {
            _mapper = mapper;
            _dbOptions = dbOptions;
        }
    }
}
