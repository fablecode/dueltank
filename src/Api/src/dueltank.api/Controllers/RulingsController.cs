using System.Net;
using System.Threading.Tasks;
using dueltank.application.Queries.RulingsByCardId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/cards/{cardId}/[controller]")]
    public class RulingsController : Controller
    {
        private readonly IMediator _mediator;

        public RulingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute] long cardId)
        {
            var result = await _mediator.Send(new RulingsByCardIdQuery {CardId = cardId});

            return Ok(result);
        }
    }
}