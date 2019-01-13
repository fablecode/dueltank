using System;
using dueltank.api.Helpers;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.api.unit.tests.HelpersTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DatetimeHelpersTests
    {
        [Test]
        public void Given_A_DateTime_Should_Convert_It_To_Unix_EpochDate()
        {
            // Arrange
            const string expected = "1353369600";

            const string dateString = "2012-11-20T00:00:00Z";
            var datetime = DateTime.Parse(dateString);

            // Act
            var result = DatetimeHelpers.ToUnixEpochDate(datetime);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}