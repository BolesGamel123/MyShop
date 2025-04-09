using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModels;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM shoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value, Includeword: "Product")
            };

            foreach (var cart in shoppingCartVM.CartsList)
            {
                shoppingCartVM.TotalCarts += (cart.Count * cart.Product.Price);
            }
            return View(shoppingCartVM);
        }


        public IActionResult Plus(int cartid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.FirstOrDefault(x => x.Id == cartid);
            _unitOfWork.ShoppingCart.IncreaseCount(shoppingcart, 1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int cartid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.FirstOrDefault(x => x.Id == cartid);

            if (shoppingcart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingcart);
                _unitOfWork.Complete();
                return RedirectToAction("Index", "Home");
                //var count = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == shoppingcart.ApplicationUserId).ToList().Count() - 1;
                // HttpContext.Session.SetInt32(SD.SessionKey, count);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecreaseCount(shoppingcart, 1);

            }
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int cartid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.FirstOrDefault(x => x.Id == cartid);
            _unitOfWork.ShoppingCart.Remove(shoppingcart);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
