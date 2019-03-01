using dueltank.Domain.Service;
using dueltank.Domain.SystemIO;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.domain.unit.tests.ServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ImageServiceTests
    {
        private ImageService _sut;
        private IDirectorySystem _directorySystem;

        [SetUp]
        public void SetUp()
        {
            _directorySystem = Substitute.For<IDirectorySystem>();

            _sut = new ImageService(_directorySystem);
        }

        [Test]
        public void Given_An_ImageName_DirectoryPath_And_A_DefaultImageFile_Should_Return_Image_Path()
        {
            // Arrange
            const string expected = @"c:\card\images\jinzo.png";

            _directorySystem.GetFiles(Arg.Any<string>(), Arg.Any<string>()).Returns(new[] {@"c:\card\images\jinzo.png"});
            
            // Act
            var result = _sut.GetImagePath("jinzo", @"c:\card\images", "defaultCardImage.png");

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void Given_An_ImageName_DirectoryPath_And_A_DefaultImageFile_If_Image_Is_Not_Found_Shouldd_Return_DefaultImage_Path()
        {
            // Arrange
            const string expected = @"c:\card\images\defaultCardImage.png";

            _directorySystem.GetFiles(Arg.Any<string>(), Arg.Any<string>()).Returns(new string[0]);
            
            // Act
            var result = _sut.GetImagePath("jinzo", @"c:\card\images", "defaultCardImage.png");

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}