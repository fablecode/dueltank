using dueltank.core.Services;
using dueltank.Domain.SystemIO;
using System.Linq;

namespace dueltank.Domain.Service
{
    public class ImageService : IImageService
    {
        private readonly IDirectorySystem _directorySystem;

        public ImageService(IDirectorySystem  directorySystem)
        {
            _directorySystem = directorySystem;
        }
        public string GetImagePath(string imageName, string directoryPath, string noImageFoundFile)
        {
            var searchPattern = imageName + ".*";

            var imageFiles = _directorySystem.GetFiles(directoryPath, searchPattern);

            var imageFilePath = directoryPath + @"\" + noImageFoundFile;

            if (imageFiles.Any())
                imageFilePath = imageFiles.First();

            return imageFilePath;
        }
    }
}