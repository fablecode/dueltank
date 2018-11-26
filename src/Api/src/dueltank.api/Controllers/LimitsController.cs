using System.Net;
using System.Threading.Tasks;
using dueltank.api.Constants;
using dueltank.application.Queries.AllLimits;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class LimitsController : Controller
    {
        private readonly IMediator _mediator;

        public LimitsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Retrieve all the Limits
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ResponseCache(CacheProfileName = CacheConstants.OneWeekPolicy)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new AllLimitsQuery());

            return Ok(result);
        }
    }
}