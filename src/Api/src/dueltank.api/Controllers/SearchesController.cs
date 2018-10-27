using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dueltank.application.Queries.ArchetypeSearch;
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

        /// <summary>
        /// Search for cards
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("cards")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] CardSearchQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Search for archetypes
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("archetypes")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] ArchetypeSearchQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}