﻿using System.IO;
using System.Net;
using System.Threading.Tasks;
using dueltank.api.Models;
using dueltank.api.Models.Decks.Input;
using dueltank.api.ServiceExtensions;
using dueltank.application.Commands.UpdateDeckThumbnail;
using dueltank.application.Models.Decks.Input;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class DeckThumbnailsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeckThumbnailsController(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User);
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Patch([FromForm] UpdateDeckThumbnailInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    var command = new UpdateDeckThumbnailCommand
                    {
                        DeckThumbnail = new DeckThumbnailInputModel
                        {
                            DeckId = inputModel.DeckId,
                            UserId = user.Id
                        }
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        await inputModel.File.CopyToAsync(memoryStream);
                        command.DeckThumbnail.Thumbnail = memoryStream.ToArray();
                    }

                    var result = await _mediator.Send(command);

                    if (result.IsSuccessful)
                        return Ok(new { deckId = result.Data});

                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return BadRequest(ModelState.Errors());
            }

            return BadRequest();
        }
    }
}