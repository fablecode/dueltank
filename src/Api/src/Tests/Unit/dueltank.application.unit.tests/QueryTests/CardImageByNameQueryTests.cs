using dueltank.application.Queries.CardImageByName;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Configuration;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardImageByNameQueryTests
    {
        private IOptions<ApplicationSettings> _settings;
        private CardImageByNameQueryHandler _sut;
        private IImageService _imageService;
        private IFileService _fileService;

        [SetUp]
        public void SetUp()
        {
            _settings = Substitute.For<IOptions<ApplicationSettings>>();
            _imageService = Substitute.For<IImageService>();
            _fileService = Substitute.For<IFileService>();

            _sut = new CardImageByNameQueryHandler(_settings, _imageService, _fileService);
        }

        [TestCase("Blue-Eyes White Dragon")]
        public async Task Given_A_Card_Name_Should_Return_Image_ContentType(string cardName)
        {
            // Arrange
            const string expected = "image/png";

            _settings.Value.Returns(new ApplicationSettings{ CardImageFolderPath = @"C:Image\Card\Thumbnail"});
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Card\Thumbnail\" + cardName + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            var result = await _sut.Handle(new CardImageByNameQuery { Name = cardName }, CancellationToken.None);

            // Assert
            result.ContentType.Should().BeEquivalentTo(expected);

        }

        [TestCase("Blue-Eyes White Dragon")]
        public async Task Given_A_Card_Name_Should_Return_Image(string cardName)
        {
            // Arrange
            var expected = new byte[] {1, 2, 3};

            _settings.Value.Returns(new ApplicationSettings { CardImageFolderPath = @"C:Image\Card\Thumbnail" });
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Card\Thumbnail\" + cardName + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            var result = await _sut.Handle(new CardImageByNameQuery { Name = cardName }, CancellationToken.None);

            // Assert
            result.Image.Should().BeEquivalentTo(expected);
        }

        [TestCase("Blue-Eyes White Dragon")]
        public async Task Given_A_Card_Name_Should_Return_Invoke_GetImagePath_Once(string cardName)
        {
            // Arrange
            _settings.Value.Returns(new ApplicationSettings { CardImageFolderPath = @"C:Image\Card\Thumbnail" });
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Card\Thumbnail\" + cardName + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            await _sut.Handle(new CardImageByNameQuery { Name = cardName }, CancellationToken.None);

            // Assert
            _imageService.Received(1).GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [TestCase("Blue-Eyes White Dragon")]
        public async Task Given_A_Card_Name_Should_Return_Invoke_ReadAllBytes_Once(string cardName)
        {
            // Arrange
            _settings.Value.Returns(new ApplicationSettings { CardImageFolderPath = @"C:Image\Card\Thumbnail" });
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Card\Thumbnail\" + cardName + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            await _sut.Handle(new CardImageByNameQuery { Name = cardName }, CancellationToken.None);

            // Assert
            _fileService.Received(1).ReadAllBytes(Arg.Any<string>());
        }
    }
}