using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentRentalUI.Controllers
{
    public class AuthController : Controller
    {
        // GET: /auth/login
        [HttpGet("auth/login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = returnUrl
            }, "Google");
        }

        // POST: /auth/logout
        [HttpPost("auth/logout")]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            }, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        // GET: /auth/denied
        [HttpGet("auth/denied")]
        public IActionResult AccessDenied()
        {
            return Content("Access Denied - You do not have permission to access this resource.");
        }
    }
}