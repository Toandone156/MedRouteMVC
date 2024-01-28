using Microsoft.AspNetCore.Mvc;

namespace MedRoute.Controllers
{
    public class DoctorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
