using EquipmentRentalUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EquipmentRentalUI.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class EquipmentController : Controller
    {
        private readonly ApiClient _apiClient;

        public EquipmentController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<IActionResult> Index()
        {
            var json = await _apiClient.GetProtectedDataAsync("api/equipment", User);
            var equipmentList = JsonConvert.DeserializeObject<List<dynamic>>(json);
            return View(equipmentList);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(string name, string category, string description, string condition, decimal rentalPrice)
        {
            var payload = new
            {
                Name = name,
                Category = category,
                Description = description,
                Condition = condition,
                RentalPrice = rentalPrice,
                IsAvailable = true
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync("api/equipment", content, User);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            TempData["Error"] = "Failed to add equipment.";
            return View();
        }
    }
}
