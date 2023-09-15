using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.DTOs.Categories;
using Blog.Entity.Entities;
using Blog.Service.Extension;
using Blog.Service.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Services.Concrete
{
	public class CategoryService:ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CategoryService(IUnitOfWork unitOfWork,IMapper _mapper,IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			this._mapper = _mapper;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<List<CategoryDTO>> GetAllCategoriesNonDeleted()
		{
			var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(a => !a.IsDeleted);
			var dto = _mapper.Map<List<CategoryDTO>>(categories);
			return dto;
		}

		public async Task<List<CategoryDTO>> GetAllCategoriesDeleted()
		{
			var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(x => x.IsDeleted);
			var dto = _mapper.Map<List<CategoryDTO>>(categories);
			return dto;
		}

		public async Task<string> CreateCategoryAsync(CategoryAddDTO categoryAddDto)
		{
			var email =  _httpContextAccessor.HttpContext.User.GetLoggedInEmail();
			var category = new Category(categoryAddDto.Name,email);

			await _unitOfWork.GetRepository<Category>().AddAsync(category);
			await _unitOfWork.SaveAsync();
			return category.Name;
		}

		public async Task<Category> GetByGuidId(Guid id)
		{
			var category = await _unitOfWork.GetRepository<Category>().GetByGuidAsync(id);
		
			return category;
		}

		public async Task<string> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDto)
		{
			var email = _httpContextAccessor.HttpContext.User.GetLoggedInEmail();
			var category = await _unitOfWork.GetRepository<Category>()
				.GetAsync(x => !x.IsDeleted && x.Id == categoryUpdateDto.Id);

			category.Name= categoryUpdateDto.Name;
			category.ModifiedBy = email;
			category.ModifiedDate=DateTime.Now;

			await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
			await _unitOfWork.SaveAsync();
			return category.Name;
		}

		public async Task SafeDeleteCategory(Guid id)
		{
			var email = _httpContextAccessor.HttpContext.User.GetLoggedInEmail();

			var category = await _unitOfWork.GetRepository<Category>().GetByGuidAsync(id);

			category.IsDeleted = true;
			category.DeletedBy = email;
			category.DeletedDate=DateTime.Now;

			await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
			await _unitOfWork.SaveAsync();

		}

		public async Task UndoDeleteCategory(Guid id)
		{
			var category = await _unitOfWork.GetRepository<Category>().GetByGuidAsync(id);

			category.IsDeleted = false;
			category.DeletedBy = null;
			category.DeletedDate = null;

			await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
			await _unitOfWork.SaveAsync();

		}
	}
}
