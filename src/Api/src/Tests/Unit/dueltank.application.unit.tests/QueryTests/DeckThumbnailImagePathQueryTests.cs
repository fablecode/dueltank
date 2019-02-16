using dueltank.application.Queries.DeckThumbnailImagePath;
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
    public class DeckThumbnailImagePathQueryTests
    {
        private IOptions<ApplicationSettings> _settings;
        private DeckThumbnailImagePathQueryHandler _sut;
        private IImageService _imageService;
        private IFileService _fileService;

        [SetUp]
        public void SetUp()
        {
            _settings = Substitute.For<IOptions<ApplicationSettings>>();
            _imageService = Substitute.For<IImageService>();
            _fileService = Substitute.For<IFileService>();

            _sut = new DeckThumbnailImagePathQueryHandler(_settings, _imageService, _fileService);
        }

        [TestCase(123131)]
        public async Task Given_A_DeckId_Should_Return_Image_ContentType(long deckId)
        {
            // Arrange
            const string expected = "image/png";

            _settings.Value.Returns(new ApplicationSettings{ DeckThumbnailImageFolderPath = @"C:Image\Deck\Thumbnail" });
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Deck\Thumbnail\" + deckId + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            var result = await _sut.Handle(new DeckThumbnailImagePathQuery { DeckId = deckId }, CancellationToken.None);

            // Assert
            result.ContentType.Should().BeEquivalentTo(expected);

        }

        [TestCase(123131)]
        public async Task Given_A_DeckId_Should_Return_Image(long deckId)
        {
            // Arrange
            var expected = new byte[] {1, 2, 3};

            _settings.Value.Returns(new ApplicationSettings { DeckThumbnailImageFolderPath = @"C:Image\Deck\Thumbnail" });
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Deck\Thumbnail\" + deckId + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            var result = await _sut.Handle(new DeckThumbnailImagePathQuery { DeckId = deckId }, CancellationToken.None);

            // Assert
            result.Image.Should().BeEquivalentTo(expected);
        }

        [TestCase(123131)]
        public async Task Given_A_DeckId_Should_Return_Invoke_GetImagePath_Once(long deckId)
        {
            // Arrange
            _settings.Value.Returns(new ApplicationSettings { DeckThumbnailImageFolderPath = @"C:Image\Deck\Thumbnail" });
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Deck\Thumbnail\" + deckId + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            await _sut.Handle(new DeckThumbnailImagePathQuery { DeckId = deckId }, CancellationToken.None);

            // Assert
            _imageService.Received(1).GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [TestCase(123131)]
        public async Task Given_A_DeckId_Should_Return_Invoke_ReadAllBytes_Once(long deckId)
        {
            // Arrange
            _settings.Value.Returns(new ApplicationSettings { DeckThumbnailImageFolderPath = @"C:Image\Deck\Thumbnail" });
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Deck\Thumbnail\" + deckId + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            await _sut.Handle(new DeckThumbnailImagePathQuery { DeckId = deckId }, CancellationToken.None);

            // Assert
            _fileService.Received(1).ReadAllBytes(Arg.Any<string>());
        }
    }
}