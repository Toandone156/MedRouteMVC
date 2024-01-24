using Microsoft.AspNetCore.Mvc;

namespace MedRoute.Areas.Manage.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
