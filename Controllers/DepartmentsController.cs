using Microsoft.AspNetCore.Mvc;

namespace MedRoute.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
