using Microsoft.AspNetCore.Mvc;

namespace MedRoute.Controllers
{
    public class BocsoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
