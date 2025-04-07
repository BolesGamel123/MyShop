using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModels;

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

        public IActionResult Details(int id)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = _unitOfWork.Product.FirstOrDefault(x => x.Id == id, Includeword: "Category"),
                Count = 1
            };
            return View(shoppingCart);
        }
    }
}
