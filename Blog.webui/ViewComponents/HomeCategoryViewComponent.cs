using Blog.Service.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
namespace Blog.Webui.ViewComponents
{
    public class HomeCategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public HomeCategoryViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await categoryService.GetAllCategoriesNonDeletedTake24();

            return View(categories);
        }

    }
}
