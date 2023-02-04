using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SoulBowl.Data;
using SoulBowl.Models;
using Microsoft.AspNetCore.Authorization;

namespace SoulBowl.Pages.Menus
{
    //[Authorize]
    //[Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly SoulBowl.Data.MenuContext _context;
        public IndexModel(SoulBowl.Data.MenuContext context)
        {
            _context = context;
        }
        public IList<MenuItem> MenuItem { get;set; } = default!;
        public async Task OnGetAsync()
        {
            if (_context.MenuItem != null)
            {
                MenuItem = await _context.MenuItem.ToListAsync();
            }
        }
    }
}
