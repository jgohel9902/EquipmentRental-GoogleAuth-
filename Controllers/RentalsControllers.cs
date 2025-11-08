using EquipmentRentalApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentRentalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RentalsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("active")]
        public IActionResult GetActive()
        {
            var list = _context.Rentals
                .Where(r => r.ReturnedAt == null)
                .ToList();
            return Ok(list);
        }

        [HttpGet("overdue")]
        public IActionResult GetOverdue()
        {
            var list = _context.Rentals
                .Where(r => r.ReturnedAt == null && r.DueDate < DateTime.UtcNow)
                .ToList();
            return Ok(list);
        }

        [HttpGet("completed")]
        public IActionResult GetCompleted()
        {
            var list = _context.Rentals
                .Where(r => r.ReturnedAt != null)
                .ToList();
            return Ok(list);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Rentals.ToList());
        }
    }
}
