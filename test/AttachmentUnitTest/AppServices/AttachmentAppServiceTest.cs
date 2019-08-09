using Attachment;
using Attachment.Entities;
using AttachmentStateService;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitTestCommon;

namespace AttachmentUnitTest.AppServices
{
    [TestClass]
    public class AttachmentAppServiceTest : AppServiceTestBase
    {
        [TestMethod]
        public async Task Test_AddOrUpdateAsync_GetByIdAsync()
        {
            var id = Guid.NewGuid().ToString("N");
            var userId = Guid.NewGuid();
            var dto = new AttachmentItem
            {
                ContextId = Guid.Empty,
                Id = id,
                Location = $"attachments/2018/12/28/{id}.ext",
                Size = 100,
                Status = UploadStatus.Uploading,
                Uploaded = DateTimeOffset.UtcNow,
                UploadBy = userId
            };
            var target = new AttachmentAppService(statefulServiceContext, stateManager);
            await target.AddOrUpdateAsync(dto);

            var result = await target.GetByIdAsync(id);
            result.Location.Should().Be(dto.Location);
            result.Status.Should().Be(dto.Status);

            dto.Status = UploadStatus.Uploaded;
            await target.AddOrUpdateAsync(dto);
            result = await target.GetByIdAsync(id);
            result.Status.Should().Be(UploadStatus.Uploaded);
        }

        [TestMethod]
        public async Task Test_GetByIdAsync_ReturnNull()
        {
            var target = new AttachmentAppService(statefulServiceContext, stateManager);
            var result2 = await target.GetByIdAsync(Guid.NewGuid().ToString("N"));
            result2.Should().BeNull();
        }
    }
}
