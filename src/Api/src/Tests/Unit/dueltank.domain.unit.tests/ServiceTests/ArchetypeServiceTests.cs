using System.Threading.Tasks;
using dueltank.core.Models.Archetypes;
using dueltank.core.Models.Search.Archetypes;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using dueltank.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.domain.unit.tests.ServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeServiceTests
    {
        private ArchetypeService _sut;
        private IArchetypeRepository _archetypeRepository;

        [SetUp]
        public void SetUp()
        {
            _archetypeRepository = Substitute.For<IArchetypeRepository>();

            _sut = new ArchetypeService(_archetypeRepository);
        }

        [Test]
        public async Task Should_Invoke_Search_Method_Once()
        {
            // Arrange
            var archetypeSearchCriteria = new ArchetypeSearchCriteria();
            _archetypeRepository.Search(Arg.Any<ArchetypeSearchCriteria>()).Returns(new ArchetypeSearchResult());

            // Act
            await _sut.Search(archetypeSearchCriteria);

            // Assert
            await _archetypeRepository.Received(1).Search(Arg.Any<ArchetypeSearchCriteria>());
        }

        [Test]
        public async Task Should_Invoke_ArchetypeById_Method_Once()
        {
            // Arrange
            const int archetypeId = 2342;
            _archetypeRepository.ArchetypeById(Arg.Any<long>()).Returns(new ArchetypeByIdResult());

            // Act
            await _sut.ArchetypeById(archetypeId);

            // Assert
            await _archetypeRepository.Received(1).ArchetypeById(Arg.Any<long>());
        }

        [Test]
        public async Task Should_Invoke_MostRecentArchetypes_Method_Once()
        {
            // Arrange
            const int pageSize = 10;
            _archetypeRepository.MostRecentArchetypes(Arg.Any<int>()).Returns(new MostRecentArchetypesResult());

            // Act
            await _sut.MostRecentArchetypes(pageSize);

            // Assert
            await _archetypeRepository.Received(1).MostRecentArchetypes(Arg.Any<int>());
        }
    }
}