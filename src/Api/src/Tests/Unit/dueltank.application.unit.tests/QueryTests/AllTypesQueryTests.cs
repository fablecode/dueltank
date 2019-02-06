using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Queries.AllTypes;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AllTypesQueryTests
    {
        private ITypeService _typesService;
        private AllTypesQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _typesService = Substitute.For<ITypeService>();

            _sut = new AllTypesQueryHandler(_typesService);
        }

        [Test]
        public async Task Given_An_AllTypes_Query_Should_Return_All_Types()
        {
            // Arrange
            const int expected = 2;

            _typesService.AllTypes().Returns(new List<Type> {new Type(), new Type()});

            // Act
            var result = await _sut.Handle(new AllTypesQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(expected);
        }

        [Test]
        public async Task Given_An_AllTypes_Query_Should_Invoke_AllTypes_Method_Once()
        {
            // Arrange
            const int expected = 1;

            _typesService.AllTypes().Returns(new List<Type> {new Type(), new Type()});

            // Act
            await _sut.Handle(new AllTypesQuery(), CancellationToken.None);

            // Assert
            await _typesService.Received(expected).AllTypes();
        }
    }
}