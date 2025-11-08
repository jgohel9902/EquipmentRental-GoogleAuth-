using EquipmentRentalUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EquipmentRentalUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _apiClient;

        public HomeController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [AllowAnonymous]
        public IActionResult Index() => View();
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            var allRentals = await _apiClient.GetProtectedDataAsync("api/rentals", User);
            var activeRentals = await _apiClient.GetProtectedDataAsync("api/rentals/active", User);
            var overdueRentals = await _apiClient.GetProtectedDataAsync("api/rentals/overdue", User);
            var allEquipment = await _apiClient.GetProtectedDataAsync("api/equipment", User);

            ViewBag.RentedCount = JsonConvert.DeserializeObject<List<object>>(activeRentals)?.Count ?? 0;
            ViewBag.OverdueCount = JsonConvert.DeserializeObject<List<object>>(overdueRentals)?.Count ?? 0;
            ViewBag.EquipmentCount = JsonConvert.DeserializeObject<List<object>>(allEquipment)?.Count ?? 0;

            return View();
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserDashboard()
        {
            var json = await _apiClient.GetProtectedDataAsync("api/equipment", User);
            var equipments = JsonConvert.DeserializeObject<List<dynamic>>(json);
            ViewBag.Equipments = equipments?.Where(e => (bool)e.isAvailable).ToList();
            return View();
        }
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Secure()
        {
            var data = await _apiClient.GetProtectedDataAsync("api/Auth/me", User);
            return Content($"Secure Page\n\nData from API:\n{data}");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOnly()
        {
            var data = await _apiClient.GetProtectedDataAsync("api/Auth/admin", User);
            return Content($"Admin Page\n\nData from API:\n{data}");
        }
    }
}
