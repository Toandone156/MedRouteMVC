using MedRoute.Database;
using MedRoute.Models;
using MedRoute.Repository;
using MedRoute.Utils;
using MedRoute.Repository.Implement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedRoute.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        //private readonly IBookingRepository _bookingRepository;

        /*        public BookingController()
                {
                    AppDBContext context = new AppDBContext();
                    _bookingRepository = new BookingRepository(context);
                }*/
        public BookingController(IBookingRepository bookingRepository, 
            IUserRepository userRepository, 
            IMedicalRecordRepository medicalRecordRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<IActionResult> Index()
        {
            //var userId = Convert.ToInt32(User.FindFirstValue("Id"));
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
        [HttpPost]
        public async Task<IActionResult> Book([Bind()] BookingForm bookingForm)
        {
            //var u = bookingForm.user.FullName;
            try
            {
                // declare
                Booking booking = null;
                User user = null;
                MedicalRecord medicalRecord = null;
                // get userid
                var userId = Convert.ToInt32(User.FindFirstValue("Id"));
                // create a guest
                if(userId == null || userId == 0)
                {
                    // case user loged in
                    var userRs = await ((UserRepository)_userRepository).
                    CreateAsync(new Models.User
                    {
                        UserName = "",
                        FullName = bookingForm.FullName,
                        DateOfBirth = bookingForm.DateOfBirth,
                        Gender = bookingForm.Gender,
                        PhoneNumber = bookingForm.PhoneNumber,
                        Email = bookingForm.Email,
                        InsuranceCode = bookingForm.InsuranceCode,
                        RoleId = 1
                    });
                    if (userRs.IsSuccess)
                        user = (userRs.Data as User);
                    else throw new Exception(userRs.Message);
                }
                else
                {
                    // case user do not log in
                    var userRs = await ((UserRepository)_userRepository).
                    GetByIdAsync(userId);
                    if (userRs.IsSuccess)
                        user = (userRs.Data as User);
                    else throw new Exception(userRs.Message);
                }
                

                // get bookingid and bookingorder
                var bookingRs = await ((BookingRepository)_bookingRepository).GetByIdAsync(bookingForm.BookingId);
                if (bookingRs.IsSuccess)
                    booking = (bookingRs.Data as Booking);
                else throw new Exception(bookingRs.Message);

                // create a insurrance and save it in cookie
                var medicalRs = await ((MedicalRecordRepository)_medicalRecordRepository).
                    CreateAsync(new MedicalRecord
                    {
                        MedicalDetail = bookingForm.MedicalDetail,
                        MedicalResult= "",
                        PatientId = user.UserId,
                        BookingId = bookingForm.BookingId,
                        BookingOrder = booking.Order,
                        Status = ConstVariable.MEDICAL_RECORD_STATUS_WAITING
                    }) ;

                // increase
                booking.Order = booking.Order + 1;
                bookingRs = await ((BookingRepository)_bookingRepository).UpdateAsync(booking);
                if (!bookingRs.IsSuccess)
                    throw new Exception(bookingRs.Message);
                // booking successfull
                TempData["isSucces"] = true;
            }
            catch (Exception ex)
            {
                TempData["isSucces"] = false;
                TempData["ErrMess"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
