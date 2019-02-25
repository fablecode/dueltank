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
    public class RulingServiceTests
    {
        private RulingService _sut;
        private IRulingRepository _rulingRepository;

        [SetUp]
        public void SetUp()
        {
            _rulingRepository = Substitute.For<IRulingRepository>();

            _sut = new RulingService(_rulingRepository);
        }

        [Test]
        public async Task Should_Invoke_GetByCardId_Once()
        {
            // Arrange
            const int deckId = 90;

            _rulingRepository.GetByCardId(Arg.Any<long>()).Returns(new List<RulingSection>());

            // Act
            await _sut.GetRulingsByCardId(deckId);

            // Assert
            await _rulingRepository.Received(1).GetByCardId(Arg.Any<long>());
        }
    }
}