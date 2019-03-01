namespace dueltank.Domain.SystemIO
{
    public interface IDirectorySystem
    {
        string[] GetFiles(string path, string searchPattern);
    }
}