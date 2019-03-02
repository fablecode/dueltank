using dueltank.application.Helpers;
using dueltank.core.Models.Search.Banlists;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class IsBanlistCardSearchMonsterCardTests
    {
        [Test]
        public void Given_A_BanlistCardSearch_That_Is_A_Monster_Card_Should_Return_True()
        {
            // Arrange
            var deckCardDetail = new BanlistCardSearch { Category = "Monster"};

            // Act
            var result = MonsterCardHelper.IsMonsterCard(deckCardDetail);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Given_A_BanlistCardSearch_That_Is_Not_A_Monster_Card_Should_Return_False()
        {
            // Arrange
            var deckCardDetail = new BanlistCardSearch { Category = "FakeCategory" };

            // Act
            var result = MonsterCardHelper.IsMonsterCard(deckCardDetail);

            // Assert
            result.Should().BeFalse();
        }
    }
}