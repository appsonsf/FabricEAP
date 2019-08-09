using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupFile;
using GroupFile.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroupFileServiceTest.Services
{
    [TestClass]
    public class MapperTest : TheGroupFileAppServiceTestBase
    {
        //[TestMethod]
        //public void Mapper_Test()
        //{
        //    var mapper = CreateMapper();
        //    var groupid = Guid.NewGuid();
        //    var fileId = Guid.NewGuid();
        //    var updaloadOn = DateTimeOffset.Now;
        //    var uploaderId = Guid.NewGuid();
        //    var name = "xxx";
        //    var fileItem = new FileItem()
        //    {
        //        DownloadAmount = 10,
        //        GroupId = groupid,
        //        Id = fileId,
        //        Name = name,
        //        StoreId = "md5",
        //        UpdatedOn = updaloadOn,
        //        UploaderId = uploaderId,
        //        Store = new FileStore()
        //        {
        //            Size = 10,
        //        }
        //    };
        //    var result = mapper.Map<FileItemDto>(fileItem);
        //    Assert.AreEqual(uploaderId, result.UploaderId);
        //    Assert.AreEqual(name, result.Name);
        //    Assert.AreEqual(fileId, result.Id);
        //    Assert.AreEqual(updaloadOn, result.UpdatedOn);
        //    Assert.AreEqual(10, result.DownloadAmount);
        //    Assert.AreEqual(10, result.Size);
        //}
    }
}
