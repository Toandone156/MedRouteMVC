using MedRoute.Database;
using MedRoute.Models;
using MedRoute.Repository;
using MedRoute.Utils;
using MedRoute.Repository.Implement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MedRoute.Services;
using System.Net.NetworkInformation;
using MedRoute.Models.System;

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
                StatusMessage medicalRecordRs;
                // get booking information      
                var bookingRs = await ((BookingRepository)_bookingRepository).GetAllAsync();
                if (bookingRs.IsSuccess)
                {
                    booking = (bookingRs.Data as DbSet<Booking>).ToList().
                        OrderByDescending(b => b.Date).
                        FirstOrDefault();
                }
                else throw new Exception(bookingRs.Message);
                // check 
                var bookingDate = DateOnly.FromDateTime(booking.Date).ToString("yyyy-MM-dd");
                var dateNow = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                if (bookingDate != dateNow)
                {
                    bookingRs = await ((BookingRepository)_bookingRepository).CreateAsync(new Booking
                    {
                        Date= DateTime.Parse(dateNow),
                        Order = 1
                    });;
                    if (bookingRs.IsSuccess)
                    {
                        booking = bookingRs.Data as Booking;
                    }
                    else throw new Exception(bookingRs.Message);
                }
                // checking 
                if (medicalRecordId <= 0)
                {
                     medicalRecordRs = await ((MedicalRecordRepository)_medicalRecordRepository).GetAllAsync();
                    if (medicalRecordRs.IsSuccess)
                    {
                        medicalRecord = (medicalRecordRs.Data as DbSet<MedicalRecord>).ToList().
                            Where(p => p.PatientId == userId && p.Status == ConstVariable.MEDICAL_RECORD_STATUS_WAITING && p.BookingId == booking.BookingId).FirstOrDefault();
                        if(medicalRecord!= null)
                        {
                            medicalRecordId = medicalRecord.MedicalRecordId;
                        }
                       
                        cookieHandler.AddCookie(HttpContext, 1, "MedicalRecordId", medicalRecordId.ToString());
                    }
                    else throw new Exception(medicalRecordRs.Message);
                }
                
                
                /* // set user in cookie
               cookieHandler.AddCookie(HttpContext, 1, "MedicalRecordId", medicalRecord.MedicalRecordId.ToString());*/
                // get insurrance if it have
                if (medicalRecordId > 0)
                {
                    medicalRecordRs = await ((MedicalRecordRepository)_medicalRecordRepository).GetByIdAsync(medicalRecordId);
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

        private async Task<MedicalRecord> CreateAMedicalRecord(MedicalRecord medicalRecord)
        {
                        /*MedicalDetail = bookingForm.MedicalDetail,
                        MedicalResult = "",
                        PatientId = user.UserId,
                        BookingId = bookingForm.BookingId,
                        BookingOrder = booking.Order,
                        Status = ConstVariable.MEDICAL_RECORD_STATUS_WAITING*/
            //MedicalRecord medicalRecord = null;
            var medicalRs = await ((MedicalRecordRepository)_medicalRecordRepository).
                    CreateAsync(medicalRecord);
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
        public async Task<IActionResult> Book([Bind()] BookingForm bookingForm)
        {
            //var u = bookingForm.user.FullName;
            try
            {
                // set cookie
                var cookieHandler = new CookieService();
                // declare
                var bookingID = int.Parse(bookingForm.bookingIDStr);
                Booking booking = null;
                // get booking
                // get booking information      
                var bookingRs = await ((BookingRepository)_bookingRepository).GetAllAsync();
                if (bookingRs.IsSuccess)
                {
                    booking = (bookingRs.Data as DbSet<Booking>).ToList().
                        OrderByDescending(b => b.Date).
                        FirstOrDefault();
                }
                else throw new Exception(bookingRs.Message);
                User user = null;
                //User user = null;
                MedicalRecord medicalRecord = null;
                // get 
                var userId = Convert.ToInt32(User.FindFirstValue("Id"));
                if (userId <= 0)
                {
                    // create a guest
                    var userRs = await ((UserRepository)_userRepository).
                    CreateAsync(new Models.User
                    {
                        UserName = "",
                        FullName = "",
                        DateOfBirth = bookingForm.DateOfBirth,
                        Gender = true,
                        PhoneNumber = "",
                        Email = "",
                        InsuranceCode = "",
                        RoleId = 1
                    });
                    if (userRs.IsSuccess)
                        user = (userRs.Data as User);
                    else throw new Exception(userRs.Message);
                    userId = user.UserId;
                }
                medicalRecord = await CreateAMedicalRecord(new MedicalRecord()
                {
                    MedicalDetail = "",
                    MedicalResult = "",
                    PatientId = userId,
                    BookingId = bookingID,
                    BookingOrder = booking.Order,
                    Status = ConstVariable.MEDICAL_RECORD_STATUS_WAITING
                });
                // increase
                booking.Order = booking.Order + 1;
                 bookingRs = await ((BookingRepository)_bookingRepository).UpdateAsync(booking);
                if (!bookingRs.IsSuccess)
                    throw new Exception(bookingRs.Message);
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
                // get medicalRecord  
                var medicalRecordRs= await  ((MedicalRecordRepository)_medicalRecordRepository).GetByIdAsync(medicalRecordId);
                if (medicalRecordRs.IsSuccess)
                    medicalRecord = (medicalRecordRs.Data as MedicalRecord);
                else throw new Exception(medicalRecordRs.Message);
                // set value user
                if (userId <= 0) {
                    userId = medicalRecord.PatientId.Value;
                }
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
                    /*MedicalDetail = bookingForm.MedicalDetail,
                        MedicalResult = "",
                        PatientId = user.UserId,
                        BookingId = bookingForm.BookingId,
                        BookingOrder = booking.Order,
                        Status = ConstVariable.MEDICAL_RECORD_STATUS_WAITING*/
                    medicalRecord = await CreateAMedicalRecord(new MedicalRecord()
                    {
                        MedicalDetail = bookingForm.MedicalDetail,
                        MedicalResult = "",
                        PatientId = user.UserId,
                        BookingId = bookingForm.BookingId,
                        BookingOrder = booking.Order,
                        Status = ConstVariable.MEDICAL_RECORD_STATUS_WAITING
                    });
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
                    medicalRecord = await CreateAMedicalRecord(new MedicalRecord()
                    {
                        MedicalDetail = bookingForm.MedicalDetail,
                        MedicalResult = "",
                        PatientId = user.UserId,
                        BookingId = bookingForm.BookingId,
                        BookingOrder = booking.Order,
                        Status = ConstVariable.MEDICAL_RECORD_STATUS_WAITING
                    });
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
