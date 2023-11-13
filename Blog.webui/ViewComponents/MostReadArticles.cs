using Blog.Service.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Webui.ViewComponents
{
    public class MostReadArticles: ViewComponent
    {
        public IArticleService ArticleService { get; set; }
        public MostReadArticles(IArticleService articleService)
        {
            ArticleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var articles = await ArticleService.GetMostReadArticles();

            return View(articles);
        }

        
    }
}
