using Business.Model;
using Business.Services.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Results;
using DataAccess.Results.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Business.Services
{
	public interface IBlogService
	{
		IQueryable<BlogModel> Query();
		Result Add(BlogModel model);
		Result Update(BlogModel model);
		Result Delete(int id);
	}

	public class BlogService : ServiceBase, IBlogService
	{
		public BlogService(Db db) : base(db)
		{
		}


		// Read
		public IQueryable<BlogModel> Query()
		{
			return _db.Blogs.Include(b => b.Category).Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
				.OrderBy(b => b.Title)
				.Select(b => new BlogModel()
				{
					Id = b.Id,
					Title = b.Title,
					Content = b.Content,
					Rating = b.Rating,
					CategoryId = b.CategoryId,
					PublishedDate = b.PublishedDate,
					CreatedAt = b.CreatedAt,
					UpdatedAt = b.UpdatedAt,
                    TagIds = b.BlogTags.Select(bt => bt.TagId).ToList(),
                    TagNamesOutput = string.Join(", ", b.BlogTags.Select(bt => bt.Tag.Name)),
                    PublishedDateOutput = b.PublishedDate.HasValue ? b.PublishedDate.Value.ToString("yyyy-MM-dd") : "N/A"
                });
		}

        // Create
        public Result Add(BlogModel model)
        {
            if (model.TagIds == null)
                model.TagIds = new List<int>();  // Ensure TagIds is never null

            var blog = new Blog
            {
                Title = model.Title.Trim(),
                Content = model.Content.Trim(),
                Rating = model.Rating,
                CategoryId = model.CategoryId,
                PublishedDate = model.PublishedDate,
                CreatedAt = DateTime.Now,
                BlogTags = new List<BlogTag>()
            };

            _db.Blogs.Add(blog);
            _db.SaveChanges();  // Save to generate Blog ID, assuming this is needed before assigning BlogId

            // Assign BlogId to each BlogTag after the Blog has been saved and has an Id
            blog.BlogTags.AddRange(model.TagIds.Select(id => new BlogTag { BlogId = blog.Id, TagId = id }));
            _db.SaveChanges();  // Save again to store BlogTags


            model.Id = blog.Id;
            model.CreatedAt = blog.CreatedAt;
            model.UpdatedAt = blog.UpdatedAt;

            return new SuccessResult("Blog added successfully.");
        }



        // Update
        public Result Update(BlogModel model)
		{
			var blog = _db.Blogs.Include(b => b.BlogTags).FirstOrDefault(b => b.Id == model.Id);
			if (blog == null)
				return new ErrorResult("Blog not found!");

			blog.Title = model.Title.Trim();
			blog.Content = model.Content.Trim();
			blog.Rating = model.Rating;
			blog.CategoryId = model.CategoryId;
			blog.PublishedDate = model.PublishedDate;
			blog.UpdatedAt = DateTime.Now;

			_db.SaveChanges();

			model.UpdatedAt = (DateTime)blog.UpdatedAt;

			return new SuccessResult("Blog updated successfully.");
		}

		public Result Delete(int id)
		{
			var blog = _db.Blogs.Find(id);
			if (blog == null)
				return new ErrorResult("Blog not found!");

			_db.Blogs.Remove(blog);
			_db.SaveChanges();

			return new SuccessResult("Blog deleted successfully.");
		}
	}
}
