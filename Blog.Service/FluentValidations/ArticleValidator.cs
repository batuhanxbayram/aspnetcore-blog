using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entity.Entities;
using FluentValidation;

namespace Blog.Service.FluentValidations
{
	public class ArticleValidator: AbstractValidator<Article>
	{
		public ArticleValidator()
		{
			RuleFor(x => x.Title)
				.NotEmpty()
				.MinimumLength(3)
				.NotNull()
				.WithName("Başlık");
			
			RuleFor(x => x.Content)
				.NotEmpty()
				.NotNull()
				.MinimumLength(3)
				.WithName("İçerik");
		}


		
	}
}
