using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using SoulBowl.Data;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Microsoft.Extensions.Configuration;
using System;

namespace SoulBowl.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly MenuContext _db;
        private readonly UserManager<IdentityUser> _UserManager;
		private readonly IConfiguration _configuration;
		public IList<CheckoutItem> Items { get; private set; }
        public decimal Total;
        public long AmountPayable;


        //Order is used here 
		public OrderHistory Order = new OrderHistory();
		public CheckoutModel(MenuContext db, UserManager<IdentityUser> UserManager, IConfiguration configuration)
        {
            _db = db;
            _UserManager = UserManager;
            _configuration = configuration;
        }
        public async Task OnGetAsync()
        {
            var user = await _UserManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            Items = _db.CheckoutItems.FromSqlRaw(
                "SELECT Menu.ID, Menu.ImageData, " +
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
                Total += (item.Quantity * item.Price);
            }
            AmountPayable = (long)(Total * 100);

        }
        public async Task Process()
        {
            var currentOrder = _db.OrderHistories.FromSqlRaw("SELECT * FROM OrderHistories")
                .OrderByDescending(b => b.OrderNo)
                .FirstOrDefault();

            if (currentOrder == null)
            {
                Order.OrderNo = 1;
            }
            else
            {
                Order.OrderNo = currentOrder.OrderNo + 1;
            }
            var user = await _UserManager.GetUserAsync(User);
            Order.Email = user.Email;
            _db.OrderHistories.Add(Order);

            CheckoutCustomer customer = await _db
                .CheckoutCustomers
                .FindAsync(user.Email);

            var basketItems =
                _db.BasketItems.FromSqlRaw("SELECT * FROM BasketItems WHERE BasketID = {0}", customer.BasketID)
                .ToList();

            foreach (var item in basketItems)
            {
                OrderItem oi = new OrderItem
                {
                    OrderNo = Order.OrderNo,
                    StockID = item.StockID,
                    Quantity = item.Quantity
                };
                _db.OrderItems.Add(oi);
                _db.BasketItems.Remove(item);
            }
            await _db.SaveChangesAsync();
            //return RedirectToPage("/Index");
        }
		public async Task<IActionResult> OnPostChargeAsync(string stripeEmail, string stripeToken, long amount)
		{
			var customers = new CustomerService();
			var charges = new ChargeService();

			var customer = customers.Create(new CustomerCreateOptions
			{
				Email = stripeEmail,
				Source = stripeToken
			});

			var charge = charges.Create(new ChargeCreateOptions
			{
				Amount = amount,
				Description = "Soul Bowl Charge",
				Currency = "gbp",
				Customer = customer.Id
			});

            await Process();

			return RedirectToPage("/CheckoutConfirm");
		}
	}
}
