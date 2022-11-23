using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoulBowl.Models;

namespace SoulBowl.Data
{
    public class MenuContext : DbContext
    {
        public MenuContext (DbContextOptions<MenuContext> options)
            : base(options)
        {
        }

        public DbSet<MenuItem> MenuItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>().ToTable("Menu");
        }
    }
}
