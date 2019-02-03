using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Queries.AllAttributes;
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
    public class AllAttributesQueryTests
    {
        private IAttributeService _attributeService;
        private AllAttributesQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _attributeService = Substitute.For<IAttributeService>();

            _sut = new AllAttributesQueryHandler(_attributeService);
        }

        [Test]
        public async Task Given_An_AllAttributes_Query_Should_Return_All_Attributes()
        {
            // Arrange
            const int expected = 2;

            _attributeService.AllAttributes().Returns(new List<Attribute> {new Attribute(), new Attribute()});

            // Act
            var result = await _sut.Handle(new AllAttributesQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(expected);
        }

        [Test]
        public async Task Given_An_AllAttributes_Query_Should_Invoke_AllAttributes_Method_Once()
        {
            // Arrange
            const int expected = 1;

            _attributeService.AllAttributes().Returns(new List<Attribute> {new Attribute(), new Attribute()});

            // Act
            await _sut.Handle(new AllAttributesQuery(), CancellationToken.None);

            // Assert
            await _attributeService.Received(expected).AllAttributes();
        }
    }
}