﻿using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.Domain.Repository
{
    public interface ITipRepository
    {
        Task<IList<TipSection>> GetByCardId(long cardId);
    }
}