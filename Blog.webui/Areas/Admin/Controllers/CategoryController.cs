using AutoMapper;
using Blog.Entity.DTOs.Categories;
using Blog.Entity.Entities;
using Blog.Service.Services.Abstract;
using Blog.Webui.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.Webui.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly IValidator<Category> _validator;
		private readonly IMapper _mapper;
		private readonly IToastNotification _toastNotification;


		public CategoryController(ICategoryService categoryService,IValidator<Category> validator,IMapper mapper,IToastNotification toastNotification)
		{
			_categoryService = categoryService;
			_validator = validator;
			_mapper = mapper;
			_toastNotification = toastNotification;
		}
		public async Task<IActionResult> Index()
		{
			var categories = await _categoryService.GetAllCategoriesNonDeleted();
			return View(categories);
		}

		//public async Task<IActionResult> Add(CategpryAddDTO categpryAddDto)
		//{


		//}
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Add(CategoryAddDTO categoryAddDto)
		{
			var map = _mapper.Map<Category>(categoryAddDto);
			var result =  await _validator.ValidateAsync(map);

			if (result.IsValid)
			{
				await _categoryService.CreateCategoryAsync(categoryAddDto);
				_toastNotification.AddSuccessToastMessage(Messages.Category.Add(categoryAddDto.Name),new ToastrOptions(){Title = "İşlem Başarılı"});
				return RedirectToAction("Index", "Category", new { Area = "Admin" });

			}
			else
			{
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> AddWithAjax([FromBody] CategoryAddDTO categoryAddDto)
		{
			var map = _mapper.Map<Category>(categoryAddDto);
			var result = await _validator.ValidateAsync(map);
			if (result.IsValid)
			{
				await _categoryService.CreateCategoryAsync(categoryAddDto);
				_toastNotification.AddSuccessToastMessage(Messages.Category.Add(categoryAddDto.Name), new ToastrOptions() { Title = "İşlem Başarılı" });

				return Json(Messages.Category.Add(categoryAddDto.Name));
			}
			else
			{
				_toastNotification.AddErrorToastMessage(result.Errors.First().ErrorMessage, new ToastrOptions() { Title = "İşlem Başarısız" });
				return Json(result.Errors.First().ErrorMessage);
			}
		}

		[HttpGet]
		public async Task<IActionResult> Update(Guid categoryId)
		{
			var category = await _categoryService.GetByGuidId(categoryId);
			var map = _mapper.Map<Category,CategoryUpdateDTO>(category);

			return View(map);
		}

		[HttpGet]
		public async Task<IActionResult> DeletedCategory()
		{
			var categories = await _categoryService.GetAllCategoriesDeleted();
			var map = _mapper.Map<List<CategoryDTO>>(categories);
			return View(map);
		}


		[HttpPost]
		public async Task<IActionResult> Update(CategoryUpdateDTO updateDto)
		{
			var map = _mapper.Map<Category>(updateDto);
			var result = await _validator.ValidateAsync(map);

			if (result.IsValid)
			{
				await _categoryService.UpdateCategoryAsync(updateDto);
				_toastNotification.AddSuccessToastMessage(Messages.Category.Update(updateDto.Name),
					new ToastrOptions(){Title = updateDto.Name});
				return RedirectToAction("Index", "Category", new { Area = "Admin" });
			}
			else
			{
				return View();
			}

		}

		public async Task<IActionResult> Delete(Guid categoryId)
		{
			await _categoryService.SafeDeleteCategory(categoryId);
			return RedirectToAction("Index", "Category", new { Area = "Admin" });

		}

		public async Task<IActionResult> UndoDelete(Guid categoryId)
		{
			await _categoryService.UndoDeleteCategory(categoryId);
			return RedirectToAction("Index", "Category", new { Area = "Admin" });
		}
	}
}
