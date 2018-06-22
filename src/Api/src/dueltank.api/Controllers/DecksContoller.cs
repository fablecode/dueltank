using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using dueltank.application.Commands.UploadYgoProDeck;
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
                var command = new UploadYgoProDeckCommand
                {
                    Name = Path.GetFileNameWithoutExtension(file.FileName),
                    UserId = Guid.Parse(User.Identity.Name)
                };

                using (var reader = new StreamReader(file.OpenReadStream()))
                    command.Deck = await reader.ReadToEndAsync();

                var result = await _mediator.Send(command);

                if (result.IsSuccessful)
                    return CreatedAtRoute("GetDeckById", new {id = result.Data});

                return BadRequest(result.Errors);
            }

            return BadRequest("YgoPro deck file not selected");
        }
    }
}