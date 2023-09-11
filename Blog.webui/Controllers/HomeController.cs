using Blog.webui.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Blog.Service.Services.Abstract;

namespace Blog.webui.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IArticleService _articleService;


		public HomeController(ILogger<HomeController> logger, IArticleService articleService)
		{
			_logger = logger;
			_articleService = articleService;

		}

		public async Task<IActionResult> Index()
		{

			var item = await _articleService.GetAllArticleWithCategoryNonDeletedAsync();
			return View(item);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}