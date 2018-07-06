using System.IO;
using System.Linq;

namespace dueltank.application.Helpers
{
    public static class ImageHelper
    {
        public static string GetImagePath(string imageName, string directoryPath, string noImageFoundFile)
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