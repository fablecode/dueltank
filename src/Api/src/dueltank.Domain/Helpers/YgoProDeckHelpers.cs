using System;
using System.Linq;
using dueltank.core.Models.YgoPro;

namespace dueltank.core.Helpers
{
    public static class YgoProDeckHelpers
    {
        public static string AddLeadingZerosToCardNumber(string cardNumber)
        {
            return long.Parse(cardNumber).ToString("D8");
        }

        public static string[] ExtractCardNumbers(string deckSection)
        {
            if (string.IsNullOrWhiteSpace(deckSection))
                return new string[0];

            return
                deckSection.Split('\n')
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Select(c => c.Trim(Environment.NewLine.ToCharArray()))
                    .Select(AddLeadingZerosToCardNumber)
                    .ToArray();
        }

        public static YgoProDeck MapToYgoProDeck(string deck)
        {
            var ygoProDeck = new YgoProDeck();

            string[] delimiters = {"#main", "#extra", "!side"};

            var deckSections = deck.Split(delimiters, StringSplitOptions.None).Skip(1).ToList();

            if (deckSections.Count == 3)
            {
                var mainDeckSecion = deckSections[0].Trim();
                var extraDeckSection = deckSections[1].Trim();
                var sideCards = deckSections[2].Trim();

                if (!string.IsNullOrWhiteSpace(mainDeckSecion))
                    ygoProDeck.Main.AddRange(ExtractCardNumbers(mainDeckSecion));

                if (!string.IsNullOrWhiteSpace(extraDeckSection))
                    ygoProDeck.Extra.AddRange(ExtractCardNumbers(extraDeckSection));

                if (!string.IsNullOrWhiteSpace(sideCards))
                    ygoProDeck.Side.AddRange(ExtractCardNumbers(sideCards));
            }

            return ygoProDeck;
        }
    }
}