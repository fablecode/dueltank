using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class FormatService : IFormatService
    {
        private readonly IFormatRepository _formatRepository;

        public FormatService(IFormatRepository formatRepository)
        {
            _formatRepository = formatRepository;
        }

        public Task<IList<Format>> AllFormats()
        {
            return _formatRepository.AllFormats();
        }
    }
}