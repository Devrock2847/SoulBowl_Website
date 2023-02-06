using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SoulBowl.Data;
using SoulBowl.Models;

namespace SoulBowl.Pages.Menus
{
    public class DetailsModel : PageModel
    {
        private readonly SoulBowl.Data.MenuContext _context;

        public DetailsModel(SoulBowl.Data.MenuContext context)
        {
            _context = context;
        }
      public MenuItem MenuItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.MenuItems == null)
            {
                return NotFound();
            }
            var menuitem = await _context.MenuItems.FirstOrDefaultAsync(m => m.ID == id);
            if (menuitem == null)
            {
                return NotFound();
            }
            else 
            {
                MenuItem = menuitem;
            }
            return Page();
        }
    }
}
