using Attachment;
using Attachment.Controllers;
using Attachment.Entities;
using Attachment.Services;
using Attachment.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnitTestCommon;

namespace AttachmentUnitTest.Controllers
{
    [TestClass]
    public class AttachmentControllerTest : ControllerTestBase
    {
        [TestMethod]
        public async Task TestGetStatusAsync()
        {
            IAttachmentAppService factory(string id)
            {
                var appService = Substitute.For<IAttachmentAppService>();
                appService.GetByIdAsync(id)
                    .ReturnsForAnyArgs(new AttachmentItem
                    {
                        Id = id,
                        Status = UploadStatus.Uploaded
                    });
                return appService;
            };
            var target = new AttachmentController(factory, Substitute.For<IAttachmentStoreService>(),
                Substitute.For<IMemoryCache>());
            var result = await target.GetStatusAsync(Guid.NewGuid().ToString("N"));
            var data = result.Value;
            data.Should().Be(UploadStatus.Uploaded);
        }

        [TestMethod]
        public async Task TestUploadFileAsync_Uploaded()
        {
            IAttachmentAppService factory(string id)
            {
                var appService = Substitute.For<IAttachmentAppService>();
                appService.GetByIdAsync(id)
                    .ReturnsForAnyArgs(new AttachmentItem
                    {
                        Id = id,
                        Status = UploadStatus.Uploaded
                    });
                return appService;
            };
            var storeService = Substitute.For<IAttachmentStoreService>();
            var formFile = Substitute.For<IFormFile>();
            var target = new AttachmentController(factory, storeService,
                Substitute.For<IMemoryCache>());
            var vm = new UploadVm
            {
                ContextId = Guid.Empty,
                File = formFile,
                MD5 = Guid.NewGuid().ToString("N")
            };
            var result = await target.UploadFileAsync(vm);
            var responseData = result.Value;
            responseData.Data.Should().Be(UploadStatus.Uploaded);
            await storeService.DidNotReceiveWithAnyArgs().StoreFileAsync(vm, DateTimeOffset.UtcNow);
        }

        [TestMethod]
        public async Task TestUploadFileAsync_Stored()
        {
            var formFile = Substitute.For<IFormFile>();
            var vm = new UploadVm
            {
                ContextId = Guid.Empty,
                File = formFile,
                MD5 = Guid.NewGuid().ToString("N")
            };

            var appService = Substitute.For<IAttachmentAppService>();
            appService.AddOrUpdateAsync(Arg.Any<AttachmentItem>())
                .Returns(Task.CompletedTask);
            IAttachmentAppService factory(string id) => appService;

            var storeService = Substitute.For<IAttachmentStoreService>();
            var now = DateTimeOffset.UtcNow;
            storeService.StoreFileAsync(vm, Arg.Any<DateTimeOffset>())
                .Returns("abc.ext");

            var target = new AttachmentController(factory, storeService,
                Substitute.For<IMemoryCache>());
            target.ControllerContext = CreateMockContext();

            var result = await target.UploadFileAsync(vm);
            var responseData = result.Value;
            Assert.AreEqual(UploadStatus.Uploaded, responseData.Data);

            await appService.Received().AddOrUpdateAsync(Arg.Is<AttachmentItem>(o =>
                o.Id == vm.MD5 && o.Location == "abc.ext"
                ));
        }

        [TestMethod]
        public async Task TestDownloadFileAsync()
        {
            var md5 = Guid.NewGuid().ToString("N");
            var formFile = Substitute.For<IFormFile>();

            var appService = Substitute.For<IAttachmentAppService>();
            IAttachmentAppService factory(string id) => appService;

            var storeService = Substitute.For<IAttachmentStoreService>();

            var target = new AttachmentController(factory, storeService,
                Substitute.For<IMemoryCache>());
            target.ControllerContext = CreateMockContext();

            var result = await target.DownloadFileAsync(md5, null);
            result.Should().BeOfType<NotFoundResult>();

            appService.GetByIdAsync(md5)
                .Returns(new AttachmentItem
                {
                    Id = md5,
                    Status = UploadStatus.Uploading,
                    Location = null
                });
            result = await target.DownloadFileAsync(md5, null);
            result.Should().BeOfType<NotFoundResult>();

            appService.GetByIdAsync(md5)
                .Returns(new AttachmentItem
                {
                    Id = md5,
                    Status = UploadStatus.Uploaded,
                    Location = null
                });
            result = await target.DownloadFileAsync(md5, null);
            result.Should().BeOfType<NotFoundResult>();

            var dto = new AttachmentItem
            {
                Id = md5,
                Status = UploadStatus.Uploaded,
                Location = "abc.ext"
            };
            appService.GetByIdAsync(md5)
              .Returns(dto);
            storeService.FillMemoryStreamAsync(dto, Arg.Any<MemoryStream>())
                .Returns(Task.CompletedTask);
            result = await target.DownloadFileAsync(md5, null);
            result.Should().BeOfType<FileStreamResult>();

            await storeService.Received().FillMemoryStreamAsync(dto, Arg.Any<MemoryStream>());
        }

        //TODO FIX TestDownloadThumbnailsAsync 
        //[TestMethod]
        public async Task TestDownloadThumbnailsAsync()
        {
            var md5 = Guid.NewGuid().ToString("N");
            var formFile = Substitute.For<IFormFile>();

            var appService = Substitute.For<IAttachmentAppService>();
            IAttachmentAppService factory(string id) => appService;

            var cacheDict = new Dictionary<string, byte[]>();
            var storeService = Substitute.For<IAttachmentStoreService>();
            var cache = Substitute.For<IMemoryCache>();
            cache.Set(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<TimeSpan>())
                .Returns(x =>
                {
                    return x[1] as byte[];
                });
            cache.TryGetValue(Arg.Any<string>(), out Arg.Any<byte[]>())
                .Returns(x =>
                {
                    if (cacheDict.ContainsKey(x[0].ToString()))
                    {
                        x[1] = cacheDict[x[0].ToString()];
                        return true;
                    }
                    return false;
                });

            var target = new AttachmentController(factory, storeService, cache);
            target.ControllerContext = CreateMockContext();

            var result = await target.DownloadThumbnailsAsync(md5, null);
            result.Should().BeOfType<NotFoundResult>();

            appService.GetByIdAsync(md5)
                .Returns(new AttachmentItem
                {
                    Id = md5,
                    Status = UploadStatus.Uploading,
                    Location = null
                });
            result = await target.DownloadFileAsync(md5, null);
            result.Should().BeOfType<NotFoundResult>();

            appService.GetByIdAsync(md5)
                .Returns(new AttachmentItem
                {
                    Id = md5,
                    Status = UploadStatus.Uploaded,
                    Location = null
                });
            result = await target.DownloadFileAsync(md5, null);
            result.Should().BeOfType<NotFoundResult>();

            var dto = new AttachmentItem
            {
                Id = md5,
                Status = UploadStatus.Uploaded,
                Location = "abc.jpg"
            };
            appService.GetByIdAsync(md5)
              .Returns(dto);
            storeService.FillMemoryStreamAsync(dto, Arg.Any<MemoryStream>())
                .Returns(Task.CompletedTask);
            result = await target.DownloadFileAsync(md5, null);
            result.Should().BeOfType<FileStreamResult>();

            await storeService.Received().FillMemoryStreamAsync(dto, Arg.Any<MemoryStream>());
        }
    }
}
