using System.IO;
using System.Net;
using System.Threading.Tasks;
using dueltank.api.Models;
using dueltank.application.Commands.UploadYgoProDeck;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class YgoProDecksController : Controller
    {
        public const string YgoproDeckFileNotSelected = "YgoPro deck file not selected";
        public const string YgoproDeckFileIsEmpty = "YgoPro deck file is empty";

        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public YgoProDecksController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        /// <summary>
        ///     Upload an YgoPro deck
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [ProducesResponseType((int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [RequestSizeLimit(100_000_00)]
        [HttpPost] // 10MB request size
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    var user = await GetCurrentUserAsync();

                    if (user != null)
                    {
                        var command = new UploadYgoProDeckCommand
                        {
                            Name = Path.GetFileNameWithoutExtension(file.FileName),
                            UserId = user.Id
                        };

                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            command.Deck = await reader.ReadToEndAsync();
                        }

                        var result = await _mediator.Send(command);

                        if (result.IsSuccessful)
                            return CreatedAtRoute("GetDeckById", new { id = result.Data }, result.Data);

                        return BadRequest(result.Errors);
                    }


                    return BadRequest();
                }

                return BadRequest(YgoproDeckFileIsEmpty);
            }

            return BadRequest(YgoproDeckFileNotSelected);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User);
        }

    }
}