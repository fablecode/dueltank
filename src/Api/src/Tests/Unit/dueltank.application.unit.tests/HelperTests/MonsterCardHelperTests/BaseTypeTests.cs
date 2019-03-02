using System.Collections.Generic;
using dueltank.application.Helpers;
using dueltank.core.Models.Db;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BaseTypeTests
    {
        [TestCase("Monster", "monster")]
        [TestCase("Fusion", "fusion")]
        [TestCase("Xyz", "xyz")]
        [TestCase("Synchro", "synchro")]
        [TestCase("Link", "link")]
        public void Given_A_Monster_Card_With_A_SubCategory_Should_Return_BaseType(string subCategory, string expected)
        {
            // Arrange

            var card = new Card
            {
                CardSubCategory = new List<CardSubCategory>
                {
                    new CardSubCategory
                    {
                        SubCategory = new SubCategory
                        {
                            Name = subCategory
                        }
                    }
                }
            };

            // Act
            var result = MonsterCardHelper.BaseType(card);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}