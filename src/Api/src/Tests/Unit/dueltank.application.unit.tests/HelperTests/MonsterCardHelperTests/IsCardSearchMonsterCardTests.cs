using dueltank.application.Helpers;
using dueltank.core.Models.Cards;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class IsCardSearchMonsterCardTests
    {
        [Test]
        public void Given_A_CardSearch_That_Is_A_Monster_Card_Should_Return_True()
        {
            // Arrange
            var deckCardDetail = new CardSearch { Category = "Monster"};

            // Act
            var result = MonsterCardHelper.IsMonsterCard(deckCardDetail);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Given_A_CardSearch_That_Is_Not_A_Monster_Card_Should_Return_False()
        {
            // Arrange
            var deckCardDetail = new CardSearch { Category = "FakeCategory" };

            // Act
            var result = MonsterCardHelper.IsMonsterCard(deckCardDetail);

            // Assert
            result.Should().BeFalse();
        }
    }
}