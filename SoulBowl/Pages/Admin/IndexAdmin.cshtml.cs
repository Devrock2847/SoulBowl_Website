using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace SoulBowl.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexAdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
