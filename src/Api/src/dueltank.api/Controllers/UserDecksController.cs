﻿using AutoMapper;
using dueltank.api.Models;
using dueltank.application.Commands.DeleteDeck;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Queries.DecksByUserId;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class UserDecksController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDecksController(UserManager<ApplicationUser> userManager, IMapper mapper, IMediator mediator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromQuery] SearchDecksInputModel searchModel)
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

            return BadRequest();
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            var user = await GetCurrentUserAsync();

            if (user != null)
            {
                var command = new DeleteDeckCommand
                {
                    Deck = new DeckInputModel
                    {
                        Id = id,
                        UserId = user.Id
                    }
                };

                var result = await _mediator.Send(command);

                if (result.IsSuccessful)
                    return Ok(new { id = result.Data} );

                return BadRequest(result.Errors);
            }

            return BadRequest();
        }

        #region private helpers

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User);
        }

        #endregion

    }
}