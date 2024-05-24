using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
	public class Db : DbContext
	{
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<BlogTag> BlogTags { get; set; }
		public DbSet<Role> Roles { get; set; }


		public Db(DbContextOptions<Db> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); // Bu çağrı Identity ile ilgili yapılandırmaları ekler.

			// One-to-many relationship
			modelBuilder.Entity<Blog>()
				.HasOne(b => b.Category)
				.WithMany(c => c.Blogs)
				.HasForeignKey(b => b.CategoryId);

			// Many-to-many relationship
			modelBuilder.Entity<BlogTag>()
				.HasKey(bt => new { bt.BlogId, bt.TagId });
			modelBuilder.Entity<BlogTag>()
				.HasOne(bt => bt.Blog)
				.WithMany(b => b.BlogTags)
				.HasForeignKey(bt => bt.BlogId);
			modelBuilder.Entity<BlogTag>()
				.HasOne(bt => bt.Tag)
				.WithMany(t => t.BlogTags)
				.HasForeignKey(bt => bt.TagId);
		}
	}
}
