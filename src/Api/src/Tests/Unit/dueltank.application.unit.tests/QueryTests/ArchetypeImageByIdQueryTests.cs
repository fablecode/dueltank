using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Queries.ArchetypeImageById;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeImageByIdQueryTests
    {
        private IOptions<ApplicationSettings> _settings;
        private ArchetypeImageByIdQueryHandler _sut;
        private IImageService _imageService;
        private IFileService _fileService;

        [SetUp]
        public void SetUp()
        {
            _settings = Substitute.For<IOptions<ApplicationSettings>>();
            _imageService = Substitute.For<IImageService>();
            _fileService = Substitute.For<IFileService>();

            _sut = new ArchetypeImageByIdQueryHandler(_settings, _imageService, _fileService);
        }

        [TestCase(2342)]
        public async Task Given_An_ArchetypeId_Should_Return_Image_ContentType(long archetypeId)
        {
            // Arrange
            const string expected = "image/png";

            _settings.Value.Returns(new ApplicationSettings{ ArchetypeImageFolderPath = @"C:Image\Archetype\Thumbnail"});
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Archetype\Thumbnail\" + archetypeId + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            var result = await _sut.Handle(new ArchetypeImageByIdQuery{ ArchetypeId = archetypeId }, CancellationToken.None);

            // Assert
            result.ContentType.Should().BeEquivalentTo(expected);

        }

        [TestCase(2342)]
        public async Task Given_An_ArchetypeId_Should_Return_Image(long archetypeId)
        {
            // Arrange
            var expected = new byte[] {1, 2, 3};

            _settings.Value.Returns(new ApplicationSettings{ ArchetypeImageFolderPath = @"C:Image\Archetype\Thumbnail"});
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Archetype\Thumbnail\" + archetypeId + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            var result = await _sut.Handle(new ArchetypeImageByIdQuery{ ArchetypeId = archetypeId }, CancellationToken.None);

            // Assert
            result.Image.Should().BeEquivalentTo(expected);
        }

        [TestCase(2342)]
        public async Task Given_An_ArchetypeId_Should_Return_Invoke_GetImagePath_Once(long archetypeId)
        {
            // Arrange
            _settings.Value.Returns(new ApplicationSettings{ ArchetypeImageFolderPath = @"C:Image\Archetype\Thumbnail"});
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Archetype\Thumbnail\" + archetypeId + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            await _sut.Handle(new ArchetypeImageByIdQuery{ ArchetypeId = archetypeId }, CancellationToken.None);

            // Assert
            _imageService.Received(1).GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }
        [TestCase(2342)]
        public async Task Given_An_ArchetypeId_Should_Return_Invoke_ReadAllBytes_Once(long archetypeId)
        {
            // Arrange
            _settings.Value.Returns(new ApplicationSettings{ ArchetypeImageFolderPath = @"C:Image\Archetype\Thumbnail"});
            _imageService.GetImagePath(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(@"C:Image\Archetype\Thumbnail\" + archetypeId + ".png");
            _fileService.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] { 1, 2, 3 });

            // Act
            await _sut.Handle(new ArchetypeImageByIdQuery{ ArchetypeId = archetypeId }, CancellationToken.None);

            // Assert
            _fileService.Received(1).ReadAllBytes(Arg.Any<string>());
        }
    }
}