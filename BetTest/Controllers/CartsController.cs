using BetTest.Areas.Identity.Data;
using BetTest.Data;
using BetTest.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BetTest.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly BetTestDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartsController(BetTestDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.ViewCart.Where(c => c.UserId == userId).ToListAsync();    
            return View(cart);
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
          var cartItems = await _context.ViewCart.Where(c => c.UserId == userId).ToListAsync();
            string bodytext = "";
            var emaildto = new EmailDto();
            emaildto.To = userEmail;
            emaildto.Subject = "Your Purchases";
            foreach (var e in cartItems)
            {
                
                string html = "<table border = '1'><tr><td>Product</td><td>Price</td><td>Total</td></tr><tr><td>{Product}</td><td>{Price}</td><td>{Total}</td></tr></table>";
                string product = e.Name;
                string price = e.Price.ToString();
                string total = e.TotalInclVat.ToString();
                html = html.Replace("{Product}", product).Replace("{Price}", price).Replace("{Total}", price);
                bodytext = html;
            }
           
            emaildto.Body = bodytext;
            return RedirectToAction("Index", "Products");
        }
        
    }
}
