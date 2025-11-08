using EquipmentRentalUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EquipmentRentalUI.Controllers
{
    [Authorize(Roles = "Admin,User")] 
    public class RentalsController : Controller
    {
        private readonly ApiClient _apiClient;

        public RentalsController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Active()
        {
            var json = await _apiClient.GetProtectedDataAsync("api/rentals/active", User);
            var data = JsonConvert.DeserializeObject<List<dynamic>>(json);
            return View(data);
        }

        public async Task<IActionResult> Overdue()
        {
            var json = await _apiClient.GetProtectedDataAsync("api/rentals/overdue", User);
            var data = JsonConvert.DeserializeObject<List<dynamic>>(json);
            return View(data);
        }

        public async Task<IActionResult> Completed()
        {
            var json = await _apiClient.GetProtectedDataAsync("api/rentals/completed", User);
            var data = JsonConvert.DeserializeObject<List<dynamic>>(json);
            return View(data);
        }
    }
}
