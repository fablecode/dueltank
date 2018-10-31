using dueltank.application.Enums;
using dueltank.application.Queries.LatestBanlist;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using dueltank.application.Queries.MostRecentBanlists;

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

        /// <summary>
        /// Get the latest banlist by format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{format:alpha:length(1,3)}/latest")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Latest([FromRoute] BanlistFormat format)
        {
            var result = await _mediator.Send(new LatestBanlistQuery { Format = format});

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("latest")]
        public async Task<IActionResult> MostRecentDecks()
        {
            var result = await _mediator.Send(new MostRecentBanlistsQuery());

            return Ok(result);
        }

    }
}