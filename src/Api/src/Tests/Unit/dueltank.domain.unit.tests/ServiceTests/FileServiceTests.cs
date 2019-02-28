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
    public class FileServiceTests
    {
        private IFileSystem _fileSystem;
        private FileService _sut;

        [SetUp]
        public void SetUp()
        {
            _fileSystem = Substitute.For<IFileSystem>();

            _sut = new FileService(_fileSystem);
        }

        [Test]
        public void Given_A_Valid_Path_Should_ReadAllBytes()
        {
            // Arrange
            var expected = new byte[] {1, 2, 3};

            const string path = @"C:\textfile.txt";

            _fileSystem.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] {1, 2, 3});

            // Act
            var result = _sut.ReadAllBytes(path);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_A_Valid_Path_Should_Invoke_ReadAllBytes_Once()
        {
            // Arrange
            const int expected = 1;

            const string path = @"C:\textfile.txt";

            _fileSystem.ReadAllBytes(Arg.Any<string>()).Returns(new byte[] {1, 2, 3});

            // Act
            _sut.ReadAllBytes(path);

            // Assert
            _fileSystem.Received(expected).ReadAllBytes(Arg.Any<string>());
        }
    }
}