using dueltank.api.Helpers;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Specialized;

namespace dueltank.api.unit.tests.HelpersTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class UrlHelpersTests
    {
        [Test]
        public void Given_A_Url_And_Parameters_Should_Append_Parameters_To_Url()
        {
            // Arrange
            const string expected = "http://www.dueltank.com/register_confirmation?userId=1231321&token=sdfsf0u8w42029u";

            const string url = "http://www.dueltank.com/register_confirmation?userId=1231321";

            var parameters = new NameValueCollection
            {
                { "token", "sdfsf0u8w42029u"}
            };

            // Act
            var result = UrlHelpers.AppendToReturnUrl(url, parameters);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}