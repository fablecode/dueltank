using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dueltank.core.Models.YgoPro
{
    public class YgoProDeck
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Main deck")]
        public List<long> Main { get; set; } = new List<long>();

        [Display(Name = "Extra deck")]
        public List<long> Extra { get; set; } = new List<long>();

        [Display(Name = "Side deck")]
        public List<long> Side { get; set; } = new List<long>();
    }
}