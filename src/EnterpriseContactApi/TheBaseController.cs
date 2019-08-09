using Common;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    public abstract class TheBaseController : EapBaseController
    {
        protected readonly IMapper _mapper;
        protected readonly IMemoryCache _cache;
        protected readonly IDepartmentAppService _departmentAppService;
        protected readonly IPositionAppService _positionAppService;

        public TheBaseController(
            IMemoryCache cache,
            IMapper mapper,
            IDepartmentAppService departmentAppService,
            IPositionAppService positionAppService
            )
        {
            _cache = cache;
            _mapper = mapper;
            _departmentAppService = departmentAppService;
            _positionAppService = positionAppService;
        }

        protected Dictionary<Guid, DepartmentListOutput> _departmentListCache;
        protected async Task EnsureDepartmentListCacheAsync()
        {
            if (_departmentListCache == null)
            {
                if (!_cache.TryGetValue(CacheKeys.EnterpriseContact_DepartmentList, out Dictionary<Guid, DepartmentListOutput> data))
                {
                    var dto = await _departmentAppService.GetAllListAsync();
                    if (dto == null || dto.Count == 0)
                        data = new Dictionary<Guid, DepartmentListOutput>();
                    else
                        data = dto.ToDictionary(o => o.Id, o => o);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetAbsoluteExpiration(CacheKeys.DefaultCacheAbsoluteExpiration);

                    _cache.Set(CacheKeys.EnterpriseContact_DepartmentList, data, cacheEntryOptions);
                }
                _departmentListCache = data;
            }
        }

        protected Dictionary<Guid, PositionListOutput> _positionListCache;

        protected async Task EnsurePositionListCacheAsync()
        {
            if (_positionListCache == null)
            {
                if (!_cache.TryGetValue(CacheKeys.EnterpriseContact_PositionList, out Dictionary<Guid, PositionListOutput> data))
                {
                    var dto = await _positionAppService.GetAllListAsync();
                    if (dto == null || dto.Count == 0)
                        data = new Dictionary<Guid, PositionListOutput>();
                    else
                        data = dto.ToDictionary(o => o.Id, o => o);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetAbsoluteExpiration(CacheKeys.DefaultCacheAbsoluteExpiration);

                    _cache.Set(CacheKeys.EnterpriseContact_PositionList, data, cacheEntryOptions);
                }
                _positionListCache = data;
            }
        }
    }
}
