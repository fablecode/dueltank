using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
    }

    [Route("api/cards/{cardId}/[controller]")]
    public class TipsController : Controller
    {
        private readonly IMediator _mediator;

        public TipsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] long cardId)
        {
            var result = await _mediator.Send(new TipsByCardIdQuery {CardId = cardId});

            return Ok(result);
        }
    }

    public class TipsByCardIdQuery : IRequest<IEnumerable<TipOutputModel>>
    {
        public long CardId { get; set; }
    }

    public class TipOutputModel
    {
    }
}