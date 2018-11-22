﻿using AutoMapper;
using dueltank.api.Models;
using dueltank.api.ServiceExtensions;
using dueltank.application.Queries.DecksByUserId;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using dueltank.application.Models.Decks.Input;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class UserDecksController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UserDecksController(UserManager<ApplicationUser> userManager, IMapper mapper, IMediator mediator)
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
}