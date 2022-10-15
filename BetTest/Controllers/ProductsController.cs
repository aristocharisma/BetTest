using BetTest.Data;
using BetTest.Models;
using BetTest.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BetTest.Controllers
{

    public class ProductsController : Controller
    {
        private readonly BetTestDBContext _context;

        public ProductsController(BetTestDBContext context)
        {
            _context = context;
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
            var cart = new Cart();
            {
                cart.Id = Guid.NewGuid();
                cart.Adddate = DateTime.Now;
               
            }
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            Guid productId = id;
            var cartItem = new CartItem();
            {
                cartItem.Id = Guid.NewGuid();
                cartItem.ProductId = productId;
                cartItem.CartId = cart.Id;
                cartItem.Qunatity = createCart.Quantity;
            }
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Carts");            
        }


    }
}
