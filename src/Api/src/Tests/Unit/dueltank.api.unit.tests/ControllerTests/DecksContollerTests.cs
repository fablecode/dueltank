using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.tests.core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.api.unit.tests.ControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DecksContollerTests
    {
        private IMediator _mediator;
        private DecksController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            //_sut = new DecksController(_mediator, new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(),));
        }
    }
}