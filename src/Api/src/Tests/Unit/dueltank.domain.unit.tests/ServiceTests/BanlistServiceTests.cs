using dueltank.core.Models.Search.Banlists;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using dueltank.tests.core;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace dueltank.domain.unit.tests.ServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BanlistServiceTests
    {
        private BanlistService _sut;
        private IBanlistRepository _banlistRepository;

        [SetUp]
        public void SetUp()
        {
            _banlistRepository = Substitute.For<IBanlistRepository>();

            _sut = new BanlistService(_banlistRepository);
        }

        [Test]
        public async Task Should_Invoke_LatestBanlistByFormatAcronym_Once()
        {
            // Arrange
            const string formatAcronym = "tcg";

            _banlistRepository.LatestBanlistByFormatAcronym(Arg.Any<string>()).Returns(new BanlistCardSearchResult());

            // Act
            await _sut.LatestBanlistByFormatAcronym(formatAcronym);

            // Assert
            await _banlistRepository.Received(1).LatestBanlistByFormatAcronym(formatAcronym);
        }

        [Test]
        public async Task Should_Invoke_MostRecentBanlists_Once()
        {
            // Arrange
            _banlistRepository.MostRecentBanlists().Returns(new MostRecentBanlistResult());

            // Act
            await _sut.MostRecentBanlists();

            // Assert
            await _banlistRepository.Received(1).MostRecentBanlists();
        }
    }
}