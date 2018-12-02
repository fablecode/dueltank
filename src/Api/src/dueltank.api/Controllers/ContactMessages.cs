using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]")]
    public class ContactMessages : Controller
    {
        [AllowAnonymous]
        [Throttle(Name = "ContactSupport", Seconds = 5)]
        [HttpPost]
        public IActionResult Post([FromBody] ContactMessageInputModel model)
        {
            return Ok();
        }
    }

    public class ContactMessageInputModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(2083, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        public string Message { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ThrottleAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }
        public int Seconds { get; set; }
        public string Message { get; set; }

        private static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());

        public override void OnActionExecuting(ActionExecutingContext c)
        {
            var key = string.Concat(Name, "-", c.HttpContext.Request.HttpContext.Connection.RemoteIpAddress);

            if (!Cache.TryGetValue(key, out bool entry))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(Seconds));

                Cache.Set(key, true, cacheEntryOptions);
            }
            else
            {
                if (string.IsNullOrEmpty(Message))
                    Message = "You may only perform this action every {n} seconds.";

                c.Result = new ContentResult { Content = Message.Replace("{n}", Seconds.ToString()) };
                c.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            }
        }
    }
}