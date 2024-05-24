#nullable disable
using Business.Model;
using Business.Services;
using DataAccess.Results.Bases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

// Generated from Custom Template.
namespace MVC.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;

        public BlogsController(IBlogService blogService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
        }

        // GET: Blogs
        public IActionResult Index()
        {
            List<BlogModel> blogList = _blogService.Query().ToList();
            return View(blogList);
        }

        // GET: Blogs/Details/5
        public IActionResult Details(int id)
        {
            BlogModel blog = _blogService.Query().SingleOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            return View();
        }

        // POST: Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogModel blog)
        {
            if (ModelState.IsValid)
            {
                Result result = _blogService.Add(blog);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", blog.CategoryId);
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public IActionResult Edit(int id)
        {
            BlogModel blog = _blogService.Query().SingleOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", blog.CategoryId);
            return View(blog);
        }

        // POST: Blogs/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BlogModel blog)
        {
            if (ModelState.IsValid)
            {
                Result result = _blogService.Update(blog);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", blog.CategoryId);
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public IActionResult Delete(int id)
        {
            BlogModel blog = _blogService.Query().SingleOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Result result = _blogService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
