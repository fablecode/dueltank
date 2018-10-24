using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using dueltank.core.Models.Search.Banlist;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class BanlistRepository : IBanlistRepository
    {
        private readonly DueltankDbContext _dbContext;

        private const string LatestBanlistByFormatAcronymQuery = "EXEC LatestBanlistByFormatAcronym @formatAcronym, @releaseDate out";

        public BanlistRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<BanlistCardSearchResult> LatestBanlistByFormatAcronym(string formatAcronym)
        {
            var result = new BanlistCardSearchResult();

            var sqlParameters = new List<object>
            {
                new SqlParameter("@formatAcronym", formatAcronym)
            };

            var banlistReleaseDate = new SqlParameter
            {
                ParameterName = "releaseDate",
                Value = default(DateTime),
                Direction = ParameterDirection.Output
            };

            sqlParameters.Add(banlistReleaseDate);

            result.Cards = await _dbContext.BanlistCardSearch.FromSql(LatestBanlistByFormatAcronymQuery, sqlParameters.ToArray()).ToListAsync();
            result.ReleaseDate = (DateTime) banlistReleaseDate.Value;

            return result;
        }
    }
}