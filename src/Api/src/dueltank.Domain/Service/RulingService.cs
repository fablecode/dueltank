using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class RulingService : IRulingService
    {
        private readonly IRulingRepository _rulingRepository;

        public RulingService(IRulingRepository rulingRepository)
        {
            _rulingRepository = rulingRepository;
        }

        public Task<IList<RulingSection>> GetRulingsByCardId(long cardId)
        {
            return _rulingRepository.GetByCardId(cardId);
        }
    }
}