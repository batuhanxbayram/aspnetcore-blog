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
		Task<IList<CategoryDTO>> GetAllCategoriesNonDeleted();
	}
}
