namespace Htime.Models.ViewModel
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public List<OrderDetail> Items { get; set; }
    }

}
