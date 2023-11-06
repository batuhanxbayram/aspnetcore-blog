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

		public async Task<IActionResult> Index(Guid ? categoryId,int currentPage=1,int pageSize=3,bool isAscending=false)
		{
			var articles = await _articleService.GetAllByPagesAsync(categoryId,currentPage,pageSize,isAscending);

			return View(articles);
		}
		public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
		{
			var articles = await _articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);

			return View(articles);
		}
		public async Task<IActionResult> Detail(Guid Id)
		{
			var article = await _articleService.GetArticleWithCategoryNonDeletedAsync(Id);

			return View(article);

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