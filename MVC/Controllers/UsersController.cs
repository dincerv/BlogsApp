using Business.Model;
using Business.Services;
using DataAccess.Enums;
using DataAccess.Results.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBlogService _blogService;
        private readonly IRoleService _roleService;

        public UsersController(IUserService userService, IBlogService blogService, IRoleService roleService)
        {
            _userService = userService;
            _blogService = blogService;
            _roleService = roleService;
        }

        [Authorize(Roles = "admin, user")]
        public IActionResult Index()
        {
            List<UserModel> userList = _userService.Query().ToList();
            return View("List", userList);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Details(int id)
        {
            UserModel user = _userService.Query().SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewBag.GenderList = GetGenderSelectList();
            ViewBag.Roles = new SelectList(_roleService.Query().ToList(), "Id", "Name");
            ViewBag.BlogList = GetBlogSelectList();
            return View(new UserModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                Result result = await _userService.AddAsync(user);
                if (result.IsSuccessful)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.GenderList = GetGenderSelectList();
            ViewBag.Roles = new SelectList(_roleService.Query().ToList(), "Id", "Name");
            ViewBag.BlogList = GetBlogSelectList();
            return View(user);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            UserModel user = _userService.Query().SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.GenderList = GetGenderSelectList();
            ViewBag.RoleId = new SelectList(_roleService.Query().ToList(), "Id", "Name");
            ViewBag.BlogList = GetBlogSelectList();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(UserModel user)
        {
            if (ModelState.IsValid)
            {
                Result result = _userService.Update(user);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.GenderList = GetGenderSelectList();
            ViewBag.RoleId = new SelectList(_roleService.Query().ToList(), "Id", "Name");
            ViewBag.BlogList = GetBlogSelectList();
            return View(user);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            UserModel user = _userService.Query().SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            Result result = _userService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        private SelectList GetGenderSelectList()
        {
            var genderList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Male", Value = Gender.Male.ToString() },
                new SelectListItem { Text = "Female", Value = Gender.Female.ToString() },
                new SelectListItem { Text = "Other", Value = Gender.Other.ToString() }
            };
            return new SelectList(genderList, "Value", "Text");
        }

        private SelectList GetBlogSelectList()
        {
            var blogs = _blogService.Query().Select(b => new SelectListItem
            {
                Text = b.Title,
                Value = b.Id.ToString()
            }).ToList();
            return new SelectList(blogs, "Value", "Text");
        }
    }
}
