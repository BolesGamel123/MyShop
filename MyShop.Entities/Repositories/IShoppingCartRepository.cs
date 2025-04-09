using MyShop.Entities.Models;

namespace MyShop.Entities.Repositories
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        int IncreaseCount(ShoppingCart shoppingCart, int Count);
        int DecreaseCount(ShoppingCart shoppingCart, int Count);
    }
}
