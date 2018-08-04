using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class LimitService : ILimitService
    {
        private readonly ILimitRepository _limitRepository;

        public LimitService(ILimitRepository limitRepository)
        {
            _limitRepository = limitRepository;
        }
        public Task<IList<Limit>> AllLimits()
        {
            return _limitRepository.AllLimits();
        }
    }
}