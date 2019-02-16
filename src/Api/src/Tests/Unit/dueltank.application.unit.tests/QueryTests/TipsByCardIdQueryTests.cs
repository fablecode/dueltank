using dueltank.application.Queries.TipsByCardId;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class TipsByCardIdQueryTests
    {
        private ITipService _tipService;
        private TipsByCardIdQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _tipService = Substitute.For<ITipService>();
            _sut = new TipsByCardIdQueryHandler(_tipService);
        }

        [Test]
        public async Task Given_A_CardId_If_No_Tips_Are_Found_Should_Return_Default_Tip()
        {
            // Arrange
            const string expected = "No tips for this card yet.....";

            var query = new TipsByCardIdQuery();

            _tipService.GetTipsByCardId(Arg.Any<long>()).Returns(new List<TipSection>());

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1).And.ContainSingle(t => t.Name.Equals(expected, StringComparison.CurrentCultureIgnoreCase));
        }

        [Test]
        public async Task Given_A_CardId_Should_Return_Tips()
        {
            // Arrange
            const string expected = "Blue-Eyes White Dragon";

            var query = new TipsByCardIdQuery();

            _tipService.GetTipsByCardId(Arg.Any<long>()).Returns(new List<TipSection>
            {
                new TipSection
                {
                    CardId = 234242,
                    Name = "Blue-Eyes White Dragon",
                    Tip = new List<Tip>
                    {
                        new Tip
                        {
                            Text = "Effect Activated during damage calculation"
                        }
                    }
                }
            });

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1).And.ContainSingle(t => t.Name.Equals(expected, StringComparison.CurrentCultureIgnoreCase));
        }

        [Test]
        public async Task Given_A_CardId_Should_Invoke_GetTipsByCardId_Once()
        {
            // Arrange
            var query = new TipsByCardIdQuery();

            _tipService.GetTipsByCardId(Arg.Any<long>()).Returns(new List<TipSection>
            {
                new TipSection
                {
                    CardId = 234242,
                    Name = "Blue-Eyes White Dragon",
                    Tip = new List<Tip>
                    {
                        new Tip
                        {
                            Text = "Effect Activated during damage calculation"
                        }
                    }
                }
            });

            // Act
            await _sut.Handle(query, CancellationToken.None);

            // Assert
            await _tipService.Received(1).GetTipsByCardId(Arg.Any<long>());
        }
    }
}