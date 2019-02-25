using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using dueltank.tests.core;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace dueltank.domain.unit.tests.ServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class UserServiceTests
    {
        private UserService _sut;
        private IUserRepository _userRepository;

        [SetUp]
        public void SetUp()
        {
            _userRepository = Substitute.For<IUserRepository>();

            _sut = new UserService(_userRepository);
        }

        [Test]
        public async Task Should_Invoke_GetTipsByCardId_Once()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            const long deckId = 90;

            _userRepository.IsUserDeckOwner(Arg.Any<string>(), Arg.Any<long>()).Returns(true);

            // Act
            await _sut.IsUserDeckOwner(userId, deckId);

            // Assert
            await _userRepository.Received(1).IsUserDeckOwner(Arg.Any<string>(), Arg.Any<long>());
        }
    }
}