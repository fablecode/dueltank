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
    public class FormatRepositoryTests
    {
        private FormatRepository _sut;
        private DbContextOptions<DueltankDbContext> _dbContextOptions;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DueltankDbContext>()
                .UseInMemoryDatabase("dueltank")
                .Options;

            _sut = new FormatRepository(new DueltankDbContext(_dbContextOptions));
        }

        [Test]
        public async Task Selects_All_Attributes()
        {
            // Arrange
            var expected = 2;
            using (var context = new DueltankDbContext(_dbContextOptions))
            {
                context.Format.Add(new Format
                {
                    Name = "TCG",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });
                context.Format.Add(new Format
                {
                    Name = "OCG",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                });

                context.SaveChanges();
            }

            // Act
            var result = await _sut.AllFormats();

            // Assert
            result.Count.Should().Be(expected);
        }
    }
}