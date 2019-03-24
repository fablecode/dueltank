using System.Collections.Generic;
using AutoMapper;
using dueltank.application.Helpers;
using dueltank.application.Mappings.Profiles;
using dueltank.core.Models.Search.Banlists;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MonsterBanlistCardSearchMapToCardOutputModelTests
    {
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration
            (
                cfg => { cfg.AddProfile(new CardProfile()); }
            );

            _mapper = config.CreateMapper();
        }


        [Test]
        public void Given_A_BanlistCardSearch_Should_Map_To_Monster_CardOutputModel()
        {
            // Arrange
            var cardSearch = new BanlistCardSearch
            {
                CategoryId = 23424,
                Category = "Monster",
                SubCategories = "Normal,Fairy",
            };

            // Act
            var result = MonsterCardHelper.MapToCardOutputModel(_mapper, cardSearch);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void Given_A_BanlistCardSearch_Should_Map_To_Monster_SubCategories_To_Types_()
        {
            // Arrange
            var expected = new List<string> { "Monster", "Normal", "Fairy" };

            var deckCardDetail = new BanlistCardSearch
            {
                CategoryId = 23424,
                Category = "Monster",
                SubCategories = "Normal,Fairy",
            };

            // Act
            var result = MonsterCardHelper.MapToCardOutputModel(_mapper, deckCardDetail);

            // Assert
            result.Types.Should().BeEquivalentTo(expected);
        }
    }
}