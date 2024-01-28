using MedRoute.Database;
using MedRoute.Models;
using MedRoute.Repository;
using MedRoute.Repository.Implement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedRoute.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;

        /*        public BookingController()
                {
                    AppDBContext context = new AppDBContext();
                    _bookingRepository = new BookingRepository(context);
                }*/
        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IActionResult> Index()
        {
            Booking booking = null;
            var bookingRs = await ((BookingRepository)_bookingRepository).GetAllAsync();
            if (bookingRs.IsSuccess)
            {
                booking = (bookingRs.Data as DbSet<Booking>).ToList().
                    OrderByDescending(b => b.Date).
                    FirstOrDefault();
            }
            ViewBag.LatestBooking = booking;
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
