using System.Security.Cryptography.X509Certificates;
using Blog.Data.Context;
using Blog.Data.Extensions;
using Blog.Entity.Entities;
using Blog.Service.Describer;
using Blog.Service.Extension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadServiceLayerExtension();
builder.Services.LoadDataLayerExtension(builder.Configuration);

builder.Services.AddSession();
builder.Services.AddControllersWithViews()
	.AddNToastNotifyToastr(new ToastrOptions()
	{
		ProgressBar = false,
		PositionClass = ToastPositions.TopCenter,
		TimeOut = 3000
    })
	.AddRazorRuntimeCompilation();

//builder.Services.AddIdentity<AppUser, AppRole>(opt =>
//	{
//		opt.Password.RequireLowercase = false;
//		opt.Password.RequireUppercase = false;
//	})
//	.AddRoleManager<RoleManager<AppRole>>()
//	.AddEntityFrameworkStores<AppDbContext>()
//	.AddDefaultTokenProviders();

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
	{
		opt.Password.RequireNonAlphanumeric = false;
		opt.Password.RequireLowercase = false;
		opt.Password.RequireUppercase = false;
	})
	.AddRoleManager<RoleManager<AppRole>>()
	.AddErrorDescriber<CustomIdentityErrorDescriber>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(config =>
{
	config.LoginPath = new PathString("/Admin/Auth/Login");
	config.LogoutPath = new PathString("/Admin/Auth/Logout");
	config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");
	config.ExpireTimeSpan = TimeSpan.FromDays(1);
	config.SlidingExpiration = true;
	config.Cookie = new CookieBuilder()
	{
		HttpOnly = true,
		SameSite = SameSiteMode.Strict,
		Name = "Blog",
		SecurePolicy = CookieSecurePolicy.SameAsRequest
	};
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseNToastNotify();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapAreaControllerRoute(
		name: "Admin",
		areaName: "Admin",
		pattern: "Admin/{controller=Home}/{action=Index}/{Id?}"
	);
	endpoints.MapDefaultControllerRoute();
});


app.Run();
