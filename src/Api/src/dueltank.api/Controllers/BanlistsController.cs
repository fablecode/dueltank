using System.Net;
using dueltank.application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class BanlistsController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("{format:alpha:length(1,3)}/latest")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Latest([FromRoute]BanlistFormat format)
        {
            return Ok();
        }
    }
}