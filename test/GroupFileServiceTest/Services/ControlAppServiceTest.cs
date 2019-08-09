using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupFile;
using GroupFile.Dtos;
using GroupFile.Entities;
using GroupFile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroupFileServiceTest.Services
{
    [TestClass]
    public class ControlAppServiceTest : TheGroupFileAppServiceTestBase
    {
        private const string Store_Md5 = "xxxxxxmd5xxxx";
        private const string Store_Location = "http://github.com";
        private const string File_Name = "学习强国";

        private readonly Guid File_Id = new Guid("02BA9FC9-FAF9-4944-B77F-A0FB69E613B0");
        private readonly Guid File_GroupId = new Guid("02BA9FC9-FAF9-4944-B77F-A0FB69E613B1");
        private readonly Guid Uploader_Id = new Guid("12BA9FC9-FAF9-4944-B77F-A0FB69E613B1");



        // delete
        //[TestMethod]
        //public async Task CheckFileExistAsync_Test()
        //{
        //    //Arrage:
        //    var service = await ArrageAsync();

        //    //Act:
        //    var b = await service.CheckFileExistAsync(Store_Md5);

        //    //Assert:
        //    Assert.IsTrue(b);
        //}

        /// <summary>
        /// 获取下载地址
        /// </summary>
        /// <returns></returns>
        //[TestMethod]
        //public async Task DownloadFileAsync_Test()
        //{
        //    //Arrage:
        //    var service = await ArrageAsync();

        //    //Act:
        //    var _location = await service.DownloadFileAsync(File_Id);
        //    Assert.AreEqual(Store_Location, _location);
        //}

        //[TestMethod]
        //public async Task GetFileItemAsync_Test()
        //{
        //    //Arrage:
        //    var service = await this.ArrageAsync();

        //    //Act:
        //    var fileItem = await service.GetFileItemAsync(File_Id);

        //    Assert.IsNotNull(fileItem);
        //    Assert.AreEqual(File_Name, fileItem.Name);
        //}

        //[TestMethod]
        //public async Task GetFileItemsAsync_Test()
        //{
        //    //Arrage:
        //    var service = await this.ArrageAsync();

        //    //Act:
        //    var files = await service.GetFileItemsAsync(File_GroupId, 1);

        //    Assert.IsNotNull(files);
        //    Assert.IsTrue(files.Count == 1);
        //    Assert.AreEqual(File_Name, files.First().Name);
        //}


        //[TestMethod]
        //public async Task SaveFileInfoAsync_Test()
        //{
        //    //Arrage:
        //    var (_, dbOptions) = OpenDb();
        //    var mapper = CreateMapper();
        //    var service = new ControlAppService(statelessServiceContext, dbOptions, mapper);

        //    var input = new FileInfoInput()
        //    {
        //        GroupId = File_GroupId,
        //        Location = Store_Location,
        //        MD5 = Store_Md5,
        //        Name = File_Name,
        //        Size = 40,
        //        UploaderId = Guid.NewGuid(),
        //    };
        //    await service.SaveFileInfoAsync(input);
        //}

        //delete
        //[TestMethod]
        //public async Task SecUploadFileAsync_Test()
        //{
        //    //Arrage:
        //    var services = await ArrageAsync();

        //    //Act:
        //    var fileItem = await services.UploadFileAsync(new UploadInput()
        //    {
        //        FileName = "NewSec",
        //        GroupId = File_GroupId,
        //        StoreId = Store_Md5
        //    }, Uploader_Id);


        //    Assert.IsNotNull(fileItem);
        //    Assert.AreEqual(Uploader_Id, fileItem.UploaderId);
        //    Assert.AreEqual("NewSec", fileItem.Name);
        //}



        //private async Task<ControlAppService> ArrageAsync()
        //{
        //    var (_, dbOptions) = OpenDb();
        //    var mapper = CreateMapper();
        //    var fileStore = new FileStore()
        //    {
        //        Id = Store_Md5,
        //        Items = new List<FileItem>()
        //        {
        //            new FileItem()
        //            {
        //                DownloadAmount = 0,
        //                GroupId = File_GroupId,
        //                Id = File_Id,
        //                Name = File_Name,
        //                StoreId = Store_Md5,
        //                UpdatedOn = DateTimeOffset.Now,
        //                UploaderId = Guid.NewGuid()
        //            }
        //        },
        //        Location = Store_Location,
        //        Size = 10
        //    };
        //    var service = new ControlAppService(statelessServiceContext, dbOptions, mapper);
        //    using (var db = new ServiceDbContext(dbOptions))
        //    {
        //        var s = db.FileStores.ToList();
        //        db.FileStores.Add(fileStore);
        //        await db.SaveChangesAsync();
        //    }

        //    return service;
        //}

        //[TestInitialize]
        //public void Cleanup()
        //{
        //    var (_, dbOptions) = OpenDb();
        //    using (var db = new ServiceDbContext(dbOptions))
        //    {
        //        var stores = db.FileStores.Include(u => u.Items).ToList();
        //        db.FileStores.RemoveRange(stores);
        //        db.SaveChanges();
        //    }

        //}
    }
}
