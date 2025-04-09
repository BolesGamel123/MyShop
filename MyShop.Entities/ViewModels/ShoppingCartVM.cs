using MyShop.Entities.Models;

namespace MyShop.Entities.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> CartsList { get; set; }
        public decimal TotalCarts { get; set; }
    }
}
