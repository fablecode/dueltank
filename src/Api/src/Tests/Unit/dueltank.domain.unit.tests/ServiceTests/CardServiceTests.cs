using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using dueltank.tests.core;

namespace dueltank.domain.unit.tests.ServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardServiceTests
    {
        CardService _sut;
        private ICardRepository _cardRepository;

        [SetUp]
        public void SetUp()
        {
            _cardRepository = Substitute.For<ICardRepository>();

            _sut = new CardService(_cardRepository);
        }

        [Test]
        public async Task Should_Invoke_GetCardByNumber_Once()
        {
            // Arrange
            const int cardNumber = 32525523;
            _cardRepository.GetCardByNumber(Arg.Any<long>()).Returns(new Card());

            // Act
            await _sut.GetCardByNumber(cardNumber);

            // Assert
            await _cardRepository.Received(1).GetCardByNumber(cardNumber);

        }
    }
}