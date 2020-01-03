using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class OrderController : ControllerBase
    {
        // GET
        public IActionResult Index()
        {
            return Ok();
        }
    }
}