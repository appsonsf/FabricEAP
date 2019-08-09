using Attachment.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SystemDrawingImage = System.Drawing.Image;

namespace AttachmentUnitTest.Services
{
    [TestClass]
    public class PhotoResizer_ResizeShould
    {
        [TestMethod]
        public void SavedAndSizeIsCorrect()
        {
            var filePath = "..\\..\\..\\InputAssets\\IMG_2301.jpg";
            var saveFilePath = Path.Combine(Path.GetTempPath(), "IMG_2301.jpg");
            using (var load = new FileStream(filePath, FileMode.Open))
            {
                var image = SystemDrawingImage.FromStream(load, false, false);
                load.Position = 0;
                using (var save = new FileStream(saveFilePath, FileMode.Create))
                {
                    PhotoResizer.Resize(load, save, image.Width, image.Height);
                }
            }

            Assert.IsTrue(File.Exists(saveFilePath));
            using (var fs = new FileStream(saveFilePath, FileMode.Open))
            {
                Assert.IsTrue(fs.Length > 0);
                var image = SystemDrawingImage.FromStream(fs, false, false);
                Assert.IsTrue(image.Width == PhotoResizer.DefaultThumbnailSize
                    || image.Height == PhotoResizer.DefaultThumbnailSize);
            }
        }
    }
}
