using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class DeckTypeService : IDeckTypeService
    {
        private readonly IDeckTypeRepository _deckTypeRepository;

        public DeckTypeService(IDeckTypeRepository deckTypeRepository)
        {
            _deckTypeRepository = deckTypeRepository;
        }

        public Task<IEnumerable<DeckType>> AllDeckTypes()
        {
            return _deckTypeRepository.AllDeckTypes();
        }
    }
}