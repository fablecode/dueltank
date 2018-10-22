using dueltank.application.Enums;
using dueltank.application.Queries.LatestBanlist;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class BanlistsController : Controller
    {
        private readonly IMediator _mediator;

        public BanlistsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("{format:alpha:length(1,3)}/latest")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Latest([FromRoute] BanlistFormat format)
        {
            var result = await _mediator.Send(new LatestBanlistQuery { Format = format});

            return Ok(result);
        }
    }
}