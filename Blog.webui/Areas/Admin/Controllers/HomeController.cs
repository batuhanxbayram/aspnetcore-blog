using Blog.Service.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Webui.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IArticleService _articleService;

		public HomeController(IArticleService articleService)
		{
			_articleService = articleService;
		}
		public async Task<IActionResult> Index()
		{
			var articles =await _articleService.GetAllArticleWithCategoryNonDeletedAsync();
			return View(articles);
		}
	}
}
