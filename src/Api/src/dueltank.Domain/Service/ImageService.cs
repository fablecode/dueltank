using System.IO;
using System.Linq;
using dueltank.core.Services;

namespace dueltank.Domain.Service
{
    public class ImageService : IImageService
    {
        public string GetImagePath(string imageName, string directoryPath, string noImageFoundFile)
        {
            var searchPattern = imageName + ".*";

            var imageFiles = Directory.GetFiles(directoryPath, searchPattern);

            var imageFilePath = directoryPath + @"\" + noImageFoundFile;

            if (imageFiles.Any())
                imageFilePath = imageFiles.First();

            return imageFilePath;
        }
    }
}