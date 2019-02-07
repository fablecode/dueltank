namespace dueltank.core.Services
{
    public interface IFileService
    {
        byte[] ReadAllBytes(string path);
    }
}