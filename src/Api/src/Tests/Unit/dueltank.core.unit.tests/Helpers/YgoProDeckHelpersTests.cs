using System.IO;
using System.Reflection;
using dueltank.core.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.core.unit.tests.Helpers
{
    [TestFixture]
    public class YgoProDeckHelpersTests
    {
        [TestCase("7737", "00007737")]
        public void Given_A_CardNumber_Should_Add_Leading_Zeros(string cardNumber, string expected)
        {
            // Arrange

            // Act
            var result = YgoProDeckHelpers.AddLeadingZerosToCardNumber(cardNumber);

            // Assert
            result.Should().BeEquivalentTo(result);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Given_An_Invalid_DeckSection_Should_Return_Empty_StringCollection(string deckSection)
        {
            // Arrange
            var expected = new string[0];

            // Act
            var result = YgoProDeckHelpers.ExtractCardNumbers(deckSection);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestCaseSource(nameof(_deckSections))]
        public void Given_A_DeckSection_Should_Return_CardNumbers(string deckSection, string[] expected)
        {
            // Arrange
            // Act
            var result = YgoProDeckHelpers.ExtractCardNumbers(deckSection);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestCaseSource(nameof(_ygoProMainDecks))]
        public void Given_An_Upload_YgoPro_Deck_Should_Parse_All_CardNumbers_In_MainDeck(string ygoProDeck, int expected)
        {
            // Arrange
            // Act
            var result = YgoProDeckHelpers.MapToYgoProDeck(ygoProDeck);

            // Assert
            result.Main.Should().HaveCount(expected);
        }

        [TestCaseSource(nameof(_ygoProExtraDecks))]
        public void Given_An_Upload_YgoPro_Deck_Should_Parse_All_CardNumbers_In_ExtraDeck(string ygoProDeck, int expected)
        {
            // Arrange
            // Act
            var result = YgoProDeckHelpers.MapToYgoProDeck(ygoProDeck);

            // Assert
            result.Extra.Should().HaveCount(expected);
        }

        [TestCaseSource(nameof(_ygoProSideDecks))]
        public void Given_An_Upload_YgoPro_Deck_Should_Parse_All_CardNumbers_In_SideDeck(string ygoProDeck, int expected)
        {
            // Arrange
            // Act
            var result = YgoProDeckHelpers.MapToYgoProDeck(ygoProDeck);

            // Assert
            result.Side.Should().HaveCount(expected);
        }

        #region TestCaseSources

        private static object[] _deckSections =
        {
            new object[]
            {
                @"33698022
                    58820923
                    39030163
                    31801517",
                new[] { "33698022", "58820923", "39030163", "31801517" }
            }
        };

        private static object[] _ygoProMainDecks =
        {
            new object[]
            {
                GetResourceContents("Fairy Counter Deck.ydk"),
                40
            }
        };

        private static object[] _ygoProExtraDecks =
        {
            new object[]
            {
                GetResourceContents("Fairy Counter Deck.ydk"),
                8
            }
        };

        private static object[] _ygoProSideDecks =
        {
            new object[]
            {
                GetResourceContents("Fairy Counter Deck.ydk"),
                0
            }
        };



        #endregion

        private static string GetResourceContents(string resourceName)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resource = string.Format("dueltank.core.unit.tests.TestData.YgoProDecks.{0}", resourceName);
            using (var stream = asm.GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }
    }
}