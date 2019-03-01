using System.IO;
using dueltank.Domain.SystemIO;

namespace dueltank.infrastructure.SystemIO
{
    public class DirectorySystem : IDirectorySystem
    {
        public string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }
    }
}