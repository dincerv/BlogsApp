#nullable disable
using Business.Model;
using Business.Services;
using DataAccess.Results.Bases;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

//Generated from Custom Template.
namespace MVC.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: Tags
        public IActionResult Index()
        {
            List<TagModel> tagList = _tagService.Query().ToList();
            return View(tagList);
        }

        // GET: Tags/Details/5
        public IActionResult Details(int id)
        {
            TagModel tag = _tagService.Query().SingleOrDefault(t => t.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TagModel tag)
        {
            if (ModelState.IsValid)
            {
                Result result = _tagService.Add(tag);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(tag);
        }

        // GET: Tags/Edit/5
        public IActionResult Edit(int id)
        {
            TagModel tag = _tagService.Query().SingleOrDefault(t => t.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TagModel tag)
        {
            if (ModelState.IsValid)
            {
                Result result = _tagService.Update(tag);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public IActionResult Delete(int id)
        {
            TagModel tag = _tagService.Query().SingleOrDefault(t => t.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tags/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Result result = _tagService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
