using dueltank.core.Models.Db;
using dueltank.infrastructure.Database;
using dueltank.infrastructure.Repository;
using dueltank.tests.core;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace dueltank.infrastructure.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardRepositoryTests
    {
        private CardRepository _sut;
        private DbContextOptions<DueltankDbContext> _dbContextOptions;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DueltankDbContext>()
                .UseInMemoryDatabase("dueltank")
                .Options;

            _sut = new CardRepository(new DueltankDbContext(_dbContextOptions));
        }

        [Test]
        public async Task Selects_All_Cards()
        {
            // Arrange
            const int expected = 77585513;
            using (var context = new DueltankDbContext(_dbContextOptions))
            {
                context.Card.Add(new Card
                {
                    Name = "Jinzo",
                    CardNumber = 77585513,
                    Description = "Trap Cards, and their effects on the field, cannot be activated. Negate all Trap effects on the field.",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });

                context.SaveChanges();
            }

            // Act
            var result = await _sut.GetCardByNumber(77585513);

            // Assert
            result.CardNumber.Should().Be(expected);
        }
    }
}