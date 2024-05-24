using Business.Model;
using Business.Services.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Results;
using DataAccess.Results.Bases;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Business.Services
{
	public interface IBlogTagService
	{
		IQueryable<BlogTagModel> Query();
		//Result Add(BlogTagModel model);
		Task<Result> Add(BlogTagModel model);
		Task<Result> Delete(int blogId, int tagId);
	}

	public class BlogTagService : ServiceBase,IBlogTagService
	{
        public BlogTagService(Db db) : base(db)
        {
        
		}

		// Read
		public IQueryable<BlogTagModel> Query()
		{
			return _db.BlogTags.Include(bt => bt.Blog).Include(bt => bt.Tag)
				.Select(bt => new BlogTagModel()
				{
					Id = bt.Id,
					BlogId = bt.BlogId,
					TagId = bt.TagId,
					Blog = new BlogModel
					{
						Id = bt.Blog.Id,
						Title = bt.Blog.Title,
						Content = bt.Blog.Content,
						Rating = bt.Blog.Rating,
						CategoryId = bt.Blog.CategoryId,
						PublishedDate = bt.Blog.PublishedDate,
						CreatedAt = bt.Blog.CreatedAt,
						UpdatedAt = bt.Blog.UpdatedAt
					},
					Tag = new TagModel
					{
						Id = bt.Tag.Id,
						Name = bt.Tag.Name,
						IsPopular = bt.Tag.IsPopular,
						CreatedAt = bt.Tag.CreatedAt,
						UpdatedAt = bt.Tag.UpdatedAt
					}
				});
		}

		// Create
		/*public Result Add(BlogTagModel model)
		{
			if (_db.BlogTags.Any(bt => bt.BlogId == model.BlogId && bt.TagId == model.TagId))
				return new ErrorResult("This blog tag association already exists!");

			var blogTag = new BlogTag
			{
				BlogId = model.BlogId,
				TagId = model.TagId
			};

			_db.BlogTags.Add(blogTag);
			_db.SaveChanges();

			model.Id = blogTag.Id;

			return new SuccessResult("Blog tag association added successfully.");
		}*/

		public async Task<Result> Add(BlogTagModel model)
		{
			if (await _db.BlogTags.AnyAsync(bt => bt.BlogId == model.BlogId && bt.TagId == model.TagId))
				return new ErrorResult("This blog tag association already exists!");

			var blogTag = new BlogTag
			{
				BlogId = model.BlogId,
				TagId = model.TagId
			};

			_db.BlogTags.Add(blogTag);
			await _db.SaveChangesAsync();

			model.Id = blogTag.Id;

			return new SuccessResult("Blog tag association added successfully.");
		}


		// Delete
		public async Task<Result> Delete(int blogId, int tagId)
		{
			var blogTag = await _db.BlogTags.FirstOrDefaultAsync(bt => bt.BlogId == blogId && bt.TagId == tagId);
			if (blogTag == null)
			{
				return new ErrorResult("Blog tag association not found!");
			}

			_db.BlogTags.Remove(blogTag);
			await _db.SaveChangesAsync();

			return new SuccessResult("Blog tag association deleted successfully.");
		}
	}
}
