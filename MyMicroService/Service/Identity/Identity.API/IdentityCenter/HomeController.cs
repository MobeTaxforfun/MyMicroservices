using Microsoft.AspNetCore.Mvc;

namespace Identity.API.IdentityCenter
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
