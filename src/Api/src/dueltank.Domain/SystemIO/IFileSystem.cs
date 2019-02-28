namespace dueltank.Domain.SystemIO
{
    public interface IFileSystem
    {
        byte[] ReadAllBytes(string path);
    }
}