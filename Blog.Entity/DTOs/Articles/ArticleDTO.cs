using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entity.DTOs.Categories;

namespace Blog.Entity.DTOs.Articles
{
	public class ArticleDTO
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public CategoryDTO Category { get; set; }

		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsDeleted { get; set; }

	}
}
