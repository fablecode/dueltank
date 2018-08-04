using System;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.infrastructure.Database;
using dueltank.infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace dueltank.infrastructure.unit.tests
{
    [TestFixture]
    public class CategoryRepositoryTests
    {
        private CategoryRepository _sut;
        private DbContextOptions<DueltankDbContext> _dbContextOptions;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DueltankDbContext>()
                .UseInMemoryDatabase("dueltank")
                .Options;

            _sut = new CategoryRepository(new DueltankDbContext(_dbContextOptions));
        }

        [Test]
        public async  Task Selects_All_Categories()
        {
            // Arrange
            var expected = 2;
            using (var context = new DueltankDbContext(_dbContextOptions))
            {
                context.Category.Add(new Category
                {
                    Name = "Monster",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });
                context.Category.Add(new Category
                {
                    Name = "Spell",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });

                context.SaveChanges();
            }

            // Act
            var result = await _sut.AllCategories();

            // Assert
            result.Count.Should().Be(expected);
        }
    }
}