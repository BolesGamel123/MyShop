using MyShop.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Entities.ViewModels
{
    public class ShoppingCart
    {
        public Product Product { get; set; }

        [Range(1, 100, ErrorMessage = "You must enter value between 1 to 100")]
        public int Count { get; set; }
    }
}
