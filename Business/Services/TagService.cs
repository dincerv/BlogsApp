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
	public interface ITagService
	{
		IQueryable<TagModel> Query();
		Result Add(TagModel model);
		Result Update(TagModel model);
		Result Delete(int id);
	}

	public class TagService : ServiceBase,ITagService
	{
        public TagService(Db db) : base(db)
        {
        }

        // Read
        public IQueryable<TagModel> Query()
		{
			return _db.Tags.Include(t => t.BlogTags).ThenInclude(bt => bt.Blog)
				.OrderBy(t => t.Name)
				.Select(t => new TagModel()
				{
					Id = t.Id,
					Name = t.Name,
					IsPopular = t.IsPopular,
					CreatedAt = t.CreatedAt,
					UpdatedAt = t.UpdatedAt,
					BlogIds = t.BlogTags.Select(bt => bt.BlogId).ToList(),
					BlogNamesOutput = string.Join(", ", t.BlogTags.Select(bt => bt.Blog.Title))
				});
		}

		// Create
		public Result Add(TagModel model)
		{
			if (_db.Tags.Any(t => t.Name.ToLower() == model.Name.ToLower().Trim()))
				return new ErrorResult("Tag with the same name exists!");

			var tag = new Tag
			{
				Name = model.Name.Trim(),
				IsPopular = model.IsPopular,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			_db.Tags.Add(tag);
			_db.SaveChanges();

			model.Id = tag.Id;
			model.CreatedAt = tag.CreatedAt;
			model.UpdatedAt = (DateTime)tag.UpdatedAt;

			return new SuccessResult("Tag added successfully.");
		}

		// Update
		public Result Update(TagModel model)
		{
			var tag = _db.Tags.Find(model.Id);
			if (tag == null)
				return new ErrorResult("Tag not found!");

			if (_db.Tags.Any(t => t.Id != model.Id && t.Name.ToLower() == model.Name.ToLower().Trim()))
				return new ErrorResult("Tag with the same name exists!");

			tag.Name = model.Name.Trim();
			tag.IsPopular = model.IsPopular;
			tag.UpdatedAt = DateTime.UtcNow;

			_db.SaveChanges();

			model.UpdatedAt = (DateTime)tag.UpdatedAt;

			return new SuccessResult("Tag updated successfully.");
		}

		public Result Delete(int id)
		{
			var tag = _db.Tags.Find(id);
			if (tag == null)
				return new ErrorResult("Tag not found!");

			_db.Tags.Remove(tag);
			_db.SaveChanges();

			return new SuccessResult("Tag deleted successfully.");
		}
	}
}
