using Microsoft.EntityFrameworkCore;
using Pos.Api.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Pos.Api.Data
{
    public class PosDbContext : DbContext
    {
        public PosDbContext(DbContextOptions<PosDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Location code unique
            modelBuilder.Entity<Location>()
                .HasIndex(l => l.Code)
                .IsUnique();
        }
    }
}
