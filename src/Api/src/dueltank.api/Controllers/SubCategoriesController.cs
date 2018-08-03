using System.Net;
using System.Threading.Tasks;
using dueltank.application.Queries.AllCategories;
using dueltank.application.Queries.AllSubCategories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class SubCategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public SubCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieve all the SubCategories
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new AllSubCategoriesQuery());

            return Ok(result);
        }
    }
}