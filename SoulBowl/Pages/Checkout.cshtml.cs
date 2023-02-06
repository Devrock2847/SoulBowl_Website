using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using SoulBowl.Data;
using Microsoft.EntityFrameworkCore;

namespace SoulBowl.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly MenuContext _db;
        private readonly UserManager<IdentityUser> _UserManager;
        public IList<CheckoutItem> Items { get; private set; }
        public decimal Total;
        public long AmountPayable;

        public CheckoutModel(MenuContext db, UserManager<IdentityUser> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }
        public async Task OnGetAsync()
        {
            var user = await _UserManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            Items = _db.CheckoutItems.FromSqlRaw(
                "SELECT Menu.ID, " +
				"CONVERT(Decimal(20, 2), Price, 2) AS Price, " +
				"Menu.ItemName, " +
                "BasketItems.BasketID, BasketItems.Quantity " +
                "FROM Menu INNER JOIN BasketItems " +
                "ON Menu.ID = BasketItems.StockID " +
                "WHERE BasketID = {0}", customer.BasketID
                ).ToList();
            Total = 0;

            foreach (var item in Items)
            {
                var quantityInt = Convert.ToInt32(item.Quantity);
                var priceInt = Convert.ToInt32(item.Price);
				Total += (quantityInt * priceInt);
            }
            AmountPayable = (long)Total;
        }
    }
}
