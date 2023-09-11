using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
	public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");

            builder.HasData(new AppUserRole
            {
                UserId = Guid.Parse("CB94223B-CCB8-4F2F-93D7-0DF96A7F065C"),
                RoleId = Guid.Parse("16EA936C-7A28-4C30-86A2-9A9704B6115E")
            }, 
            new AppUserRole
            {
                UserId = Guid.Parse("3AA42229-1C0F-4630-8C1A-DB879ECD0427"),
                RoleId = Guid.Parse("7CB750CF-3612-4FB4-9F7D-A38BA8F16BF4")
            });
        }
    }
}
