using Microsoft.AspNetCore.Mvc;
using MyAppWeb.Data;
using MyAppWeb.Models;

namespace MyAppWeb.Controllers
{
    public class CategoryController : Controller
    {
         private ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _context.Categories;
            return View(categories);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var category = _context.Categories.Find(id);
            return View(category);
        }
        //edit post
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
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
            var category = _context.Categories.Find(id);
            return View(category);
        }
        //delete post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category category)
        {
            if (ModelState.IsValid)
            {
                
                _context.Categories.Remove(category);
                _context.SaveChanges();
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
                _context.Categories.Add(category);
                _context.SaveChanges();
                TempData["success"] = "Data Added !!";
                return RedirectToAction("Index");
            }
            return (View(category));
        }

    }
}
