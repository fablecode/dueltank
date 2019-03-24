using System.Net;
using System.Threading.Tasks;
using dueltank.api.Constants;
using dueltank.application.Queries.ArchetypeImageById;
using dueltank.application.Queries.CardImageByName;
using dueltank.application.Queries.DeckThumbnailImagePath;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        ///     Card image by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("cards")]
        [ResponseCache(CacheProfileName = CacheConstants.TwoWeeksPolicy)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Cards([FromQuery]string name)
        {
            var result = await _mediator.Send(new CardImageByNameQuery {Name = name});

            return File(result.Image, result.ContentType);
        }

        /// <summary>
        ///     Deck thumbnail by deck id
        /// </summary>
        /// <param name="deckId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("decks/{deckId}/thumbnail")]
        public async Task<IActionResult> DeckThumbnail(long deckId)
        {
            var result = await _mediator.Send(new DeckThumbnailImagePathQuery {DeckId = deckId});

            return File(result.Image, result.ContentType);
        }

        /// <summary>
        ///     Archetype image by archetype id
        /// </summary>
        /// <param name="archetypeId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("archetypes/{archetypeId}")]
        public async Task<IActionResult> Archetypes(long archetypeId)
        {
            var result = await _mediator.Send(new ArchetypeImageByIdQuery {ArchetypeId = archetypeId});

            return File(result.Image, result.ContentType);
        }
    }
}