using System;
using System.Collections.Generic;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;

namespace dueltank.core.Models.Decks
{
    public class DeckDetail
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string VideoUrl { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public int TotalCards => MainDeck.Count;

        public List<CardDetail> MainDeck { get; set; } = new List<CardDetail>();

        public List<CardDetail> ExtraDeck { get; set; } = new List<CardDetail>();

        public List<CardDetail> SideDeck { get; set; } = new List<CardDetail>();

        public static DeckDetail From(Deck entity)
        {
            return new DeckDetail
            {
                Username = entity.User.UserName,
                UserId = entity.User.Id,
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                VideoUrl = entity.VideoUrl,
                Created = entity.Created,
                Updated = entity.Updated
            };
        }
    }
}