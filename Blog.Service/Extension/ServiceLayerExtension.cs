using System.Globalization;
using Blog.Service.Services.Abstract;
using Blog.Service.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Blog.Service.FluentValidations;
using Blog.Service.Helpers.Images;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Extension
{
    public static class ServiceLayerExtension
    {
        public static IServiceCollection LoadServiceLayerExtension(this IServiceCollection services)
            
        {
	        services.AddScoped<IArticleService, ArticleService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IImageHelper,ImageHelper>();
			var assembly = Assembly.GetExecutingAssembly();
			services.AddAutoMapper(assembly);
			services.AddControllersWithViews()
				.AddFluentValidation(opt =>
				{
					opt.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
					opt.DisableDataAnnotationsValidation = true;
					opt.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
				});
			

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			return services;
        }
    }
}
