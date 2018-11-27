using System;
using System.Collections.Generic;
using System.Data;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Models.Search.Decks;

namespace dueltank.infrastructure.Repository
{
    public class DeckRepository : IDeckRepository
    {
        private const string DeckSearchQuery = "EXEC DeckSearch @SearchTerm, @PageSize, @PageIndex, @TotalRowsCount out";
        private const string DeckSearchByUserIdQuery = "EXEC DeckSearchByUserId @UserId, @SearchTerm, @PageSize, @PageIndex, @TotalRowsCount out";
        private const string DeckSearchByUsernameQuery = "EXEC DeckSearchByUsername @Username, @SearchTerm, @PageSize, @PageIndex, @TotalRowsCount out";

        private readonly DueltankDbContext _dbContext;

        public DeckRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Deck> Add(Deck deck)
        {
            await _dbContext.Deck.AddAsync(deck);

            await _dbContext.SaveChangesAsync();

            return deck;
        }

        public async Task<DeckDetail> GetDeckById(long id)
        {
            var deckResult = await _dbContext.Deck.AsNoTracking().Include(d => d.User).SingleOrDefaultAsync(d => d.Id == id);

            var cardResult = _dbContext.CardDetail.FromSql("EXEC usp_GetDeckCardsByDeckId @DeckId", new SqlParameter("DeckId", id)).AsNoTracking().ToList();

            if (deckResult != null)
            {
                var deckDetail = DeckDetail.From(deckResult);

                var groupedDeckCards = cardResult.GroupBy(dt => dt.DeckType, dt => dt);

                foreach (var cardGroup in groupedDeckCards)
                {
                    switch (cardGroup.Key.ToLower())
                    {
                        case "main":
                            deckDetail.MainDeck = cardGroup.ToList();
                            break;
                        case "extra":
                            deckDetail.ExtraDeck = cardGroup.ToList();
                            break;
                        case "side":
                            deckDetail.SideDeck = cardGroup.ToList();
                            break;
                    }
                }

                return deckDetail;
            }

            return null;
        }

        public async Task<DeckSearchResult> Search(DeckSearchCriteria searchCriteria)
        {
            var response = new DeckSearchResult();

            var sqlParameters = new List<object>();

            var totalRowsCount = new SqlParameter
            {
                ParameterName = "TotalRowsCount",
                Value = 0,
                Direction = ParameterDirection.Output
            };

            sqlParameters.Add(totalRowsCount);

            sqlParameters.Add(new SqlParameter("@SearchTerm", (object)searchCriteria.SearchTerm ?? DBNull.Value));
            sqlParameters.Add(new SqlParameter("@PageSize", searchCriteria.PageSize));
            sqlParameters.Add(new SqlParameter("@PageIndex", searchCriteria.PageIndex));

            response.Decks = await _dbContext.DeckDetail.FromSql(DeckSearchQuery, sqlParameters.ToArray()).ToListAsync();
            response.TotalRecords = (int)totalRowsCount.Value;

            return response;
        }

        public async Task<DeckSearchResult> Search(DeckSearchByUserIdCriteria searchCriteria)
        {
            var response = new DeckSearchResult();

            var sqlParameters = new List<object>();

            var totalRowsCount = new SqlParameter
            {
                ParameterName = "TotalRowsCount",
                Value = 0,
                Direction = ParameterDirection.Output
            };

            sqlParameters.Add(totalRowsCount);

            sqlParameters.Add(new SqlParameter("@SearchTerm", (object)searchCriteria.SearchTerm ?? DBNull.Value));
            sqlParameters.Add(new SqlParameter("@UserId", searchCriteria.UserId));
            sqlParameters.Add(new SqlParameter("@PageSize", searchCriteria.PageSize));
            sqlParameters.Add(new SqlParameter("@PageIndex", searchCriteria.PageIndex));

            response.Decks = await _dbContext.DeckDetail.FromSql(DeckSearchByUserIdQuery, sqlParameters.ToArray()).ToListAsync();
            response.TotalRecords = (int)totalRowsCount.Value;

            return response;
        }
        public async Task<DeckSearchResult> Search(DeckSearchByUsernameCriteria searchCriteria)
        {
            var response = new DeckSearchResult();

            var sqlParameters = new List<object>();

            var totalRowsCount = new SqlParameter
            {
                ParameterName = "TotalRowsCount",
                Value = 0,
                Direction = ParameterDirection.Output
            };

            sqlParameters.Add(totalRowsCount);

            sqlParameters.Add(new SqlParameter("@SearchTerm", (object)searchCriteria.SearchTerm ?? DBNull.Value));
            sqlParameters.Add(new SqlParameter("@Username", searchCriteria.Username));
            sqlParameters.Add(new SqlParameter("@PageSize", searchCriteria.PageSize));
            sqlParameters.Add(new SqlParameter("@PageIndex", searchCriteria.PageIndex));

            response.Decks = await _dbContext.DeckDetail.FromSql(DeckSearchByUsernameQuery, sqlParameters.ToArray()).ToListAsync();
            response.TotalRecords = (int)totalRowsCount.Value;

            return response;
        }

        public async Task<MostRecentDecksResult> MostRecentDecks(int pageSize)
        {
            var response = new MostRecentDecksResult();

            const string searchSqlQuery = "MostRecentDecks @PageSize";

            response.Decks = await _dbContext.DeckDetail.FromSql(searchSqlQuery, new SqlParameter("@PageSize", pageSize)).ToListAsync();

            return response;
        }

        public async Task<Deck> Update(Deck deck)
        {
            _dbContext.Deck.Update(deck);

            await _dbContext.SaveChangesAsync();

            return deck;
        }
    }
}