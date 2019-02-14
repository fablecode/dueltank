using dueltank.application.Queries.MostRecentArchetypes;
using dueltank.core.Models.Archetypes;
using dueltank.core.Services;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.tests.core;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MostRecentArchetypesQueryTests
    {
        private IArchetypeService _archetypeService;
        private MostRecentArchetypesQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _archetypeService = Substitute.For<IArchetypeService>();

            _sut = new MostRecentArchetypesQueryHandler(_archetypeService);
        }

        [Test]
        public async Task Given_A_PageSize_Should_Return_Most_Recent_Archetypes()
        {
            // Arrange
            var query = new MostRecentArchetypesQuery();

            _archetypeService.MostRecentArchetypes(Arg.Any<int>()).Returns(new MostRecentArchetypesResult{ Archetypes = new List<ArchetypeSearch>()});

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Archetypes.Should().BeNull();
        }

        [Test]
        public async Task Given_A_PageSize_Should_Invoke_MostRecentArchetypes_Once()
        {
            // Arrange
            var query = new MostRecentArchetypesQuery();

            _archetypeService.MostRecentArchetypes(Arg.Any<int>()).Returns(new MostRecentArchetypesResult
            {
                Archetypes = new List<ArchetypeSearch>
                {
                    new ArchetypeSearch()
                }
            });

            // Act
            await _sut.Handle(query, CancellationToken.None);

            // Assert
            await _archetypeService.Received(1).MostRecentArchetypes(Arg.Any<int>());
        }
    }
}