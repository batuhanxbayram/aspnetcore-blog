using Blog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entity.DTOs.Categories;

namespace Blog.Service.Services.Abstract
{
	public interface ICategoryService
	{
		Task<List<CategoryDTO>> GetAllCategoriesNonDeleted();
		Task<string> CreateCategoryAsync(CategoryAddDTO categoryAddDto);
		Task<Category> GetByGuidId(Guid id);
		Task<string> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDto);
		Task SafeDeleteCategory(Guid id);
	}
}
