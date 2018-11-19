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
        private const string DeckSearchWithoutSearchTermQuery = "EXEC DeckSearchWithoutSearchTerm @PageSize, @PageIndex, @TotalRowsCount out";
        private const string DeckSearchWithSearchTermQuery = "EXEC DeckSearchWithSearchTerm @SearchTerm, @PageSize, @PageIndex, @TotalRowsCount out";

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

            string query;
            var sqlParameters = new List<object>();

            if (string.IsNullOrWhiteSpace(searchCriteria.SearchTerm))
            {
                query = DeckSearchWithoutSearchTermQuery;
            }
            else
            {
                query = DeckSearchWithSearchTermQuery;
                sqlParameters.Add(new SqlParameter("@SearchTerm", searchCriteria.SearchTerm));
            }

            var totalRowsCount = new SqlParameter
            {
                ParameterName = "TotalRowsCount",
                Value = 0,
                Direction = ParameterDirection.Output
            };

            sqlParameters.Add(totalRowsCount);

            sqlParameters.Add(new SqlParameter("PageSize", searchCriteria.PageSize));
            sqlParameters.Add(new SqlParameter("@PageIndex", searchCriteria.PageIndex));

            response.Decks = await _dbContext.DeckDetail.FromSql(query, sqlParameters.ToArray()).ToListAsync();
            response.TotalRecords = (int)totalRowsCount.Value;

            return response;
        }

        public Task<DeckSearchResult> Search(string userId, DeckSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
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