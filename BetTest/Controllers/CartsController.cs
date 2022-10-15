using BetTest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BetTest.Controllers
{
    public class CartsController : Controller
    {
        private readonly BetTestDBContext _context;

        public CartsController(BetTestDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cart = await _context.ViewCart.ToListAsync();
            return View(cart);
        }
    }
}
