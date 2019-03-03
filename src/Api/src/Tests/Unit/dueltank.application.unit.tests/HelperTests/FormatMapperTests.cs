using System.Collections.Generic;
using dueltank.application.Helpers;
using dueltank.core.Models.Db;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class FormatMapperTests
    {
        [Test]
        public void Given_A_Format_Collection_Should_Map_To_FormatOutputModel_Collection()
        {
            // Arrange
            const int expected = 2;
            var formats = new List<Format>
            {
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
                            BanlistCard = new List<BanlistCard>
                            {
                                new BanlistCard
                                {
                                    BanlistId = 234223,
                                    CardId = 2394,
                                    Limit = new Limit
                                    {
                                        Name = "Unlimited"
                                    }
                                }
                            }
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
                            BanlistCard = new List<BanlistCard>
                            {
                                new BanlistCard
                                {
                                    BanlistId = 234223,
                                    CardId = 2394,
                                    Limit = new Limit
                                    {
                                        Name = "Unlimited"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var results = formats.MapToOutputModels();

            // Assert
            results.Should().HaveCount(expected);
        }
        [Test]
        public void Given_An_Invalid_Format_Collection_Should_Map_To_FormatOutputModel_Collection()
        {
            // Arrange
            var formats = new List<Format>();

            // Act
            var results = formats.MapToOutputModels();

            // Assert
            results.Should().BeEmpty();
        }
    }
}