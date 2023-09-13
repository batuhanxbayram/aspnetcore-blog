using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.DTOs.Articles;
using Blog.Entity.Entities;
using Blog.Entity.Enums;
using Blog.Service.Extension;
using Blog.Service.Helpers.Images;
using Blog.Service.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Services.Concrete
{
	public class ArticleService : IArticleService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IImageHelper _imageHelper;

		public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_imageHelper = imageHelper;
		}

		public async Task<List<ArticleDTO>> GetAllArticleWithCategoryNonDeletedAsync()
		{
			var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted, x => x.Category);
			var map = _mapper.Map<List<ArticleDTO>>(articles);
			return map;
		}

		public async Task<ArticleDTO> GetArticleWithCategoryNonDeletedAsync(Guid id)
		{
			var article = await _unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id == id, c => c.Category ,c=> c.Image);
			var dto = _mapper.Map<ArticleDTO>(article);
			return dto;
		}
		public async Task CreateArticleAsync(ArticleAddDTO articleAddDto)
		{
			var user = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();
			var email = _httpContextAccessor.HttpContext.User.GetLoggedInEmail();

			var imageupload = await _imageHelper.Upload(articleAddDto.Title, articleAddDto.Photo, ImageType.Post);

			//Image image = new Image();
			Image image = new Image(imageupload.FullName,articleAddDto.Photo.ContentType,email);
			await _unitOfWork.GetRepository<Image>().AddAsync(image);

			var article = new Article()
			{
				Content = articleAddDto.Content,
				CategoryId = articleAddDto.CategoryId,
				Title = articleAddDto.Title,
				UserId = user,
				CreatedBy = email,
				ImageId = image.Id,
			};
			await _unitOfWork.GetRepository<Article>().AddAsync(article);
			await _unitOfWork.SaveAsync();
		}

		public async Task UpdateArticleAsync(ArticleUpdateDTO articleUpdateDto)
		{

			var email = _httpContextAccessor.HttpContext.User.GetLoggedInEmail();
			var article = await _unitOfWork.GetRepository<Article>()
				.GetAsync(x => !x.IsDeleted && x.Id == articleUpdateDto.Id, c => c.Category,i=>i.Image);

			if (articleUpdateDto.Photo != null)
			{
				_imageHelper.Delete(articleUpdateDto.Photo.FileName);
				var imageUpload =
					await _imageHelper.Upload(articleUpdateDto.Title, articleUpdateDto.Photo, ImageType.Post);
				Image image = new Image(imageUpload.FullName, articleUpdateDto.Photo.ContentType, email);
				await _unitOfWork.GetRepository<Image>().AddAsync(image);
				article.ImageId=image.Id;


			}
			article.Content = articleUpdateDto.Content;
			article.Title = articleUpdateDto.Title;
			article.CategoryId = articleUpdateDto.CategoryId;
			article.ModifiedBy = email;
			article.ModifiedDate = DateTime.Now;



			await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
			await _unitOfWork.SaveAsync();
		}

		public async Task SafeDeleteArticleAsync(Guid articleId)
		{
			
			var email = _httpContextAccessor.HttpContext.User.GetLoggedInEmail();
			var article = await _unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
			article.IsDeleted = true;
			article.DeletedDate = DateTime.Now;
			article.DeletedBy=email;
			await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
			await _unitOfWork.SaveAsync();

		}
	}
}
