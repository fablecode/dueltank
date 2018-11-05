using dueltank.api.Models;
using dueltank.application.Queries.DeckById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using dueltank.application.Queries.MostRecentDecks;


namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class DecksController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public DecksController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        /// <summary>
        /// Get a deck by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}", Name = "GetDeckById")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _mediator.Send(new DeckByIdQuery {DeckId = id});

            if (result != null)
                return Ok(result);

            return NotFound(id);
        }


        /// <summary>
        /// Add a new deck
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [RequestSizeLimit(100_000_00)] // 10MB request size
        public IActionResult Post([FromBody] AddDeckInputModel input)
        {
            var deckName = input.Info.Name;

            return Ok();
        }



        /// <summary>
        /// Retrieve the most recent decks
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [Route("latest")]
        public async Task<IActionResult> MostRecentDecks([FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new MostRecentDecksQuery { PageSize = pageSize });

            return Ok(result);
        }
    }

    public class AddDeckInputModel
    {
        public DeckInfoInputModel Info { get; set; }

        public DeckInputModel Deck { get; set; }
    }

    public class DeckInputModel
    {
        public string Username { get; set; }
        public long Id { get; set; }

        public CardInputModel[] Main { get; set; }
    }

    public class CardInputModel
    {
        public long Id { get; set; }
    }

    public class DeckInfoInputModel
    {
        public IFormFile Thumbnail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
    }
}