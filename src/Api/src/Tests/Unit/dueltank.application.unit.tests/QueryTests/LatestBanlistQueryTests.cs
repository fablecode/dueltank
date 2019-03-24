using dueltank.application.Queries.LatestBanlist;
using dueltank.core.Models.Search.Banlists;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.application.Mappings.Profiles;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class LatestBanlistQueryTests
    {
        private IBanlistService _banlistService;
        private LatestBanlistQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _banlistService = Substitute.For<IBanlistService>();

            var config = new MapperConfiguration
            (
                cfg => { cfg.AddProfile(new CardProfile()); }
            );

            var mapper = config.CreateMapper();


            _sut = new LatestBanlistQueryHandler(_banlistService, mapper);
        }

        [Test]
        public async Task Given_A_BanlistFormat_If_Format_Is_Not_Found_Format_Should_Be_Null()
        {
            // Arrange
            _banlistService.LatestBanlistByFormatAcronym(Arg.Any<string>()).Returns(new BanlistCardSearchResult{ Cards = new List<BanlistCardSearch>()});

            // Act
            var result = await _sut.Handle(new LatestBanlistQuery(), CancellationToken.None);

            // Assert
            result.Format.Should().BeNull();
        }

        [Test]
        public async Task Given_A_BanlistFormat_If_Format_Is_Found_Should_Return_Forbidden_Cards()
        {
            // Arrange
            _banlistService.LatestBanlistByFormatAcronym(Arg.Any<string>()).Returns(new BanlistCardSearchResult{ Cards = new List<BanlistCardSearch>
            {
                new BanlistCardSearch
                {
                    Id = 4933,
                    CategoryId = 2,
                    Category = "Spell",
                    CardNumber = 83764718,
                    Name = "Monster Reborn",
                    Description = "Target 1 monster in either player's GY; Special Summon it.",
                    SubCategories = "Normal",
                    Limit = "Forbidden"
                }
            }});

            // Act
            var result = await _sut.Handle(new LatestBanlistQuery(), CancellationToken.None);

            // Assert
            result.Forbidden.Should().HaveCount(1);
        }

        [Test]
        public async Task Given_A_BanlistFormat_If_Format_Is_Found_Should_Return_Limited_Cards()
        {
            // Arrange
            _banlistService.LatestBanlistByFormatAcronym(Arg.Any<string>()).Returns(new BanlistCardSearchResult{ Cards = new List<BanlistCardSearch>
            {
                new BanlistCardSearch
                {
                    Id = 4933,
                    CategoryId = 2,
                    Category = "Spell",
                    CardNumber = 83764718,
                    Name = "Monster Reborn",
                    Description = "Target 1 monster in either player's GY; Special Summon it.",
                    SubCategories = "Normal",
                    Limit = "Limited"
                }
            }});

            // Act
            var result = await _sut.Handle(new LatestBanlistQuery(), CancellationToken.None);

            // Assert
            result.Limited.Should().HaveCount(1);
        }

        [Test]
        public async Task Given_A_BanlistFormat_If_Format_Is_Found_Should_Return_SemiLimited_Cards()
        {
            // Arrange
            _banlistService.LatestBanlistByFormatAcronym(Arg.Any<string>()).Returns(new BanlistCardSearchResult{ Cards = new List<BanlistCardSearch>
            {
                new BanlistCardSearch
                {
                    Id = 4933,
                    CategoryId = 2,
                    Category = "Spell",
                    CardNumber = 83764718,
                    Name = "Monster Reborn",
                    Description = "Target 1 monster in either player's GY; Special Summon it.",
                    SubCategories = "Normal",
                    Limit = "Semi-Limited"
                }
            }});

            // Act
            var result = await _sut.Handle(new LatestBanlistQuery(), CancellationToken.None);

            // Assert
            result.SemiLimited.Should().HaveCount(1);
        }

        [Test]
        public async Task Given_A_BanlistFormat_If_Format_Is_Found_Should_Return_Unlimited_Cards()
        {
            // Arrange
            _banlistService.LatestBanlistByFormatAcronym(Arg.Any<string>()).Returns(new BanlistCardSearchResult{ Cards = new List<BanlistCardSearch>
            {
                new BanlistCardSearch
                {
                    Id = 4933,
                    CategoryId = 2,
                    Category = "Spell",
                    CardNumber = 83764718,
                    Name = "Monster Reborn",
                    Description = "Target 1 monster in either player's GY; Special Summon it.",
                    SubCategories = "Normal",
                    Limit = "Unlimited"
                }
            }});

            // Act
            var result = await _sut.Handle(new LatestBanlistQuery(), CancellationToken.None);

            // Assert
            result.Unlimited.Should().HaveCount(1);
        }
    }
}