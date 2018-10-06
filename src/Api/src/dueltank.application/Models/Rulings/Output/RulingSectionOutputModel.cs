using System.Collections.Generic;

namespace dueltank.application.Models.Rulings.Output
{
    public class RulingSectionOutputModel
    {
        public long CardId { get; set; }
        public string Name { get; set; }

        public List<RulingOutputModel> Rulings { get; set; } = new List<RulingOutputModel>();

    }
}