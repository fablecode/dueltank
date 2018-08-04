using System;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.infrastructure.Database;
using dueltank.infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Attribute = dueltank.core.Models.Db.Attribute;

namespace dueltank.infrastructure.unit.tests
{
    [TestFixture]
    public class AttributeRepositoryTests
    {
        private AttributeRepository _sut;
        private DbContextOptions<DueltankDbContext> _dbContextOptions;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DueltankDbContext>()
                .UseInMemoryDatabase("dueltank")
                .Options;

            _sut = new AttributeRepository(new DueltankDbContext(_dbContextOptions));
        }

        [Test]
        public async Task Selects_All_Attributes()
        {
            // Arrange
            var expected = 2;
            using (var context = new DueltankDbContext(_dbContextOptions))
            {
                context.Attribute.Add(new Attribute
                {
                    Name = "Earth",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });
                context.Attribute.Add(new Attribute
                {
                    Name = "Water",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });

                context.SaveChanges();
            }

            // Act
            var result = await _sut.AllAttributes();

            // Assert
            result.Count.Should().Be(expected);
        }
    }
}