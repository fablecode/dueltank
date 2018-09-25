using System.Net;
using System.Threading.Tasks;
using dueltank.application.Queries.CardSearches;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class SearchesController : Controller
    {
        private readonly IMediator _mediator;

        public SearchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("cards")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] CardSearchQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}