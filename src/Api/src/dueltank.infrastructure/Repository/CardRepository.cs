using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.Search.Card;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly DueltankDbContext _dbContext;

        private const string CardSearchWithoutSearchTermQuery = "EXEC CardSearchWithoutSearchTerm @banlistId, @limitId, @categoryId, @subCategoryId, @attributeId, @typeId, @lvlRank, @atk, @def, @pageSize, @pageIndex, @filteredRowsCount out";
        private const string CardSearchWithSearchTermQuery = "EXEC CardSearchWithSearchTerm @banlistId, @limitId, @categoryId, @subCategoryId, @attributeId, @typeId, @lvlRank, @atk, @def, @searchTerm, @pageSize, @pageIndex, @filteredRowsCount out";

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

        public async Task<CardSearchResult> Search(CardSearchCriteria searchCriteria)
        {
            var result = new CardSearchResult();

            string query;
            var sqlParameters = new List<object>();

            if (string.IsNullOrWhiteSpace(searchCriteria.SearchTerm))
            {
                query = CardSearchWithoutSearchTermQuery;
            }
            else
            {
                query = CardSearchWithSearchTermQuery;
                sqlParameters.Add(new SqlParameter("@searchTerm", searchCriteria.SearchTerm));
            }

            var filteredRowsCount = new SqlParameter
            {
                ParameterName = "filteredRowsCount",
                Value = 0,
                Direction = ParameterDirection.Output
            };

            sqlParameters.Add(filteredRowsCount);

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

            result.Cards = await _dbContext.CardSearch.FromSql(query, sqlParameters.ToArray()).ToListAsync();
            result.TotalRecords = (int) filteredRowsCount.Value;

            return result;
        }
    }
}