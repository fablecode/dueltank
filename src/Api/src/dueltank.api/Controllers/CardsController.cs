using System.Threading.Tasks;
using dueltank.application.Queries.CardByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly IMediator _mediator;

        public CardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name)
        {
            var result = await _mediator.Send(new CardByNameQuery { Name = name });

            if (result != null)
                return Ok(result);

            return NotFound();
        }
    }
}