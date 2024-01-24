using MedRoute.Models;
using MedRoute.Repository;
using MedRoute.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedRoute.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticateService _authenticateService;

        public AuthController(IUserRepository userRepository, IAuthenticateService authenticateService)
        {
            _userRepository = userRepository;
            _authenticateService = authenticateService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username, Password")] Login login)
        {
            if (ModelState.IsValid)
            {
                var status = await _authenticateService.ValidateLoginAsync(login);

                if (status.IsSuccess)
                {
                    await HttpContext.LoginAsync(status.Data as User);

                    TempData["Message"] = "Login successfully";
                    return RedirectToAction("Index", "Home");
                }

                TempData["Message"] = status.Message;
            }

            return View();
        }

        [Authorize(AuthenticationSchemes = "Scheme")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password,AgainPassword,FullName,Email")]
                                Register register)
        {
            if (ModelState.IsValid)
            {
                var checkStatus = await _authenticateService.CheckEmailAndUsernameAsync(register.Email, register.Username);

                if ((bool)checkStatus.Data)
                {
                    return View();
                }

                _authenticateService.RegisterAsync(register);

                return RedirectToAction("Login");
            }
            ModelState.AddModelError(String.Empty, "Some fields was wrong");

            return View();
        }
    }
}
