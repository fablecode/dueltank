using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.api.Models;
using dueltank.api.ServiceExtensions;
using dueltank.application.Queries.DecksByUserId;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper, IMediator mediator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] SearchDecksByUserIdInputModel searchModel)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    var inputModel = _mapper.Map<DecksByUserIdInputModel>(searchModel);

                    var result = await _mediator.Send(new DecksByUserIdQuery
                    {
                        UserId = user.Id,
                        SearchTerm = inputModel.SearchTerm,
                        PageSize = inputModel.PageSize,
                        PageIndex = inputModel.PageIndex
                    });

                    return Ok(result);
                }
            }
            else
            {
                return BadRequest(ModelState.Errors());
            }

            return BadRequest();
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(User);
    }

    [Route("api/[controller]")]
    public class FavouriteDecksController : Controller
    {
    }

    [Route("api/[controller]")]
    public class UserDecksController : Controller
    {
    }

    public sealed class SearchDecksByUserIdInputModel
    {
        [MaxLength(255)]
        public string SearchTerm { get; set; }

        [MinLength(1)]
        [MaxLength(20)]
        public int PageSize { get; set; } = 10;

        [MinLength(1)]
        public int PageIndex { get; set; } = 1;
    }

    public sealed class DecksByUserIdInputModel
    {
        public string UserId { get; set; }

        public string SearchTerm { get; set; }

        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 1;
    }
}