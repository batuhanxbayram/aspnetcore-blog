﻿using AutoMapper;
using Blog.Entity.DTOs.Articles;
using Blog.Entity.Entities;
using Blog.Service.Extension;
using Blog.Service.Services.Abstract;
using Blog.Service.Services.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using NToastNotify;
using Blog.Webui.ResultMessages;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Webui.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ArticleController : Controller
	{
		private readonly IArticleService _articleService;
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;
		private readonly IValidator<Article> _validator;
		private readonly IToastNotification _toastNotification;

		public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper,IValidator<Article> validator,IToastNotification toastNotification)
		{
            _articleService = articleService;
            _categoryService = categoryService;
			_mapper = mapper;
			_validator = validator;
			_toastNotification = toastNotification;
		}
		[Authorize(Roles = "Superadmin,Admin,User")]
		public async Task<IActionResult> Index()
		{
			var articles = await _articleService.GetAllArticleWithCategoryNonDeletedAsync();

			return View(articles);
		}
		[HttpGet]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> Add()
		{
			var categories = await _categoryService.GetAllCategoriesNonDeleted();

			return View(new ArticleAddDTO() { Categories = categories });
		}

		[HttpPost]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> Add(ArticleAddDTO articleAddDto)
		{
			var map = _mapper.Map<Article>(articleAddDto);
			var result = await _validator.ValidateAsync(map);
			if (result.IsValid)
			{
				_toastNotification.AddSuccessToastMessage("Makaleniz Başarıyla Eklendi",
					new ToastrOptions() { Title = "Başarılı" });
				await _articleService.CreateArticleAsync(articleAddDto);
				return RedirectToAction("Index", "Article", new { Area = "Admin" });
			}
			else
			{
				result.AddToModelState(this.ModelState);
				var categories = await _categoryService.GetAllCategoriesNonDeleted();
				return View(new ArticleAddDTO() { Categories = categories });
			}
			
		}

		[HttpGet]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> Update(Guid articleId)
		{
			var article = await _articleService.GetArticleWithCategoryNonDeletedAsync(articleId);
			var categories = await _categoryService.GetAllCategoriesNonDeleted();
			var dto = _mapper.Map<ArticleUpdateDTO>(article);
			dto.Categories = categories;

			return View(dto);
		}

		[HttpPost]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> Update(ArticleUpdateDTO articleUpdateDto)
		{
			var map =_mapper.Map<Article>(articleUpdateDto);
			var result = await _validator.ValidateAsync(map);

			if (result.IsValid)
			{
				await _articleService.UpdateArticleAsync(articleUpdateDto);
				return RedirectToAction("Index", "Article", new { Area = "Admin" });

			}
			else
			{
				result.AddToModelState(this.ModelState);
			}


			var categories = await _categoryService.GetAllCategoriesNonDeleted();
			articleUpdateDto.Categories = categories;
			return View(articleUpdateDto);

		}

		[HttpGet]
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> Delete(Guid articleId)
		{

			await _articleService.SafeDeleteArticleAsync(articleId);
			return RedirectToAction("Index", "Article", new { Area = "Admin" });
		}
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> DeletedArticle()
		{
			var articles = await _articleService.GetAllArticleWithCategoryDeletedAsync();
			return View(articles);

		}
		[Authorize(Roles = "Superadmin,Admin")]
		public async Task<IActionResult> UndoDelete(Guid articleId)
		{
			await _articleService.UndoDeleteArticleAsync(articleId);
			return RedirectToAction("Index", "Article", new { Area = "Admin" });
		}

	}
}
