#nullable disable
using Business.Model;
using Business.Services;
using DataAccess.Results.Bases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Controllers.Bases;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers
{
    public class BlogTagsController : MvcController
    {
        private readonly IBlogTagService _blogTagService;
        private readonly IBlogService _blogService;
        private readonly ITagService _tagService;
        public BlogTagsController(IBlogTagService blogTagService, IBlogService blogService, ITagService tagService)
        {
            _blogTagService = blogTagService;
            _blogService = blogService;
            _tagService = tagService;
        }

        public IActionResult Index()
        {
            List<BlogTagModel> blogTagList = _blogTagService.Query().ToList();
            return View(blogTagList);
        }

        public IActionResult Details(int blogId, int tagId)
        {
            BlogTagModel blogTag = _blogTagService.Query().SingleOrDefault(bt => bt.BlogId == blogId && bt.TagId == tagId);
            if (blogTag == null)
            {
                return NotFound();
            }
            return View(blogTag);
        }

        public IActionResult Create()
        {
            ViewData["BlogId"] = new SelectList(_blogService.Query().ToList(), "Id", "Title");
            ViewData["TagId"] = new SelectList(_tagService.Query().ToList(), "Id", "Name");
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BlogTagModel blogTag)
		{
			if (ModelState.IsValid)
			{
				Result result = await _blogTagService.Add(blogTag); // Asenkron çağrı
				if (result.IsSuccessful)
				{
					TempData["Message"] = result.Message;
					return RedirectToAction(nameof(Index));
				}
				ModelState.AddModelError("", result.Message);
			}
			ViewData["BlogId"] = new SelectList(_blogService.Query().ToList(), "Id", "Title", blogTag.BlogId);
			ViewData["TagId"] = new SelectList(_tagService.Query().ToList(), "Id", "Name", blogTag.TagId);
			return View(blogTag);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int blogId, int tagId)
		{
			Result result = await _blogTagService.Delete(blogId, tagId); // Asenkron çağrı
			TempData["Message"] = result.Message;
			return RedirectToAction(nameof(Index));
		}


		public IActionResult Edit(int blogId, int tagId)
        {
            BlogTagModel blogTag = _blogTagService.Query().SingleOrDefault(bt => bt.BlogId == blogId && bt.TagId == tagId);
            if (blogTag == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_blogService.Query().ToList(), "Id", "Title", blogTag.BlogId);
            ViewData["TagId"] = new SelectList(_tagService.Query().ToList(), "Id", "Name", blogTag.TagId);
            return View(blogTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BlogTagModel blogTag)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update service logic here
                return RedirectToAction(nameof(Index));
            }
            // TODO: Add get related items service logic here to set ViewData if necessary
            ViewData["BlogId"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            ViewData["TagId"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            return View(blogTag);
        }


        public IActionResult Delete(int blogId, int tagId)
        {
            BlogTagModel blogTag = _blogTagService.Query().SingleOrDefault(bt => bt.BlogId == blogId && bt.TagId == tagId);
            if (blogTag == null)
            {
                return NotFound();
            }
            return View(blogTag);
        }
    }
}