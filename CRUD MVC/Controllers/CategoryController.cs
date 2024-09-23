using CRUD_MVC.Data;
using CRUD_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace CRUD_MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CategoryController(ApplicationDBContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;  
        }
        public IActionResult Index()
        {
            List<Category> objectList = _db.Categories.ToList();
            return View(objectList);
        }
        public IActionResult ViewPdf(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null || string.IsNullOrEmpty(category.FileUpload))
            {
                return RedirectToAction("Index");
            }

            var filePath = Path.Combine(_hostEnvironment.WebRootPath, category.FileUpload.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            return File(System.IO.File.ReadAllBytes(filePath), "application/pdf");
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj, IFormFile? file)
        {
            // Basic validation checks
            if (string.IsNullOrEmpty(obj.Name))
            {
                ModelState.AddModelError("Name", "The name cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(obj.Description))
            {
                ModelState.AddModelError("Description", "The description cannot be null or empty.");
            }

            if (obj.Name?.ToLower() == "test")
            {
                ModelState.AddModelError("Name", "The name cannot be 'test'.");
            }

            // If validation is passed, save to database
            if (ModelState.IsValid)
            {
                // File upload logic
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", fileName);

                    // Save the file to wwwroot/uploads
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // Store the file path in the object to save in the database
                    obj.FileUpload = "/uploads/" + fileName;
                }

                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);  // Return to the view with validation errors
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? obj = _db.Categories.Find(id);
            Category? obj1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            Category? obj2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
           
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? obj = _db.Categories.Find(id);
          
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category obj = _db.Categories.Find(id);
            if (obj == null) {
                return NotFound();
                    }
            _db.Categories.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
       
        }
        public static void Log(string msg)
        {
            Debug.WriteLine(msg);
}
        public static void Log2(string msg)
        {
            Debug.WriteLine(msg);
        }

    }
}



