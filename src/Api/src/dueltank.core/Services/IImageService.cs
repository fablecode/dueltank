namespace dueltank.core.Services
{
    public interface IImageService
    {
        string GetImagePath(string imageName, string directoryPath, string noImageFoundFile);
    }
}