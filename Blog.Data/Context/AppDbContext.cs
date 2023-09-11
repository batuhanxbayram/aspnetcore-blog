
using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blog.Data.Context
{
	public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Article> Articles { get; set; }
		public DbSet<Category> Categories { get; set; }
		
		

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		}
	}
}
