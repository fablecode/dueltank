using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        [AllowAnonymous]
        [HttpGet("cards/{name}")]
        public Task<IActionResult> Cards(string name)
        {
            throw new NotImplementedException();
        }
    }
}