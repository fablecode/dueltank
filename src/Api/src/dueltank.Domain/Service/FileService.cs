using dueltank.core.Services;
using dueltank.Domain.SystemIO;

namespace dueltank.Domain.Service
{
    public class FileService : IFileService
    {
        private readonly IFileSystem _fileSystem;

        public FileService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        public byte[] ReadAllBytes(string path)
        {
            return _fileSystem.ReadAllBytes(path);
        }
    }
}