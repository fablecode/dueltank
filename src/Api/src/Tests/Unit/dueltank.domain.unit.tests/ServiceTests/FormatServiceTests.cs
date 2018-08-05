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
    public class FormatServiceTests
    {
        private FormatService _sut;
        private IFormatRepository _formatRepository;

        [SetUp]
        public void SetUp()
        {
            _formatRepository = Substitute.For<IFormatRepository>();

            _sut = new FormatService(_formatRepository);
        }

        [Test]
        public async Task Should_Invoke_AllFormats_Once()
        {
            // Arrange
            _formatRepository.AllFormats().Returns(new List<Format>());

            // Act
            await _sut.AllFormats();

            // Assert
            await _formatRepository.Received(1).AllFormats();

        }
    }
}