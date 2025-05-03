using System.ComponentModel.DataAnnotations;

namespace Htime.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Total { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        public string Status { get; set; } = "Pending";

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
