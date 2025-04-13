using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult Error() => View("Error");

        [Route("Error/404")]
        public IActionResult Error404() => View("404");

        [Route("Error/500")]
        public IActionResult Error500() => View("500");
    }
}