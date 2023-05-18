using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModels;

namespace MyAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitofWork;
        private IWebHostEnvironment _hostingEnvironment;

        public ProductController(IUnitOfWork unitofWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitofWork = unitofWork;
            _hostingEnvironment = hostingEnvironment;
        }

       
        public IActionResult AllProducts()
        {
            var products = _unitofWork.Product.GetAll(includeProperties:"Category"); //category is the name of the table that  you need to include inside of the product table to access it from the products controller
            return Json(new { data = products});
        }   
        

        public IActionResult Index()
        {
            //ProductVM productVM = new ProductVM();
            //productVM.Products = _unitofWork.Product.GetAll();
            return View();
        }



        #region DeleteAPICALL

        //delete post

        [HttpDelete]
 
        public IActionResult Delete(int? id)
        {
            var product = _unitofWork.Product.GetT(x => x.Id == id);
            if(product == null)
            {
                return Json(new { success = false, message = "Error in fetching Data" });
            }
            else
            {
                var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                _unitofWork.Product.Delete(product);
                _unitofWork.Save();
                return Json(new { success = true, message = "Successfully Deleted" });

            }
            
        }
        #endregion DELETEAPICALL
        [HttpGet]
        public  IActionResult CreateUpdate(int? id)
        {
            ProductVM vm = new ProductVM()
            {
                Product = new(),
                Categories = _unitofWork.Category.GetAll().Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }
                )
            };
            if (id == null || id == 0) //id =0 for create
            {
                return View(vm);
            }
            else
            {
                vm.Product = _unitofWork.Product.GetT(x => x.Id == id); //id = 1 for update
                if (vm.Product == null)
                {
                    return NotFound();
                }
                else
                {
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
        public IActionResult CreateUpdate(ProductVM vm, IFormFile? file)//the name file should be the same as the name given in the view
        {
            if (ModelState.IsValid)
            {
                string fileName = String.Empty;
                if (file != null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
                    fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);

                    if (vm.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);    
                        }
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    { 
                        file.CopyTo(fileStream);
                    }
                    vm.Product.ImageUrl = filePath; //or you could do @"\ProductImage\" + fileName;
                }
                if (vm.Product.Id == 0)
                {
                    _unitofWork.Product.Add(vm.Product);
                    TempData["success"] = "Product Data Created";

                }

                else
                {
                    _unitofWork.Product.Update(vm.Product);
                    TempData["success"] = "Product Data Updated !!";
                }
                _unitofWork.Save();
                return RedirectToAction("Index");


            }
            return View(vm);

        }

    }
}

    
