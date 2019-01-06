using dueltank.core.Models.Db;
using dueltank.infrastructure.Database;
using dueltank.infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using dueltank.tests.core;

namespace dueltank.infrastructure.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DeckTypeRepositoryTests
    {
        private DeckTypeRepository _sut;
        private DbContextOptions<DueltankDbContext> _dbContextOptions;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DueltankDbContext>()
                .UseInMemoryDatabase("dueltank")
                .Options;

            _sut = new DeckTypeRepository(new DueltankDbContext(_dbContextOptions));
        }

        [Test]
        public async Task Selects_All_DeckTypes()
        {
            // Arrange
            var expected = 2;
            using (var context = new DueltankDbContext(_dbContextOptions))
            {
                context.DeckType.Add(new DeckType
                {
                    Name = "Main",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });
                context.DeckType.Add(new DeckType
                {
                    Name = "Side",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });

                context.SaveChanges();
            }

            // Act
            var result = await _sut.AllDeckTypes();

            // Assert
            result.Count().Should().Be(expected);
        }
    }
}