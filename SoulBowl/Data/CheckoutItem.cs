using System.ComponentModel.DataAnnotations;

namespace SoulBowl.Data
{
	public class CheckoutItem
	{
		[Key, Required]
		public int ID { get; set; }
		[Required]
		public decimal Price { get; set; }
		[Required, StringLength(50)]
		public string ItemName { get; set; }
		[Required]
		public int Quantity { get; set; }
	}
}
