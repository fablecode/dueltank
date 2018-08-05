using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dueltank.domain.unit.tests.ServiceTests
{
    [TestFixture]
    public class LimitServiceTests
    {
        private LimitService _sut;
        private ILimitRepository _limitRepository;

        [SetUp]
        public void SetUp()
        {
            _limitRepository = Substitute.For<ILimitRepository>();

            _sut = new LimitService(_limitRepository);
        }

        [Test]
        public async Task Should_Invoke_AllLimits_Once()
        {
            // Arrange
            _limitRepository.AllLimits().Returns(new List<Limit>());

            // Act
            await _sut.AllLimits();

            // Assert
            await _limitRepository.Received(1).AllLimits();

        }
    }
}