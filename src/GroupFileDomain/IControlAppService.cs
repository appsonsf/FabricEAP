using GroupFile.Dtos;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupFile
{
    public interface IControlAppService : IService
    {
        /// <summary>
        /// 群组分页获取文件列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="page"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        Task<List<FileItemDto>> GetFileItemsAsync(Guid groupId, int page, int pageCount);

        /// <summary>
        /// 获取md5值 即 AttachmentItem id 并增加下载量
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<string> DownloadFileAsync(Guid fileId);

        /// <summary>
        /// 秒传文件
        /// </summary>
        /// <param name="param"></param>
        /// <param name="uploaderId"></param>
        /// <returns></returns>
        Task<FileItemDto> UploadFileAsync(UploadInput param, Guid uploaderId);


        /// <summary>
        /// 查询文件信息
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<FileItemDto> GetFileItemAsync(Guid fileId);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task DeleteFileItemAsync(Guid fileId);

    }
}
