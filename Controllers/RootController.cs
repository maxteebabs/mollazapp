using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/")]
    [ApiVersion("1.0")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        [ProducesResponseType(200)]
        public IActionResult GetRoot()
        {
            var response = new RootResponse()
            {
//                Href = null,
                Self = Link.To(nameof(GetRoot), null),
                Users = Link.To(nameof(UserController.GetUsers), null),
                Info = null // Url.Link(nameof(UserController.GetUsers), null),
            };
            return Ok(response);
        }
    }
}