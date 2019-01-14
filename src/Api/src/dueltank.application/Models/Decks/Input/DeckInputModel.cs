using dueltank.application.Models.Cards.Input;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dueltank.application.Models.Decks.Input
{
    public class DeckInputModel
    {
        [Display(Name = "Deck Id")]
        public long? Id { get; set; }

        public string UserId { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Deck name")]
        public string Name { get; set; }

        public string Description { get; set; }


        [Display(Name = "Video url")]
        public string VideoUrl { get; set; }

        [Display(Name = "Main deck")]
        public List<CardInputModel> MainDeck { get; set; }

        [Display(Name = "Extra deck")]
        public List<CardInputModel> ExtraDeck { get; set; } = new List<CardInputModel>();

        [Display(Name = "Side deck")]
        public List<CardInputModel> SideDeck { get; set; } = new List<CardInputModel>();
    }
}