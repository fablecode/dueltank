using System;
using System.Collections.Generic;
using System.IO;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Models.Decks;
using Humanizer;

namespace dueltank.application.Models.Decks.Output
{
    public class DeckDetailOutputModel
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string VideoId { get; set; }

        public string YoutubeUrl { get; set; }

        public int TotalCards { get; set; }

        public string CreatedTimeAgo => Created.ToUniversalTime().Humanize();
        public string UpdatedTimeAgo => Updated.ToUniversalTime().Humanize();

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }


        public List<CardDetailOutputModel> MainDeck { get; set; }

        public List<CardDetailOutputModel> ExtraDeck { get; set; }

        public List<CardDetailOutputModel> SideDeck { get; set; }

        public string SanitizedName { get; set; }

        public static DeckDetailOutputModel From(DeckDetail model)
        {
            var deck = new DeckDetailOutputModel
            {
                Id = model.Id,
                Username = model.Username,
                ThumbnailUrl = $"/api/images/decks/{model.Id}/thumbnail",
                Name = model.Name,
                SanitizedName = string.Join("_", model.Name.Split(Path.GetInvalidFileNameChars())),
                Description = model.Description,
                YoutubeUrl = model.VideoUrl,
                TotalCards = model.TotalCards,
                Created = model.Created,
                Updated = model.Updated
            };

            return deck;
        }
    }
}