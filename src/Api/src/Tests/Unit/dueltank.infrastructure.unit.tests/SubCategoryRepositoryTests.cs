using dueltank.core.Models.Db;
using dueltank.infrastructure.Database;
using dueltank.infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dueltank.infrastructure.unit.tests
{
    [TestFixture]
    public class SubCategoryRepositoryTests
    {
        private SubCategoryRepository _sut;
        private DbContextOptions<DueltankDbContext> _dbContextOptions;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DueltankDbContext>()
                .UseInMemoryDatabase("dueltank")
                .Options;

            _sut = new SubCategoryRepository(new DueltankDbContext(_dbContextOptions));
        }

        [Test]
        public async Task Selects_All_SubCategories()
        {
            // Arrange
            var expected = 2;
            using (var context = new DueltankDbContext(_dbContextOptions))
            {
                context.Category.Add(new Category
                {
                    Name = "Monster",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    SubCategory = new List<SubCategory>
                    {
                        new SubCategory
                        {
                            Name = "SpellCaster",
                            Created = DateTime.UtcNow,
                            Updated = DateTime.UtcNow
                        },
                        new SubCategory
                        {
                            Name = "Insect",
                            Created = DateTime.UtcNow,
                            Updated = DateTime.UtcNow
                        }
                    }
                });

                context.SaveChanges();
            }

            // Act
            var result = await _sut.AllSubCategories();

            // Assert
            result.Count.Should().Be(expected);
        }
    }
}