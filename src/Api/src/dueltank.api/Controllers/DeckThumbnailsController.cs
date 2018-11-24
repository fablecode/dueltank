using System.IO;
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
    public class DeckThumbnailsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public DeckThumbnailsController(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromForm] UpdateDeckThumbnailInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    var command = new UpdateDeckThumbnailCommand
                    {
                        DeckThumbnail = new DeckThumbnailInputModel
                        {
                            DeckId = inputModel.DeckId,
                            UserId = user.Id,
                        }
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        await inputModel.File.CopyToAsync(memoryStream);
                        command.DeckThumbnail.Thumbnail = memoryStream.ToArray();
                    }

                    var result = await _mediator.Send(command);

                    if (result.IsSuccessful)
                        Ok(result.Data);

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