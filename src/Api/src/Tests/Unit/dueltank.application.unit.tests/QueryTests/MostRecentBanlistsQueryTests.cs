using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Queries.MostRecentBanlists;
using dueltank.core.Models.Search.Banlists;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MostRecentBanlistsQueryTests
    {
        private IBanlistService _banlistService;
        private MostRecentBanlistsQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _banlistService = Substitute.For<IBanlistService>();

            _sut = new MostRecentBanlistsQueryHandler(_banlistService);
        }

        [Test]
        public async Task Given_MostRecent_Banlist_Query_If_No_Banlists_Are_Found_Banlist_Collection_Should_Empty()
        {
            // Arrange
            var query = new MostRecentBanlistsQuery();

            _banlistService.MostRecentBanlists().Returns(new MostRecentBanlistResult {Banlists = new List<MostRecentBanlist>()});

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Banlists.Should().BeNull();
        }

        [Test]
        public async Task Given_MostRecent_Banlist_Query_Should_Return_Banlists()
        {
            // Arrange
            const int expected = 2;

            var query = new MostRecentBanlistsQuery();

            _banlistService.MostRecentBanlists().Returns(new MostRecentBanlistResult
            {
                Banlists = new List<MostRecentBanlist>
                {
                    new MostRecentBanlist(),
                    new MostRecentBanlist()
                }
            });

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Banlists.Should().HaveCount(expected);
        }

        [Test]
        public async Task Given_MostRecent_Banlist_Query_Should_Invoke_MostRecentBanlists_Once()
        {
            // Arrange
            var query = new MostRecentBanlistsQuery();

            _banlistService.MostRecentBanlists().Returns(new MostRecentBanlistResult
            {
                Banlists = new List<MostRecentBanlist>
                {
                    new MostRecentBanlist(),
                    new MostRecentBanlist()
                }
            });

            // Act
            await _sut.Handle(query, CancellationToken.None);

            // Assert
            await _banlistService.Received(1).MostRecentBanlists();
        }
    }
}