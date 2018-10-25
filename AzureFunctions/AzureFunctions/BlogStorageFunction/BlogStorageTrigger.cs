using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ImageResizer;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;

namespace BlogStorageFunction
{
    public static class Function1
    {
        [FunctionName("BlogStorageTrigger")]
        public static void Run([BlobTrigger("blogstoragetrigger/{name}", Connection = "DungStorageAccount")]Stream image, string name, ILogger log,
            [Blob("blogstoragetrigger-images-sm/{name}", FileAccess.Write, Connection = "DungStorageAccount")] Stream imageSmall)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {image.Length} Bytes");

            image.Position = 0;
            var imageOrigin = Image.Load(image);

            var newImage = imageOrigin.Clone(
                ctx => ctx.Resize(
                    new ResizeOptions
                    {
                        Size = new SixLabors.Primitives.Size(640, 400),
                        Mode = ResizeMode.Crop
                    }));

            Stream outputStream = new MemoryStream();

            newImage.Save(outputStream, new JpegEncoder());

            outputStream.Position = 0;
            outputStream.CopyTo(imageSmall);
        }

        public enum ImageSize { ExtraSmall, Small, Medium }

        private static Dictionary<ImageSize, (int, int)> imageDimensionsTable = new Dictionary<ImageSize, (int, int)>() {
            { ImageSize.ExtraSmall, (320, 200) },
            { ImageSize.Small,      (640, 400) },
            { ImageSize.Medium,     (800, 600) }
        };
    }
}
