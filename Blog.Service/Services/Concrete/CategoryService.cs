using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.DTOs.Categories;
using Blog.Entity.Entities;
using Blog.Service.Services.Abstract;

namespace Blog.Service.Services.Concrete
{
	public class CategoryService:ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CategoryService(IUnitOfWork unitOfWork,IMapper _mapper)
		{
			_unitOfWork = unitOfWork;
			this._mapper = _mapper;
		}

		public async Task<IList<CategoryDTO>> GetAllCategoriesNonDeleted()
		{
			var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(a => !a.IsDeleted);
			var dto = _mapper.Map<IList<CategoryDTO>>(categories);
			return dto;
		}
	}
}
