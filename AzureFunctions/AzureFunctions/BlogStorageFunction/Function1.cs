using System.Collections.Generic;
using System.IO;
using ImageResizer;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BlogStorageFunction
{
    public static class Function1
    {
        [FunctionName("BlogStorageTrigger")]
        public static void Run([BlobTrigger("blogstorageinout/{name}", Connection = "HieuStorageAccount")]Stream image, string name, ILogger log,
            [Blob("sample-images-sm/{name}", FileAccess.Write)] Stream imageSmall,
            [Blob("sample-images-md/{name}", FileAccess.Write)] Stream imageMedium)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {image.Length} Bytes");
            var imageBuilder = ImageResizer.ImageBuilder.Current;
            var size = imageDimensionsTable[ImageSize.Small];

            imageBuilder.Build(image, imageSmall,
                new ResizeSettings(size.Item1, size.Item2, FitMode.Max, null), false);

            image.Position = 0;
            size = imageDimensionsTable[ImageSize.Medium];

            imageBuilder.Build(image, imageMedium,
                new ResizeSettings(size.Item1, size.Item2, FitMode.Max, null), false);
        }

        public enum ImageSize { ExtraSmall, Small, Medium }

        private static Dictionary<ImageSize, (int, int)> imageDimensionsTable = new Dictionary<ImageSize, (int, int)>() {
            { ImageSize.ExtraSmall, (320, 200) },
            { ImageSize.Small,      (640, 400) },
            { ImageSize.Medium,     (800, 600) }
        };
    }
}
