using System.ComponentModel.DataAnnotations;

namespace SoulBowl.Data
{
    public class OrderItem
    {
        [Key, Required]
        public int OrderNo { get; set; }
        [Required]
        public int StockID { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
