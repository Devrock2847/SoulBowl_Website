using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoulBowl.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SoulBowl.Data
{
    public class MenuContext : IdentityDbContext
    {
        public MenuContext (DbContextOptions<MenuContext> options)
            : base(options)
        {
        }
        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<CheckoutCustomer> CheckoutCustomers { get; set; } = default!;
        public DbSet<Basket> Baskets { get; set; } = default!;
        public DbSet<BasketItem> BasketItems { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //new line of code
            modelBuilder.Entity<MenuItem>().ToTable("Menu");

            modelBuilder.Entity<BasketItem>().HasKey(t => new { t.StockID, t.BasketID });
        }
    }
}
