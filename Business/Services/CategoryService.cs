using Business.Model;
using Business.Services.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Results;
using DataAccess.Results.Bases;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public interface ICategoryService
    {
        IQueryable<CategoryModel> Query();
        Result Add(CategoryModel model);
        Result Update(CategoryModel model);
        Result Delete(int id);
    }

    public class CategoryService : ServiceBase, ICategoryService
    {
        public CategoryService(Db db) : base(db)
        {
        }

        // Read
        public IQueryable<CategoryModel> Query()
        {
            return _db.Categories.Include(c => c.Blogs).OrderBy(c => c.Name).Select(c => new CategoryModel()
            {
                Id = c.Id,
                Name = c.Name,

                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                BlogIds = c.Blogs.Select(b => b.Id).ToList(),

                BlogCountOutput = c.Blogs.Count
            });
        }

        // Create
        public Result Add(CategoryModel model)
        {
            if (_db.Categories.Any(c => c.Name.ToLower() == model.Name.ToLower().Trim()))
                return new ErrorResult("Category with the same name exists!");

            var category = new Category
            {
                Name = model.Name.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Categories.Add(category);
            _db.SaveChanges();

            model.Id = category.Id;
            model.CreatedAt = category.CreatedAt;
            model.UpdatedAt = (DateTime)category.UpdatedAt;

            return new SuccessResult("Category added successfully.");
        }

        //Update
        public Result Update(CategoryModel model)
        {
            var category = _db.Categories.Find(model.Id);
            if (category == null)
                return new ErrorResult("Category not found!");

            if (_db.Categories.Any(c => c.Id != model.Id && c.Name.ToLower() == model.Name.ToLower().Trim()))
                return new ErrorResult("Category with the same name exists!");

            category.Name = model.Name.Trim();
            category.UpdatedAt = DateTime.UtcNow;

            _db.SaveChanges();

            model.UpdatedAt = (DateTime)category.UpdatedAt;

            return new SuccessResult("Category updated successfully.");
        }

        public Result Delete(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
                return new ErrorResult("Category not found!");

            _db.Categories.Remove(category);
            _db.SaveChanges();

            return new SuccessResult("Category deleted successfully.");
        }

    }
}
