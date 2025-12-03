using System.Linq;
using Pos.Api.Models;

namespace Pos.Api.Data
{
    public static class DbSeeder
    {
        public static void Seed(PosDbContext db)
        {
            // Locations
            if (!db.Locations.Any())
            {
                db.Locations.AddRange(
                    new Location { Code = "MAIN", Name = "Main Store" },
                    new Location { Code = "STORE2", Name = "Second Store" }
                );
                db.SaveChanges();
            }

            // Products
            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product { Sku = "COFFEE_SMALL", Name = "Small Coffee", Price = 2.50m },
                    new Product { Sku = "COFFEE_LARGE", Name = "Large Coffee", Price = 3.50m },
                    new Product { Sku = "PASTRY", Name = "Pastry", Price = 4.00m }
                );
                db.SaveChanges();
            }
        }
    }
}
