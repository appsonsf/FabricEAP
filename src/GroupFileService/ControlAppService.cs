using AutoMapper;
using GroupFile.Dtos;
using GroupFile.Entities;
using GroupFile.Models;
using Microsoft.EntityFrameworkCore;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;

namespace GroupFile
{
    public class ControlAppService : StatelessRemotingService, IControlAppService
    {
        private readonly DbContextOptions<ServiceDbContext> _dbContextOptions;
        private readonly IMapper _mapper;

        public ControlAppService(StatelessServiceContext serviceContext, DbContextOptions<ServiceDbContext> dbContextOptions, IMapper mapper) : base(serviceContext)
        {
            _mapper = mapper;
            _dbContextOptions = dbContextOptions;
        }

        public async Task DeleteFileItemAsync(Guid fileId)
        {
            using (var db = new ServiceDbContext(_dbContextOptions))
            {
                try
                {
                    var result = db.FileItems.FirstOrDefault(fi => fi.Id.Equals(fileId));
                    if (result != null)
                    {
                        db.FileItems.Remove(result);
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    ServiceEventSource.Current.Message("DeleteFileItemAsync try delete file:{0},but do not found!,exception:{1}", fileId, e.ToString());
                }
            }
        }

        public async Task<string> DownloadFileAsync(Guid fileId)
        {
            using (var db = new ServiceDbContext(_dbContextOptions))
            {
                try
                {
                    var result = db.FileItems.FirstOrDefault(fi => fi.Id.Equals(fileId));
                    if (result != null)
                    {
                        result.DownloadAmount++;
                        await db.SaveChangesAsync();
                        return result.StoreId;
                    }
                }
                catch (Exception e)
                {
                    ServiceEventSource.Current.Message("DownloadFileAsync try download file:{0},but do not found!,exception:{1}", fileId, e.ToString());
                }
                return null;
            }
        }

        public Task<FileItemDto> GetFileItemAsync(Guid fileId)
        {
            using (var db = new ServiceDbContext(_dbContextOptions))
            {
                var data = db.FileItems.FirstOrDefault(fi => fi.Id.Equals(fileId));
                return Task.FromResult(_mapper.Map<FileItemDto>(data));
            }
        }

        public Task<List<FileItemDto>> GetFileItemsAsync(Guid groupId, int page, int pageCount = 20)
        {
            if (page <= 0) throw new ArgumentOutOfRangeException("page", "page must > 0");
            using (var db = new ServiceDbContext(_dbContextOptions))
            {
                var skip = (page - 1) * pageCount;
                var datas = db.FileItems.Where((item) => item.GroupId.Equals(groupId)).OrderByDescending(f => f.UpdatedOn).Skip(skip).Take(pageCount).ToList();
                var result = new List<FileItemDto>();
                if (datas != null)
                {
                    foreach (var item in datas)
                    {
                        var data = _mapper.Map<FileItemDto>(item);
                        result.Add(data);
                    }
                }
                return Task.FromResult(result);
            }
        }

        public async Task<FileItemDto> UploadFileAsync(UploadInput param, Guid uploaderId)
        {
            using (var db = new ServiceDbContext(_dbContextOptions))
            {
                var fileItem = new FileItem
                {
                    Id = Guid.NewGuid(),
                    DownloadAmount = 0,
                    GroupId = param.GroupId,
                    Name = param.FileName,
                    StoreId = param.StoreId,
                    UpdatedOn = DateTimeOffset.UtcNow,
                    UploaderId = uploaderId,
                };
                await db.FileItems.AddAsync(fileItem);
                await db.SaveChangesAsync();
                return _mapper.Map<FileItemDto>(fileItem);
            }
        }
    }

}
