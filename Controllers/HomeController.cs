using MedRoute.Models;
using MedRoute.Models.System;
using MedRoute.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedRoute.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRoleRepository _roleRepository;
        public HomeController(ILogger<HomeController> logger, IRoleRepository roleRepository)
        {
            _logger = logger;
            _roleRepository = roleRepository;
        }

        public async Task<IActionResult> Index()
        {
            //Add
            var returnMessage = await _roleRepository.CreateAsync(new Role
            {
                RoleName = "New role"
            });

            var roleObj = (Role)returnMessage.Data;

            //Update
            await _roleRepository.UpdateAsync(new Role
            {
                RoleId = roleObj.RoleId,
                RoleName = roleObj.RoleName + "1"
            });

            //Get
            var a = await _roleRepository.GetAllAsync();

            //GetById
            var b = await _roleRepository.GetByIdAsync(roleObj.RoleId);

            //GetByCondition
            var c = await _roleRepository.GetByConditionsAsync(e => e.RoleName.Contains("1"));

            //Delete
            await _roleRepository.DeleteAsync(roleObj.RoleId);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
