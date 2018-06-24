using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dueltank.core.Models.YgoPro
{
    public class YgoProDeck
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Main deck")]
        public List<string> Main { get; set; } = new List<string>();

        [Display(Name = "Extra deck")]
        public List<string> Extra { get; set; } = new List<string>();

        [Display(Name = "Side deck")]
        public List<string> Side { get; set; } = new List<string>();
    }
}