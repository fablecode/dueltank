using dueltank.api.Models;
using dueltank.application.Commands.UploadYgoProDeck;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;


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

        [HttpGet("{id}", Name = "GetDeckById")]
        public IActionResult Get(long id)
        {
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [RequestSizeLimit(100_000_00)] // 10MB request size
        public async Task<IActionResult> UploadDeck([FromForm]IFormFile file)
        {
            if (file != null && file.Length >= 0)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var command = new UploadYgoProDeckCommand
                    {
                        Name = Path.GetFileNameWithoutExtension(file.FileName),
                        UserId = user.Id
                    };

                    using (var reader = new StreamReader(file.OpenReadStream()))
                        command.Deck = await reader.ReadToEndAsync();

                    var result = await _mediator.Send(command);

                    if (result.IsSuccessful)
                        return CreatedAtRoute("GetDeckById", new { id = result.Data }, result.Data);
                }


                return BadRequest();
            }

            return BadRequest("YgoPro deck file not selected");
        }
    }
}