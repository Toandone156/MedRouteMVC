using MedRoute.Database;
using MedRoute.Models;
using MedRoute.Repository;
using MedRoute.Utils;
using MedRoute.Repository.Implement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MedRoute.Services;

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
            try
            {
                // set cookie
                var cookieHandler = new CookieService();
                // get init value
                var userId = Convert.ToInt32(User.FindFirstValue("Id"));
                var medicalRecordId = Convert.ToInt32(cookieHandler.GetCookie(HttpContext, "MedicalRecordId"));
                // declare
                Booking booking = null;
                User user = null;
                MedicalRecord medicalRecord = null;
                // get booking information      
                var bookingRs = await ((BookingRepository)_bookingRepository).GetAllAsync();
                if (bookingRs.IsSuccess)
                {
                    booking = (bookingRs.Data as DbSet<Booking>).ToList().
                        OrderByDescending(b => b.Date).
                        FirstOrDefault();
                }
                else throw new Exception(bookingRs.Message);
                // get insurrance if it have
                if (medicalRecordId > 0)
                {
                    var medicalRecordRs = await ((MedicalRecordRepository)_medicalRecordRepository).GetByIdAsync(medicalRecordId);
                    if (medicalRecordRs.IsSuccess)
                    {
                        medicalRecord = medicalRecordRs.Data as MedicalRecord;
                        userId = (userId > 0 )? userId : medicalRecord.PatientId.GetValueOrDefault();
                    }
                    else throw new Exception(medicalRecordRs.Message);
                }
                // get user information
                var userRs = await ((UserRepository)_userRepository).GetByIdAsync(userId);
                if (userRs.IsSuccess)
                    user = userRs.Data as User;
                else throw new Exception(userRs.Message);
                // sending data
                ViewBag.LatestBooking = booking;
                ViewBag.User = user;
                ViewBag.MedicalRecord = medicalRecord;
            }
            catch (Exception ex)
            {
                TempData["isSucces"] = false;
                TempData["ErrMess"] = ex.Message;
            }
            return View();
        }

        public IActionResult Book()
        {

            return View();
        }

        private async Task<MedicalRecord> CreateAMedicalRecord(BookingForm bookingForm, 
            Booking booking, User user)
        {
            MedicalRecord medicalRecord = null;
            var medicalRs = await ((MedicalRecordRepository)_medicalRecordRepository).
                    CreateAsync(new MedicalRecord
                    {
                        MedicalDetail = bookingForm.MedicalDetail,
                        MedicalResult = "",
                        PatientId = user.UserId,
                        BookingId = bookingForm.BookingId,
                        BookingOrder = booking.Order,
                        Status = ConstVariable.MEDICAL_RECORD_STATUS_WAITING
                    });
            if (medicalRs.IsSuccess)
                medicalRecord = (medicalRs.Data as MedicalRecord);
            else throw new Exception(medicalRs.Message);
            return medicalRecord;
        }
        private async Task<MedicalRecord> UpdateAMedicalRecord(MedicalRecord medicalRecord)
        {
            // 
            var medicalDetail = medicalRecord.MedicalDetail;
            // get record
            var medicalRs = await ((MedicalRecordRepository)_medicalRecordRepository).
                    GetByIdAsync(medicalRecord.MedicalRecordId);
            if (medicalRs.IsSuccess)
                medicalRecord = (medicalRs.Data as MedicalRecord);
            else throw new Exception(medicalRs.Message);
            // update 
            medicalRecord.MedicalDetail = medicalDetail;
            medicalRs = await ((MedicalRecordRepository)_medicalRecordRepository).
                    UpdateAsync(medicalRecord);
            if (medicalRs.IsSuccess)
                medicalRecord = (medicalRs.Data as MedicalRecord);
            else throw new Exception(medicalRs.Message);
            return medicalRecord;
        }

        [HttpPost]
        public async Task<IActionResult> BookUser([Bind()] BookingForm bookingForm)
        {
            //var u = bookingForm.user.FullName;
            try
            {
                // set cookie
                var cookieHandler = new CookieService();
                // declare
                Booking booking = null;
                User user = null;
                MedicalRecord medicalRecord = null;
                // get 
                var userId = Convert.ToInt32(User.FindFirstValue("Id"));
                var medicalRecordId = Convert.ToInt32(cookieHandler.GetCookie(HttpContext, "MedicalRecordId"));
                // get a customer
                var userRs = await ((UserRepository)_userRepository).
                GetByIdAsync(userId);
                if (userRs.IsSuccess)
                    user = (userRs.Data as User);
                else throw new Exception(userRs.Message);
                // set user information 
                user.FullName = bookingForm.FullName;
                user.DateOfBirth = bookingForm.DateOfBirth;
                user.Gender = bookingForm.Gender;
                user.PhoneNumber = bookingForm.PhoneNumber;
                user.Email = bookingForm.Email;
                user.InsuranceCode = bookingForm.InsuranceCode;
                // update user
                userRs = await ((UserRepository)_userRepository).
                UpdateAsync(user);
                if (userRs.IsSuccess)
                    user = (userRs.Data as User);
                else throw new Exception(userRs.Message);

                // get bookingid and bookingorder
                var bookingRs = await ((BookingRepository)_bookingRepository).GetByIdAsync(bookingForm.BookingId);
                if (bookingRs.IsSuccess)
                    booking = (bookingRs.Data as Booking);
                else throw new Exception(bookingRs.Message);

                // create a insurrance and save it in cookie
                if (medicalRecordId > 0)
                {
                    medicalRecord = await UpdateAMedicalRecord(new MedicalRecord()
                    {
                        MedicalRecordId = medicalRecordId,
                        MedicalDetail = bookingForm.MedicalDetail
                    });
                }
                else
                {
                    medicalRecord = await CreateAMedicalRecord(bookingForm, booking, user);
                    // increase
                    booking.Order = booking.Order + 1;
                    bookingRs = await ((BookingRepository)_bookingRepository).UpdateAsync(booking);
                    if (!bookingRs.IsSuccess)
                        throw new Exception(bookingRs.Message);
                }
                
               
                // booking successfull
                TempData["isSucces"] = true;
                // set cookie
                cookieHandler.AddCookie(HttpContext, 1, "MedicalRecordId", medicalRecord.MedicalRecordId.ToString());
            }
            catch (Exception ex)
            {
                TempData["isSucces"] = false;
                TempData["ErrMess"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> BookGuest([Bind()] BookingForm bookingForm)
        {
            //var u = bookingForm.user.FullName;
            try
            {
                // set cookie
                var cookieHandler = new CookieService();
                // declare
                Booking booking = null;
                User user = null;
                MedicalRecord medicalRecord = null;
                // get 
                var userId = Convert.ToInt32(User.FindFirstValue("Id"));
                var medicalRecordId = Convert.ToInt32(cookieHandler.GetCookie(HttpContext, "MedicalRecordId"));
                // create a guest
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
                
                // get bookingid and bookingorder
                var bookingRs = await ((BookingRepository)_bookingRepository).GetByIdAsync(bookingForm.BookingId);
                if (bookingRs.IsSuccess)
                    booking = (bookingRs.Data as Booking);
                else throw new Exception(bookingRs.Message);

                // create a insurrance and save it in cookie
                if (medicalRecordId > 0)
                {
                    medicalRecord = await UpdateAMedicalRecord(new MedicalRecord()
                    {
                        MedicalRecordId = medicalRecordId,
                        MedicalDetail = bookingForm.MedicalDetail
                    });
                }
                else
                {
                    medicalRecord = await CreateAMedicalRecord(bookingForm, booking, user);
                    // increase
                    booking.Order = booking.Order + 1;
                    bookingRs = await ((BookingRepository)_bookingRepository).UpdateAsync(booking);
                    if (!bookingRs.IsSuccess)
                        throw new Exception(bookingRs.Message);
                }
               
                // booking successfull
                TempData["isSucces"] = true;
                // set cookie
                cookieHandler.AddCookie(HttpContext, 1, "MedicalRecordId", medicalRecord.MedicalRecordId.ToString());
            }
            catch (Exception ex)
            {
                TempData["isSucces"] = false;
                TempData["ErrMess"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Cancel()
        {
            // set cookie
            var cookieHandler = new CookieService();
            // declare
            MedicalRecord medicalRecord = null;
            var medicalRecordId = Convert.ToInt32(cookieHandler.GetCookie(HttpContext, "MedicalRecordId"));
            try
            {
                if (medicalRecordId <= 0) throw new Exception("medicalRecordId not exist");
                // get record
                var medicalRs = await ((MedicalRecordRepository)_medicalRecordRepository).
                        GetByIdAsync(medicalRecordId);
                if (medicalRs.IsSuccess)
                    medicalRecord = (medicalRs.Data as MedicalRecord);
                else throw new Exception(medicalRs.Message);
                // update to cancle
                medicalRecord.Status = ConstVariable.MEDICAL_RECORD_STATUS_CANCEL;
                medicalRs = await ((MedicalRecordRepository)_medicalRecordRepository).
                        UpdateAsync(medicalRecord);
                if (medicalRs.IsSuccess)
                    medicalRecord = (medicalRs.Data as MedicalRecord);
                else throw new Exception(medicalRs.Message);
                // remove
                cookieHandler.RemoveCookie(HttpContext, "MedicalRecordId");

            }
            catch (Exception ex)
            {
                TempData["isSucces"] = false;
                TempData["ErrMess"] = ex.Message;
            }
            cookieHandler.RemoveCookie(HttpContext, "MedicalRecordId");
            return RedirectToAction("Index");
        }
    }
}
