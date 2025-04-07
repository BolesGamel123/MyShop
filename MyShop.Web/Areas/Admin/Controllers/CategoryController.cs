using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;


namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();
                TempData["Create"] = "item has created Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var category = _unitOfWork.Category.FirstOrDefault(x=>x.Id==id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {

                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();
                TempData["Update"] = "item has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var category = _unitOfWork.Category.FirstOrDefault(x=>x.Id==id);
            return View(category);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            var categoryIndb = _unitOfWork.Category.FirstOrDefault(x => x.Id == id);
            if (categoryIndb == null)
            {
                NotFound();
            }
            _unitOfWork.Category.Remove(categoryIndb!);
            _unitOfWork.Complete();
            TempData["Delete"] = "item has Deleted Successfully";
            return RedirectToAction("Index");
        }

    }
}
