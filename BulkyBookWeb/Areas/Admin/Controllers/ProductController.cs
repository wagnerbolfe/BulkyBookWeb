using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            ProductViewModel product = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString() 
                }),
                CoverTyperList = _unitOfWork.CoverType.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
            };

            if (id == null || id == 0)
            {

                return View(product);
            }
            else
            {
                
            }

            return View(product);

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel obj, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            _unitOfWork.Save();
            TempData["success"] = "Product updated successfully";
            return RedirectToAction("Index");
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);

            return View(product);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
