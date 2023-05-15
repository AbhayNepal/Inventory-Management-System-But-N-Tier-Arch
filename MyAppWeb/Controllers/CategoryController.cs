using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;


namespace MyAppWeb.Controllers
{
    public class CategoryController : Controller
    {
         private IUnitOfWork _unitofWork;

        public CategoryController(IUnitOfWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _unitofWork.Category.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitofWork.Category.GetT(x=>x.Id == id);
            return View(category);
        }
        //edit post
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.Category.Update(category);
                _unitofWork.Save();
                TempData["success"] = "Data Updated !!";
                return RedirectToAction("Index");
            }
            return (View(category));
        }

        //delet get
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitofWork.Category.GetT(x=> x.Id == id);
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
                TempData["success"] = "Deleted !!";
                return RedirectToAction("Index");
            }
            return (View(category));
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
            if(ModelState.IsValid){
                _unitofWork.Category.Add(category);
                _unitofWork.Save();
                TempData["success"] = "Data Added !!";
                return RedirectToAction("Index");
            }
            return (View(category));
        }

    }
}
