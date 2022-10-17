using BetTest.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BetTest.Data
{
    public class BetTestDBContext : DbContext
    {
        
        public BetTestDBContext(DbContextOptions<BetTestDBContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ViewCart> ViewCart { get; set; }


    }
}
