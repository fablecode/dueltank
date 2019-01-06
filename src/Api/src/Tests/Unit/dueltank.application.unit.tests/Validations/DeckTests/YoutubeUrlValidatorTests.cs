using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Decks;
using dueltank.tests.core;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class YoutubeUrlValidatorTests
    {
        private YoutubeUrlValidator _sut;
        private DeckInputModel _inputModel;

        [SetUp]
        public void SetUp()
        {
            _sut = new YoutubeUrlValidator();
            _inputModel = new DeckInputModel();
        }

        [TestCase("http://www.youtube.com/sandalsResorts#p/c/0/")]
        [TestCase("http://www.youtube.com/user/Scobleizer#p/u/1/")]
        [TestCase("http://youtu.be/")]
        [TestCase("http://www.youtube.com/embed/")]
        [TestCase("https://www.youtube.com/embed/")]
        [TestCase("http://www.youtube.com/v/?fs=1&hl=en_US")]
        [TestCase("http://www.youtube.com/watch?v=")]
        [TestCase("http://www.youtube.com/user/Scobleizer#p/u/1/")]
        [TestCase("http://www.youtube.com/ytscreeningroom?v=")]
        [TestCase("http://www.youtube.com/user/Scobleizer#p/u/1/")]
        [TestCase("http://www.youtube.com/watch?v=&feature=featured")]
        public void Given_A_YoutubeUrl_When_It_DoesNot_Contain_A_YoutubeVideoId_Validation_Fails(string youtubeVideoUrl)
        {
            // Arrange

            // Act
            _inputModel.VideoUrl = youtubeVideoUrl;


            // Assert
            _sut.ShouldHaveValidationErrorFor(d => d.VideoUrl, _inputModel);
        }


        [TestCase("http://www.youtube.com/sandalsResorts#p/c/54B8C800269D7C1B/0/FJUvudQsKCM")]
        [TestCase("http://www.youtube.com/user/Scobleizer#p/u/1/1p3vcRhsYGo")]
        [TestCase("http://youtu.be/NLqAF9hrVbY")]
        [TestCase("http://www.youtube.com/embed/NLqAF9hrVbY")]
        [TestCase("https://www.youtube.com/embed/NLqAF9hrVbY")]
        [TestCase("http://www.youtube.com/v/NLqAF9hrVbY?fs=1&hl=en_US")]
        [TestCase("http://www.youtube.com/watch?v=NLqAF9hrVbY")]
        [TestCase("http://www.youtube.com/user/Scobleizer#p/u/1/1p3vcRhsYGo")]
        [TestCase("http://www.youtube.com/ytscreeningroom?v=NRHVzbJVx8I")]
        [TestCase("http://www.youtube.com/user/Scobleizer#p/u/1/1p3vcRhsYGo")]
        [TestCase("http://www.youtube.com/watch?v=JYArUl0TzhA&feature=featured")]
        public void Given_A_YoutubeUrl_When_It_Contains_A_YoutubeVideoId_Validation_Pass(string youtubeVideoUrl)
        {
            // Arrange

            // Act
            _inputModel.VideoUrl = youtubeVideoUrl;


            // Assert
            _sut.ShouldNotHaveValidationErrorFor(d => d.VideoUrl, _inputModel);
        }

        [Test]
        public void Given_A_YoutubeUrl_Then_Validation_Should_Only_Execute_When_A_Value_Exists()
        {
            // Arrange
            // Act
            // Assert
            _sut.ShouldNotHaveValidationErrorFor(d => d.VideoUrl, _inputModel);
        }
    }
}