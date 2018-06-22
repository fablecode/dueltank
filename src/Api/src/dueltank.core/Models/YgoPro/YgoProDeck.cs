using System;
using System.Collections.Generic;

namespace dueltank.core.Models.YgoPro
{
    public class YgoProDeck
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Main { get; set; } = new List<string>();
        public List<string> Extra { get; set; } = new List<string>();
        public List<string> Side { get; set; } = new List<string>();
    }
}