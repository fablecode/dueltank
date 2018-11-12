using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using dueltank.api.Models;
using dueltank.application.Queries.DeckById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using dueltank.application.Commands.CreateDeck;
using dueltank.application.Models.Decks.Input;
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
        /// Add new deck
        /// </summary>
        /// <param name="newDeck"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [RequestSizeLimit(100_000_00)] // 10MB request size
        public async Task<IActionResult> Post([FromBody] DeckInputModel newDeck)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            //var user = await _userManager.FindByNameAsync(username.Id);

            if (user != null)
            {
                newDeck.UserId = user.Id;

                var command = new CreateDeckCommand
                {
                    Deck = newDeck
                };

                var result = await _mediator.Send(command);

                if (result.IsSuccessful)
                    return CreatedAtRoute("GetDeckById", new { id = result.Data }, result.Data);

                return BadRequest(result.Errors);
            }

            return BadRequest();
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

    public class UpdateDeckInputModel
    {
        [Display(Name = "Deck Id")]
        public long? Id { get; set; }

        [Required]
        [Display(Name = "Deck name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Video url")]
        public string VideoUrl { get; set; }

        [Display(Name = "Main deck")]
        public List<long> MainDeck { get; set; }

        [Display(Name = "Extra deck")]
        public List<long> ExtraDeck { get; set; }

        [Display(Name = "Side deck")]
        public List<long> SideDeck { get; set; }
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