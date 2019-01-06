using dueltank.infrastructure.Database;
using dueltank.infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using dueltank.tests.core;
using Type = dueltank.core.Models.Db.Type;

namespace dueltank.infrastructure.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class TypeRepositoryTests
    {
        private TypeRepository _sut;
        private DbContextOptions<DueltankDbContext> _dbContextOptions;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DueltankDbContext>()
                .UseInMemoryDatabase("dueltank")
                .Options;

            _sut = new TypeRepository(new DueltankDbContext(_dbContextOptions));
        }

        [Test]
        public async Task Selects_All_Types()
        {
            // Arrange
            var expected = 2;
            using (var context = new DueltankDbContext(_dbContextOptions))
            {
                context.Type.Add(new Type
                {
                    Name = "Earth",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });
                context.Type.Add(new Type
                {
                    Name = "Water",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });

                context.SaveChanges();
            }

            // Act
            var result = await _sut.AllTypes();

            // Assert
            result.Count.Should().Be(expected);
        }
    }
}