using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class DecksController : Controller
    {
        private readonly IMediator _mediator;

        public DecksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        [DisableRequestSizeLimit]
        public IActionResult UploadDeck([FromForm]IFormFile file)
        {
            return Ok();
        }

    }
}