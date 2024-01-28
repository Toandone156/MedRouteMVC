using Microsoft.AspNetCore.Mvc;

namespace MedRoute.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
