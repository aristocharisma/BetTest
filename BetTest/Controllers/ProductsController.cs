using BetTest.Areas.Identity.Data;
using BetTest.Data;
using BetTest.Models;
using BetTest.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BetTest.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly BetTestDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(BetTestDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var product = await _context.Products.ToListAsync();
            return View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel createProductRequest)
        {

            var product = new Product();
            {
                product.Id = Guid.NewGuid();
                product.Name = createProductRequest.Name;
                product.Description = createProductRequest.Description;
                product.Photo = createProductRequest.Photo;
                product.Price = createProductRequest.Price;
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null)
            {
                var viewmodel = new UpdateProductViewModel();
                {
                    viewmodel.Id = product.Id;
                    viewmodel.Name = product.Name;
                    viewmodel.Description = product.Description;
                    viewmodel.Photo = product.Photo;
                    viewmodel.Price = product.Price;
                }
                return await Task.Run(() => View("View", viewmodel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateProductViewModel model)
        {
            var product = await _context.Products.FindAsync(model.Id);

            if (product != null)
            {
                product.Name = model.Name;
                product.Description = model.Description;
                product.Photo = model.Photo;
                product.Price = model.Price;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateProductViewModel model)
        {
            var product = await _context.Products.FindAsync(model.Id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCartItems(CreateCartViewModel createCart, Guid id)
        {
            Guid productId = id;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var checkcart = await _context.Carts.FirstOrDefaultAsync(x => x.userId == userId);
            var cart = new Cart();
            if (checkcart == null)
            {               
                {
                    cart.Id = Guid.NewGuid();
                    cart.userId = userId;
                    cart.Adddate = DateTime.Now;
                    await _context.Carts.AddAsync(cart);
                    await _context.SaveChangesAsync();
                }
            }            
           
            var cartItem = new CartItem();
            {
                cartItem.Id = Guid.NewGuid();
                cartItem.ProductId = productId;
                if (checkcart != null)
                {
                    cartItem.CartId = checkcart.Id;
                }
                else
                {
                    cartItem.CartId = cart.Id;
                }
                cartItem.Qunatity = createCart.Quantity;
            }
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Carts");
        }


    }
}
