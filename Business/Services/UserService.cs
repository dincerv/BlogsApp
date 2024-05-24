using Business.Model;
using Business.Services.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Results;
using DataAccess.Results.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public interface IUserService
    {
        IQueryable<UserModel> Query();
        Result Add(UserModel model);
        Result Update(UserModel model);
        Result Delete(int id);
        List<UserModel> GetList();
        UserModel GetItem(int id);
		Task<Result> AddAsync(UserModel user);
	}

    public class UserService : ServiceBase, IUserService
    {
        public UserService(Db db) : base(db)
        {
        }

        // Read
        public IQueryable<UserModel> Query()
        {
            return _db.Users.Include(u => u.Blogs)
                .OrderBy(u => u.Username)
                .Select(u => new UserModel()
                {
                    Id = u.Id,
                    Username = u.Username,
                    Password = u.Password,
                    IsActive = u.IsActive,
                    Gender = u.Gender,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                    BlogIds = u.Blogs.Select(b => b.Id).ToList(),
                    BlogCountOutput = u.Blogs.Count,
                    BlogTitlesOutput = string.Join(", ", u.Blogs.Select(b => b.Title)),
                    IsActiveOutput = u.IsActive ? "Active" : "Not Active"
                });    
        }

        // Create 
        public Result Add(UserModel model)
        {
            try
            {
                if (_db.Users.Any(u => u.Username.ToLower() == model.Username.ToLower().Trim()))
                    return new ErrorResult("User with the same username exists!");

                var user = new User
                {
                    Username = model.Username.Trim(),
                    Password = model.Password,
                    IsActive = model.IsActive,
                    Gender = model.Gender,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                model.Id = user.Id;
                model.CreatedAt = user.CreatedAt;
                model.UpdatedAt = user.UpdatedAt;

                return new SuccessResult("User added successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Log the error or inspect ex.InnerException for more details
                return new ErrorResult($"An error occurred while adding the user: {ex.Message}");
            }
        }

        // Update
        public Result Update(UserModel model)
        {
            try
            {
                var user = _db.Users.Find(model.Id);
                if (user == null)
                    return new ErrorResult("User not found!");

                if (_db.Users.Any(u => u.Id != model.Id && u.Username.ToLower() == model.Username.ToLower().Trim()))
                    return new ErrorResult("User with the same username exists!");

                user.Username = model.Username.Trim();
                user.Password = model.Password;
                user.IsActive = model.IsActive;
                user.Gender = model.Gender;
                user.UpdatedAt = DateTime.UtcNow;

                _db.SaveChanges();

                model.UpdatedAt = user.UpdatedAt;

                return new SuccessResult("User updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Log the error or inspect ex.InnerException for more details
                return new ErrorResult($"An error occurred while updating the user: {ex.Message}");
            }
        }

        // Delete
        public Result Delete(int id)
        {
            try
            {
                var user = _db.Users.Include(u => u.Blogs).SingleOrDefault(u => u.Id == id);
                if (user == null)
                    return new ErrorResult("User not found!");

                // Remove related blogs if necessary or ensure cascading delete is enabled
                _db.Users.Remove(user);
                _db.SaveChanges();

                return new SuccessResult("User deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Log the error or inspect ex.InnerException for more details
                return new ErrorResult($"An error occurred while deleting the user: {ex.Message}");
            }
        }

        // Get List
        public List<UserModel> GetList()
        {
            return Query().ToList();
        }

        // Get Item
        public UserModel GetItem(int id)
        {
            return Query().SingleOrDefault(u => u.Id == id);
        }

		public Task<Result> AddAsync(UserModel user)
		{
			throw new NotImplementedException();
		}
	}
}
