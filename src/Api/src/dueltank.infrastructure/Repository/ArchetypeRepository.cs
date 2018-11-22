using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using dueltank.core.Models.Archetypes;
using dueltank.core.Models.Search.Archetypes;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class ArchetypeRepository : IArchetypeRepository
    {
        private readonly DueltankDbContext _dbContext;

        private const string ArchetypeSearchQuery = "EXEC ArchetypeSearch @SearchTerm, @PageSize, @PageIndex, @TotalRowsCount out";
        private const string CardsByArchetypeIdQuery = "EXEC CardsByArchetypeId @archetypeId";

        public ArchetypeRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ArchetypeSearchResult> Search(ArchetypeSearchCriteria searchCriteria)
        {
            var response = new ArchetypeSearchResult();
            var sqlParameters = new List<object>();

            var totalRowsCount = new SqlParameter
            {
                ParameterName = "TotalRowsCount",
                Value = 0,
                Direction = ParameterDirection.Output
            };

            sqlParameters.Add(totalRowsCount);
            sqlParameters.Add(new SqlParameter("@SearchTerm", (object)searchCriteria.SearchTerm ?? DBNull.Value));
            sqlParameters.Add(new SqlParameter("PageSize", searchCriteria.PageSize));
            sqlParameters.Add(new SqlParameter("@PageIndex", searchCriteria.PageIndex));

            response.Archetypes = await _dbContext.ArchetypeSearch.FromSql(ArchetypeSearchQuery, sqlParameters.ToArray()).ToListAsync();
            response.TotalRecords = (int)totalRowsCount.Value;

            return response;
        }

        public async Task<ArchetypeByIdResult> ArchetypeById(long archetypeId)
        {
            var result = new ArchetypeByIdResult();

            result.Archetype = _dbContext.Archetype.AsNoTracking().SingleOrDefault(a => a.Id == archetypeId);
            result.Cards = await _dbContext.CardSearch.FromSql(CardsByArchetypeIdQuery, new SqlParameter("@ArchetypeId", archetypeId)).ToListAsync();

            return result;
        }

        public async Task<MostRecentArchetypesResult> MostRecentArchetypes(int pageSize)
        {
            var response = new MostRecentArchetypesResult();

            const string searchSqlQuery = "MostRecentArchetypes @PageSize";

            response.Archetypes = await _dbContext.ArchetypeSearch.FromSql(searchSqlQuery, new SqlParameter("@PageSize", pageSize)).ToListAsync();

            return response;
        }

    }
}