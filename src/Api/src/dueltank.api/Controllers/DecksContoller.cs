using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class DecksController : Controller
    {
        [HttpPost]
        [Authorize]
        [DisableRequestSizeLimit]
        public IActionResult UploadDeck([FromForm]IFormFile file)
        {
            return Ok();
        }

    }
}