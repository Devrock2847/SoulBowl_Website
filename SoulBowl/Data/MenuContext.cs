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
        public DbSet<MenuItem> MenuItem { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //new line of code
            modelBuilder.Entity<MenuItem>().ToTable("Menu");
        }
    }
}
