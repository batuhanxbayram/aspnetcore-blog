using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entity.Entities;
using FluentValidation;

namespace Blog.Service.FluentValidations
{
	public class CategoryValidator:AbstractValidator<Category>
	{
		public CategoryValidator()
		{
			RuleFor(x => x.Name)
				.MinimumLength(2)
				.NotEmpty()
				.NotNull()
				.MaximumLength(100)
				.WithName("Kategori Adı ");

		}
	}
}
