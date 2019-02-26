using dueltank.Domain.Helpers;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.domain.unit.tests.Helpers
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class YoutubeHelpersTests
    {
        [TestCase("http://www.youtube.com/sandalsResorts#p/c/54B8C800269D7C1B/0/FJUvudQsKCM", "FJUvudQsKCM")]
        [TestCase("http://www.youtube.com/user/Scobleizer#p/u/1/1p3vcRhsYGo", "1p3vcRhsYGo")]
        [TestCase("http://youtu.be/NLqAF9hrVbY", "NLqAF9hrVbY")]
        [TestCase("http://www.youtube.com/embed/NLqAF9hrVbY", "NLqAF9hrVbY")]
        [TestCase("https://www.youtube.com/embed/NLqAF9hrVbY", "NLqAF9hrVbY")]
        [TestCase("http://www.youtube.com/v/NLqAF9hrVbY?fs=1&hl=en_US", "NLqAF9hrVbY")]
        [TestCase("http://www.youtube.com/watch?v=NLqAF9hrVbY", "NLqAF9hrVbY")]
        [TestCase("http://www.youtube.com/user/Scobleizer#p/u/1/1p3vcRhsYGo", "1p3vcRhsYGo")]
        [TestCase("http://www.youtube.com/ytscreeningroom?v=NRHVzbJVx8I", "NRHVzbJVx8I")]
        [TestCase("http://www.youtube.com/user/Scobleizer#p/u/1/1p3vcRhsYGo", "1p3vcRhsYGo")]
        [TestCase("http://www.youtube.com/watch?v=JYArUl0TzhA&feature=featured", "JYArUl0TzhA")]
        public void Given_A_YoutubeUrl_Extract_VideoId_(string youtubeVideoUrl, string expectVideoId)
        {
            // Arrange

            // Act
            var result = YoutubeHelpers.ExtractVideoId(youtubeVideoUrl);

            // Assert
            result.Should().BeEquivalentTo(expectVideoId);
        }
    }
}