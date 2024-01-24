using MedRoute.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace MedRoute.Services
{
    public static class CookieAuthentication
    {
        public static async Task LoginAsync(this HttpContext context, User account)
        {
            string role = account.Role.RoleName;
            int id = account.UserId;
            string scheme = "Scheme";

            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", id.ToString()),
                new Claim(ClaimTypes.Name, account.FullName),
                new Claim(ClaimTypes.Role, role)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, scheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = false
            };

            await context.SignInAsync(scheme,
                new ClaimsPrincipal(claimsIdentity), properties);
        }

        public static async Task LogoutAsync(this HttpContext context)
        {
            await context.SignOutAsync("Scheme");
        }
    }
}
