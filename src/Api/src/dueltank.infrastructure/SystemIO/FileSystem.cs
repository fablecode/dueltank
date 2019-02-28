using System.IO;
using dueltank.Domain.SystemIO;

namespace dueltank.infrastructure.SystemIO
{
    public class FileSystem : IFileSystem
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}