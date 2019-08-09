using PhotoSauce.MagicScaler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Attachment.Services
{
    public static class PhotoResizer
    {
        public const int DefaultThumbnailSize = 150;
        const int Quality = 75;

        public static void Resize(Stream inputStream, Stream outputStream, int oldWidth, int oldHeight, int size = DefaultThumbnailSize)
        {
            var scaled = ScaledSize(oldWidth, oldHeight, size);

            var settings = new ProcessImageSettings()
            {
                Width = scaled.width,
                Height = scaled.height,
                ResizeMode = CropScaleMode.Max,
                SaveFormat = FileFormat.Jpeg,
                JpegQuality = Quality,
                JpegSubsampleMode = ChromaSubsampleMode.Subsample420
            };
            MagicImageProcessor.ProcessImage(inputStream, outputStream, settings);
        }

        static (int width, int height) ScaledSize(int inWidth, int inHeight, int outSize)
        {
            int width, height;
            if (inWidth > inHeight)
            {
                width = outSize;
                height = (int)Math.Round(inHeight * outSize / (double)inWidth);
            }
            else
            {
                width = (int)Math.Round(inWidth * outSize / (double)inHeight);
                height = outSize;
            }

            return (width, height);
        }
    }
}
