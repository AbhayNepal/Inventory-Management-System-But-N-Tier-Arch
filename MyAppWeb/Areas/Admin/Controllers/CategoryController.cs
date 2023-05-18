using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModels;

namespace MyAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitofWork;

        public CategoryController(IUnitOfWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.categories = _unitofWork.Category.GetAll();
            return View(categoryVM);
        }

       
        //delet get
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitofWork.Category.GetT(x => x.Id == id);
            return View(category);
        }
        //delete post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category category)
        {
            if (ModelState.IsValid)
            {

                _unitofWork.Category.Delete(category);
                _unitofWork.Save();
                TempData["success"] = "Data Deleted!!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            CategoryVM vm = new CategoryVM();
            if(id == null || id == 0) //id =0 for create
            {
                return View(vm);
            }
            else
            {
                vm.Category = _unitofWork.Category.GetT(x => x.Id == id); //id = 1 for update
                if (vm.Category == null)
                {
                    return NotFound();
                }
                else {
                return View(vm);//Editcategory 
                }

            }
            
        }

        //[HttpGet]
        //public IActionResult Create()
        //{


        //    return View();
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(CategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                if(vm.Category.Id == 0) { 
                _unitofWork.Category.Add(vm.Category);
                    TempData["success"] = "Data Created Successfully!!";
                }
                else
                {
                    _unitofWork.Category.Update(vm.Category);
                    TempData["success"] = "Data Updated !!";
                }
                _unitofWork.Save();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

    }
}
