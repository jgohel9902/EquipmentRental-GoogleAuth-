using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentRentalUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
