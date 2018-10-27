using System.Net;
using System.Threading.Tasks;
using dueltank.api.Constants;
using dueltank.application.Queries.AllCategories;
using dueltank.application.Queries.ArchetypeById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class ArchetypesController : Controller
    {
        private readonly IMediator _mediator;

        public ArchetypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get archetype by id
        /// </summary>
        /// <param name="archetypeId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{archetypeId:long}")]
        public async Task<IActionResult> Get([FromRoute] long archetypeId)
        {
            var result = await _mediator.Send(new ArchetypeByIdQuery { ArchetypeId = archetypeId });

            if (result != null)
                return Ok(result);

            return NotFound();
        }
    }
}