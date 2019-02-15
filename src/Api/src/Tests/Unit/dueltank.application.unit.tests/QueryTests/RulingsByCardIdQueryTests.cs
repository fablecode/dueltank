using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Queries.RulingsByCardId;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class RulingsByCardIdQueryTests
    {
        private IRulingService _rulingService;
        private RulingsByCardIdQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _rulingService = Substitute.For<IRulingService>();
            _sut = new RulingsByCardIdQueryHandler(_rulingService);
        }

        [Test]
        public async Task Given_A_CardId_If_No_Rulings_Are_Found_Should_Return_Default_Ruling()
        {
            // Arrange
            var expected = "No rulings for this card yet.....";

            var query = new RulingsByCardIdQuery();

            _rulingService.GetRulingsByCardId(Arg.Any<long>()).Returns(new List<RulingSection>());

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1).And.ContainSingle(r => r.Name.Equals(expected, StringComparison.CurrentCultureIgnoreCase));
        }

        [Test]
        public async Task Given_A_CardId_Should_Return_Rulings()
        {
            // Arrange
            var expected = "Blue-Eyes White Dragon";

            var query = new RulingsByCardIdQuery();

            _rulingService.GetRulingsByCardId(Arg.Any<long>()).Returns(new List<RulingSection>
            {
                new RulingSection
                {
                    CardId = 234242,
                    Name = "Blue-Eyes White Dragon",
                    Ruling = new List<Ruling>
                    {
                        new Ruling
                        {
                            Text = "Effect Activated during damage calculation"
                        }
                    }
                }
            });

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1).And.ContainSingle(r => r.Name.Equals(expected, StringComparison.CurrentCultureIgnoreCase));
        }

        [Test]
        public async Task Given_A_CardId_Should_Invoke_GetRulingsByCardId_Once()
        {
            // Arrange
            var query = new RulingsByCardIdQuery();

            _rulingService.GetRulingsByCardId(Arg.Any<long>()).Returns(new List<RulingSection>
            {
                new RulingSection
                {
                    CardId = 234242,
                    Name = "Blue-Eyes White Dragon",
                    Ruling = new List<Ruling>
                    {
                        new Ruling
                        {
                            Text = "Effect Activated during damage calculation"
                        }
                    }
                }
            });

            // Act
            await _sut.Handle(query, CancellationToken.None);

            // Assert
            await _rulingService.Received(1).GetRulingsByCardId(Arg.Any<long>());
        }
    }
}