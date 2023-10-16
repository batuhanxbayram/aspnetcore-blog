using Blog.Service.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Webui.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IArticleService _articleService;
		private readonly IDashBoardService _dashBoardService;

		public HomeController(IArticleService articleService,IDashBoardService dashBoardService)
		{
			_articleService = articleService;
			_dashBoardService = dashBoardService;
		}
		public async Task<IActionResult> Index()
		{
			var articles =await _articleService.GetAllArticleWithCategoryNonDeletedAsync();
			return View(articles);
		}

		[HttpGet]
		public async Task<IActionResult> YearlyArticleCounts()
		{
			var count = await _dashBoardService.GetYearlyArticleAsync();
			return Json(JsonConvert.SerializeObject(count));
		}

		[HttpGet]
		public async Task<IActionResult> TotalArticlesCount()
		{
			var count = await _dashBoardService.GetTotalArticleCountAsync();
			return Json(count);
		}
		[HttpGet]
		public async Task<IActionResult> TotalCategoreiesCount()
		{
			var count = await _dashBoardService.GetTotalCategoryCountAsync();
			return Json(count);
		}
		


	}
}
