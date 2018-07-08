using dueltank.application.Queries.DeckThumbnailImagePath;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("cards/{name}")]
        public Task<IActionResult> Cards(string name)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet("decks/{deckId}/thumbnail")]
        public async Task<IActionResult> DeckThumbnail(long deckId)
        {
            var result = await _mediator.Send(new DeckThumbnailImagePathQuery { DeckId = deckId});

            return File(result.Image, result.ContentType);
        }
    }
}