using Htime.Models;

namespace Htime.Models.ViewModel
{
    public class CartViewModel
    {
        public List<Cart> Carts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
