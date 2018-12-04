using System.Threading.Tasks;
using dueltank.api.Models.Attributes;
using dueltank.api.Models.ContactUs.Input;
using dueltank.api.ServiceExtensions;
using dueltank.application.Commands.SendContactUsEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class ContactMessages : Controller
    {
        private readonly IMediator _mediator;

        public ContactMessages(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [Throttle(Name = "ContactSupport", Seconds = 5)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContactMessageInputModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new SendContactUsEmailCommand
                {
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message
                });

                if(result.IsSuccessful)
                return Ok();

                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState.Errors());
        }
    }
}