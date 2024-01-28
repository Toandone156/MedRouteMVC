using Microsoft.AspNetCore.Mvc;

namespace MedRoute.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
