using Common;
using EnterpriseContact;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact.Services
{
    public class EmployeeCacheService : IEmployeeCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IEmployeeAppService _employeeAppService;

        public EmployeeCacheService(IMemoryCache cache,
            IEmployeeAppService employeeAppService)
        {
            _cache = cache;
            _employeeAppService = employeeAppService;
        }

        class MappingData
        {
            public List<EmployeeCacheOutput> List = new List<EmployeeCacheOutput>();

            public Dictionary<string, EmployeeCacheOutput> ByNumber = new Dictionary<string, EmployeeCacheOutput>();
            public Dictionary<Guid, EmployeeCacheOutput> ByUserId = new Dictionary<Guid, EmployeeCacheOutput>();
            public Dictionary<Guid, EmployeeCacheOutput> ById = new Dictionary<Guid, EmployeeCacheOutput>();

        }

        private async Task<MappingData> EnsureLoadMappingData()
        {
            if (!_cache.TryGetValue(CacheKeys.Common_EmployeeList, out MappingData data))
            {
                data = new MappingData();
                var dto = await _employeeAppService.GetCacheDataAsync();
                if (dto?.Count > 0)
                {
                    data.List = dto;

                    foreach (var item in dto)
                    {
                        if (!string.IsNullOrEmpty(item.Number))
                            data.ByNumber[item.Number] = item;

                        if (item.UserId.HasValue)
                            data.ByUserId[item.UserId.Value] = item;

                        data.ById[item.Id] = item;
                    }
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(CacheKeys.DefaultCacheAbsoluteExpiration);

                _cache.Set(CacheKeys.EnterpriseContact_PositionList, data, cacheEntryOptions);
            }
            return data;
        }

        public async Task<List<EmployeeCacheOutput>> ByUserIdAsync(params Guid[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            if (keys.Length == 0) return new List<EmployeeCacheOutput>();

            var data = await EnsureLoadMappingData();

            var result = new List<EmployeeCacheOutput>();
            foreach (var key in keys)
            {
                if (data.ByUserId.ContainsKey(key))
                {
                    result.Add(data.ByUserId[key]);
                }
            }

            return result;
        }

        public async Task<List<EmployeeCacheOutput>> ByNumberAsync(params string[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            if (keys.Length == 0) return new List<EmployeeCacheOutput>();

            var data = await EnsureLoadMappingData();

            var result = new List<EmployeeCacheOutput>();
            foreach (var key in keys)
            {
                if (data.ByNumber.ContainsKey(key))
                {
                    result.Add(data.ByNumber[key]);
                }
            }

            return result;
        }


        public async Task<List<EmployeeCacheOutput>> ByIdAsync(params Guid[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            if (keys.Length == 0) return new List<EmployeeCacheOutput>();

            var data = await EnsureLoadMappingData();

            var result = new List<EmployeeCacheOutput>();
            foreach (var key in keys)
            {
                if (data.ById.ContainsKey(key))
                {
                    result.Add(data.ById[key]);
                }
            }

            return result;
        }


        public void ClearCache()
        {
            _cache.Remove(CacheKeys.Common_EmployeeList);
        }
    }
}
