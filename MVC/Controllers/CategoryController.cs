#nullable disable
using Business.Model;
using Business.Services;
using DataAccess.Results.Bases;
using Microsoft.AspNetCore.Mvc;

//Generated from Custom Template.
namespace MVC.Controllers
{
    public class CategoryController : Controller
    {
        // TODO: Add service injections here
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: category
        public IActionResult Index()
        {
            List<CategoryModel> categoryList = _categoryService.Query().ToList(); // TODO: Add get collection service logic here
            return View(categoryList);
        }

        // GET: Details/5
        public IActionResult Details(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(s => s.Id == id); // TODO: Add get item service logic here
            if (category == null)
            {
                return NotFound(); // 404 HTTP Status Code
            }
            return View(category);
        }

        // GET: Create
        public IActionResult Create()
        {
            // TODO: Add get related items service logic here to set ViewData if necessary
            return View();
        }

        // POST: Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add insert service logic here
                Result result = _categoryService.Add(category);
                if (result.IsSuccessful)
                {
                    // Way 1:
                    //return RedirectToAction("Index", "category");
                    // Way 2:
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                // Way 1:
                //ViewData["ViewMessage"] = result.Message;
                // Way 2:
                //ViewBag.ViewMessage = result.Message;
                ModelState.AddModelError("", result.Message);
            }
            // TODO: Add get related items service logic here to set ViewData if necessary
            return View(category);
        }

        // GET: Edit/5
        public IActionResult Edit(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(s => s.Id == id); // TODO: Add get item service logic here
            if (category == null)
            {
                return NotFound();
            }
            // TODO: Add get related items service logic here to set ViewData if necessary
            return View(category);
        }

        // POST: Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryModel categorie)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update service logic here
                Result result = _categoryService.Update(categorie);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", result.Message);
            }
            // TODO: Add get related items service logic here to set ViewData if necessary
            return View(categorie);
        }

        // GET: /Delete/5
        public IActionResult Delete(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(s => s.Id == id); // TODO: Add get item service logic here
            if (category == null)
            {
                return NotFound();
            }
            _categoryService.Delete(category.Id);
            return RedirectToAction("Index");
        }

        // POST: /Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // TODO: Add delete service logic here
            Result result = _categoryService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}