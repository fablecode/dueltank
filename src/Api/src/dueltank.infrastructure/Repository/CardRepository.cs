using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;
using dueltank.core.Models.Search.Cards;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly DueltankDbContext _dbContext;

        private const string CardSearchQuery = "EXEC CardSearch @banlistId, @limitId, @categoryId, @subCategoryId, @attributeId, @typeId, @lvlRank, @atk, @def, @searchTerm, @pageSize, @pageIndex, @filteredRowsCount out";
        private const string CardSearchByNameQuery = "EXEC CardSearchByName @name";

        public CardRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Card> GetCardByNumber(long cardNumber)
        {
            return await _dbContext.Card
                            .AsNoTracking()
                            .SingleOrDefaultAsync(c => c.CardNumber == cardNumber);
        }

        public async Task<CardSearch> GetCardByName(string name)
        {
            return await _dbContext.CardSearch.FromSql(CardSearchByNameQuery, new SqlParameter("@name", name)).SingleOrDefaultAsync();
        }
        public async Task<Card> GetCardById(long id)
        {
            return await _dbContext.Card
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CardSearchResult> Search(CardSearchCriteria searchCriteria)
        {
            var result = new CardSearchResult();

            var sqlParameters = new List<object>();

            var filteredRowsCount = new SqlParameter
            {
                ParameterName = "filteredRowsCount",
                Value = 0,
                Direction = ParameterDirection.Output
            };

            sqlParameters.Add(filteredRowsCount);

            sqlParameters.Add(new SqlParameter("@searchTerm", (object)searchCriteria.SearchTerm ?? DBNull.Value));
            sqlParameters.Add(new SqlParameter("@limitId", searchCriteria.LimitId));
            sqlParameters.Add(new SqlParameter("@banlistId", searchCriteria.BanlistId));
            sqlParameters.Add(new SqlParameter("@categoryId", searchCriteria.CategoryId));
            sqlParameters.Add(new SqlParameter("@subCategoryId", searchCriteria.SubCategoryId));
            sqlParameters.Add(new SqlParameter("@attributeId", searchCriteria.AttributeId));
            sqlParameters.Add(new SqlParameter("@typeId", searchCriteria.TypeId));
            sqlParameters.Add(new SqlParameter("@lvlRank", searchCriteria.LvlRank));
            sqlParameters.Add(new SqlParameter("@atk", searchCriteria.Atk));
            sqlParameters.Add(new SqlParameter("@def", searchCriteria.Def));
            sqlParameters.Add(new SqlParameter("@pageSize", searchCriteria.PageSize));
            sqlParameters.Add(new SqlParameter("@pageIndex", searchCriteria.PageIndex));

            result.Cards = await _dbContext.CardSearch.FromSql(CardSearchQuery, sqlParameters.ToArray()).ToListAsync();
            result.TotalRecords = (int) filteredRowsCount.Value;

            return result;
        }
    }
}