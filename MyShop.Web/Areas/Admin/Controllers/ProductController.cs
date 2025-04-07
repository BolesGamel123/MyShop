using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModels;


namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData()
        {
            var products = _unitOfWork.Product.GetAll(Includeword: "Category");
            return Json(new { data = products });
        }


        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductVM
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM ProductVM, IFormFile File)
        {
            if (ModelState.IsValid)
            {

                string RootPath = _webHostEnvironment.WebRootPath;
                if (File != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(File.FileName);

                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        File.CopyTo(filestream);
                    }
                    ProductVM.Product.Img = @"Images\Products\" + filename + ext;
                }

                _unitOfWork.Product.Add(ProductVM.Product);
                _unitOfWork.Complete();
                TempData["Create"] = "item has created Successfully";
                return RedirectToAction("Index");

            }
            return View(ProductVM.Product);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var model = new ProductVM
            {
                Product = _unitOfWork.Product.FirstOrDefault(x => x.Id == id),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM ProductVM, IFormFile? File)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;

                if (File != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(File.FileName);
                    // حذف الصورة القديمة إذا كانت موجودة
                    if (!string.IsNullOrEmpty(ProductVM.Product.Img))
                    {
                        string oldFilePath = Path.Combine(RootPath, ProductVM.Product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        File.CopyTo(filestream);
                    }
                    ProductVM.Product.Img = @"Images\Products\" + filename + ext;
                }


                _unitOfWork.Product.Update(ProductVM.Product);
                _unitOfWork.Complete();
                TempData["Update"] = "item has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(ProductVM.Product);
        }



        [HttpPost]
        public IActionResult DeleteProduct(int? id)
        {
            var ProductIndb = _unitOfWork.Product.FirstOrDefault(x => x.Id == id);
            if (ProductIndb == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }
            _unitOfWork.Product.Remove(ProductIndb!);
            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, ProductIndb.Img.TrimStart('\\'));
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            _unitOfWork.Complete();
            return Json(new { success = true, message = "Product deleted successfully." });
        }

    }
}
