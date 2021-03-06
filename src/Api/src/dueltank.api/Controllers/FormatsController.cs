﻿using System.Net;
using System.Threading.Tasks;
using dueltank.api.Constants;
using dueltank.application.Queries.AllFormats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class FormatsController : Controller
    {
        private readonly IMediator _mediator;

        public FormatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Retrieve all the Formats
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ResponseCache(CacheProfileName = CacheConstants.OneWeekPolicy)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new AllFormatsQuery());

            return Ok(result);
        }
    }
}