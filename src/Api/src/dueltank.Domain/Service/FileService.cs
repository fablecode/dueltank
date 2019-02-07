using System.IO;
using dueltank.core.Services;

namespace dueltank.Domain.Service
{
    public class FileService : IFileService
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}