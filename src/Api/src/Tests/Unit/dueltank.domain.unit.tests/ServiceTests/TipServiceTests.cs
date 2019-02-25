using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.tests.core;

namespace dueltank.domain.unit.tests.ServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class TipServiceTests
    {
        private TipService _sut;
        private ITipRepository _tipRepository;

        [SetUp]
        public void SetUp()
        {
            _tipRepository = Substitute.For<ITipRepository>();

            _sut = new TipService(_tipRepository);
        }

        [Test]
        public async Task Should_Invoke_GetTipsByCardId_Once()
        {
            // Arrange
            const int cardId = 90;

            _tipRepository.GetByCardId(Arg.Any<long>()).Returns(new List<TipSection>());

            // Act
            await _sut.GetTipsByCardId(cardId);

            // Assert
            await _tipRepository.Received(1).GetByCardId(Arg.Any<long>());
        }
    }
}