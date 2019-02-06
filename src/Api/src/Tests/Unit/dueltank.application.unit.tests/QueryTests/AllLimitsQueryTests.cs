using dueltank.application.Queries.AllLimits;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AllLimitsQueryTests
    {
        private ILimitService _limitService;
        private AllLimitsQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _limitService = Substitute.For<ILimitService>();

            _sut = new AllLimitsQueryHandler(_limitService);
        }

        [Test]
        public async Task Given_An_AllLimits_Query_Should_Return_All_Limits()
        {
            // Arrange
            const int expected = 2;

            _limitService.AllLimits().Returns(new List<Limit> {new Limit(), new Limit()});

            // Act
            var result = await _sut.Handle(new AllLimitsQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(expected);
        }

        [Test]
        public async Task Given_An_AllLimits_Query_Should_Invoke_AllLimits_Method_Once()
        {
            // Arrange
            const int expected = 1;

            _limitService.AllLimits().Returns(new List<Limit> {new Limit(), new Limit()});

            // Act
            await _sut.Handle(new AllLimitsQuery(), CancellationToken.None);

            // Assert
            await _limitService.Received(expected).AllLimits();
        }
    }
}