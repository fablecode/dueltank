using dueltank.application.Queries.AllCategories;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Queries.AllFormats;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AllFormatsQueryTests
    {
        private IFormatService _formatService;
        private AllFormatsQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _formatService = Substitute.For<IFormatService>();

            _sut = new AllFormatsQueryHandler(_formatService);
        }

        [Test]
        public async Task Given_An_AllFormats_Query_Should_Return_All_Formats()
        {
            // Arrange
            const int expected = 2;

            _formatService.AllFormats().Returns(new List<Format> {
                new Format
                {
                    Id = 2342,
                    Name = "Test Format",
                    Acronym = "TF",
                    Banlist = new List<Banlist>
                    {
                        new Banlist
                        {
                            Id = 8987,
                            FormatId = 709808,
                            BanlistCard = new List<BanlistCard>{ new BanlistCard
                            {
                                BanlistId = 234223,
                                CardId = 2394,
                                Limit = new Limit
                                {
                                    Name = "Unlimited"
                                }
                            } }
                        }
                    }
                },
                new Format
                {
                    Id = 23424,
                    Name = "Test Format 2",
                    Acronym = "TF",
                    Banlist = new List<Banlist>
                    {
                        new Banlist
                        {
                            Id = 8987,
                            FormatId = 709808,
                            BanlistCard = new List<BanlistCard>{ new BanlistCard
                            {
                                BanlistId = 234223,
                                CardId = 2394,
                                Limit = new Limit
                                {
                                    Name = "Unlimited"
                                }
                            } }
                        }
                    }
                }
            });

            // Act
            var result = await _sut.Handle(new AllFormatsQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(expected);
        }

        [Test]
        public async Task Given_An_AllFormats_Query_Should_Invoke_AllFormats_Method_Once()
        {
            // Arrange
            const int expected = 1;

            _formatService.AllFormats().Returns(new List<Format> {
                new Format
                {
                    Id = 2342,
                    Name = "Test Format",
                    Acronym = "TF",
                    Banlist = new List<Banlist>()
                },
                new Format
                {
                    Id = 23424,
                    Name = "Test Format 2",
                    Acronym = "TF",
                    Banlist = new List<Banlist>()
                }
            });

            // Act
            await _sut.Handle(new AllFormatsQuery(), CancellationToken.None);

            // Assert
            await _formatService.Received(expected).AllFormats();
        }
    }
}