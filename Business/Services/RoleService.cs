using Business.Model;
using DataAccess.Results;
using DataAccess.Results.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Business.Services.Bases;

namespace Business.Services
{
    public interface IRoleService
    {
        IQueryable<RoleModel> Query();
        Result Add(RoleModel model);
        Result Update(RoleModel model);
        Result Delete(int id);
    }

    public class RoleService : ServiceBase, IRoleService
    {
        public RoleService(Db db) : base(db)
        {
        }
        public IQueryable<RoleModel> Query()
        {
            return _db.Roles.Include(r => r.Users).OrderBy(r => r.Name).Select(r => new RoleModel()
            {
                Id = r.Id,
                Name = r.Name,

                UserCountOutput = r.Users.Count
            });
        }

        public Result Add(RoleModel model)
        {
           var nameSqlParameter = new SqlParameter("name", model.Name.Trim()); 

            var query = _db.Roles.FromSqlRaw("select * from Roles where UPPER(Name) = UPPER(@name)", nameSqlParameter);
            if (query.Any()) 
                return new ErrorResult("Role with the same name already exists!");

            var entity = new Role()
            {
                Name = model.Name.Trim()
            };
            _db.Roles.Add(entity);
            _db.SaveChanges();
            return new SuccessResult("Role added successfully.");
        }

        public Result Update(RoleModel model)
        {
            var nameSqlParameter = new SqlParameter("name", model.Name.Trim()); 
            var idSqlParameter = new SqlParameter("id", model.Id);
            var query = _db.Roles.FromSqlRaw("select * from Roles where UPPER(Name) = UPPER(@name) and Id != @id", nameSqlParameter, idSqlParameter);
            if (query.Any())
                return new ErrorResult("Role with the same name already exists!");

            var entity = new Role()
            {
                Id = model.Id,
                Name = model.Name.Trim()
            };

            _db.Roles.Update(entity);
            _db.SaveChanges();
            return new SuccessResult("Role updated successfully.");
        }

        public Result Delete(int id)
        {
            var existingEntity = _db.Roles.Include(r => r.Users).SingleOrDefault(r => r.Id == id);
            if (existingEntity is null)
                return new ErrorResult("Role not found!");

            if (existingEntity.Users.Any())
                return new ErrorResult("Role can't be deleted because it has users!");

            _db.Roles.Remove(existingEntity);
            _db.SaveChanges();
            return new SuccessResult("Role deleted successfully.");
        }
    }
}