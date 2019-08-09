using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper.QueryableExtensions;

namespace EnterpriseContact
{
    public class PositionAppService : BaseAppService, IPositionAppService
    {
        public PositionAppService(StatelessServiceContext serviceContext, DbContextOptions<ServiceDbContext> dbOptions, IMapper mapper) 
            : base(serviceContext, dbOptions, mapper)
        {
        }

        public async Task<List<PositionListOutput>> GetAllListAsync()
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Positions
                            select item;

                return await query.ProjectTo<PositionListOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }
    }
}
