using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class TipService : ITipService
    {
        private readonly ITipRepository _tipRepository;

        public TipService(ITipRepository tipRepository)
        {
            _tipRepository = tipRepository;
        }
        public Task<IList<TipSection>> GetTipsByCardId(long cardId)
        {
            return _tipRepository.GetByCardId(cardId);
        }
    }
}