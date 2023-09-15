using Blog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entity.DTOs.Articles;

namespace Blog.Service.Services.Abstract
{
	public interface IArticleService
	{
		  Task<List<ArticleDTO>> GetAllArticleWithCategoryNonDeletedAsync();
		  Task<List<ArticleDTO>> GetAllArticleWithCategoryDeletedAsync();

		  Task CreateArticleAsync(ArticleAddDTO articleAddDto);
		  Task<ArticleDTO> GetArticleWithCategoryNonDeletedAsync(Guid id);
		  Task UpdateArticleAsync(ArticleUpdateDTO articleUpdateDto);
		  Task SafeDeleteArticleAsync(Guid articleId);
		  Task UndoDeleteArticleAsync(Guid articleId);
	}
}
