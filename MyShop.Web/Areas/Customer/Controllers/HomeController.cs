using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var Products = _unitOfWork.Product.GetAll();
            return View(Products);
        }

        public IActionResult Details(int ProductId)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                ProductId = ProductId,
                Product = _unitOfWork.Product.FirstOrDefault(x => x.Id == ProductId, Includeword: "Category"),
                Count = 1
            };
            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            var cartFromDb = _unitOfWork.ShoppingCart.FirstOrDefault
                (x => x.ApplicationUserId == shoppingCart.ApplicationUserId && x.ProductId == shoppingCart.ProductId);

            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);

            }
            else
            {
                _unitOfWork.ShoppingCart.IncreaseCount(cartFromDb, shoppingCart.Count);
            }

            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
