using dueltank.core.Models.Db;
using dueltank.infrastructure.Database;
using dueltank.infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace dueltank.infrastructure.unit.tests
{
    [TestFixture]
    public class LimitRepositoryTests
    {
        private LimitRepository _sut;
        private DbContextOptions<DueltankDbContext> _dbContextOptions;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DueltankDbContext>()
                .UseInMemoryDatabase("dueltank")
                .Options;

            _sut = new LimitRepository(new DueltankDbContext(_dbContextOptions));
        }

        [Test]
        public async Task Selects_AllLimits()
        {
            // Arrange
            var expected = 2;
            using (var context = new DueltankDbContext(_dbContextOptions))
            {
                context.Limit.Add(new Limit
                {
                    Name = "Forbidden",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });
                context.Limit.Add(new Limit
                {
                    Name = "Limited",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });

                context.SaveChanges();
            }

            // Act
            var result = await _sut.AllLimits();

            // Assert
            result.Count.Should().Be(expected);
        }
    }
}