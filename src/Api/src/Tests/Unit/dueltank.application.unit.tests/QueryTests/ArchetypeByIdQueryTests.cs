using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.application.Mappings.Profiles;
using dueltank.application.Queries.ArchetypeById;
using dueltank.core.Models.Archetypes;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeByIdQueryTests
    {
        private IArchetypeService _archetypeService;
        private ArchetypeByIdQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _archetypeService = Substitute.For<IArchetypeService>();


            var config = new MapperConfiguration
            (
                cfg => { cfg.AddProfile(new CardProfile()); }
            );

            var mapper = config.CreateMapper();


            _sut = new ArchetypeByIdQueryHandler(_archetypeService, mapper);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task Given_An_Invalid_ArchetypeId_Should_Return_Null(long archetypeId)
        {
            // Arrange
            // Act
            var result = await _sut.Handle(new ArchetypeByIdQuery {ArchetypeId = archetypeId}, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [TestCase(23423)]
        public async Task Given_An_Valid_ArchetypeId_Should_Return_Archetype(long archetypeId)
        {
            // Arrange
            const int expected = 23423;

            _archetypeService.ArchetypeById(Arg.Any<long>()).Returns(new ArchetypeByIdResult
            {
                Archetype = new Archetype
                {
                    Id = 23423,
                    Name = "Blue Eyes Deck",
                    Updated = DateTime.UtcNow
                },
                Cards = new List<CardSearch>()
            });

            // Act
            var result = await _sut.Handle(new ArchetypeByIdQuery {ArchetypeId = archetypeId}, CancellationToken.None);

            // Assert
            result.Id.Should().Be(expected);
        }
    }
}