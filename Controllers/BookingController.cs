using MedRoute.Models;
using MedRoute.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MedRoute.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Book()
        {
            return View();
        }
        public IActionResult Book([Bind()] MedicalRecord record)
        {
            return View();
        }
    }
}
