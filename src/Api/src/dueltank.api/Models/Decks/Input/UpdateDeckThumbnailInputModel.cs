using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace dueltank.api.Models.Decks.Input
{
    public class UpdateDeckThumbnailInputModel
    {
        [Range(0, long.MaxValue)]
        public long DeckId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}