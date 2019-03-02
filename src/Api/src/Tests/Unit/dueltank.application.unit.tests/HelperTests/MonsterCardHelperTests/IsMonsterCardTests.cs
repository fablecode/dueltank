using dueltank.application.Helpers;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class IsMonsterCardTests
    {
        [Test]
        public void Given_A_Category_That_Is_A_Monster_Card_Should_Return_True()
        {
            // Arrange
            const string category = "Monster";

            // Act
            var result = MonsterCardHelper.IsMonsterCard(category);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Given_Category_That_Is_Not_A_Monster_Card_Should_Return_False()
        {
            // Arrange
            const string category = "FakeCategory";

            // Act
            var result = MonsterCardHelper.IsMonsterCard(category);

            // Assert
            result.Should().BeFalse();
        }
    }
}