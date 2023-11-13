using Blog.Core.Entities;

namespace Blog.Entity.Entities
{
	public class Article : EntityBase
	{
		public Article()
		{

		}
		public Article(string Title, string content, Guid userId, string createdBy, Guid categoryId, Guid imageId)
		{
			this.Title = Title;
			Content = content;
			UserId = userId;
			CategoryId = categoryId;
			ImageId = imageId;
			CreatedBy = createdBy;
		}

		public string Title { get; set; }
		public string Content { get; set; }
		public int ViewCount { get; set; } = 0;

		public Guid CategoryId { get; set; }
		public Category Category { get; set; }

		public Guid? ImageId { get; set; }
		public Image Image { get; set; }

		public AppUser User { get; set; }

		public Guid UserId { get; set; }
		public ICollection<ArticleVisitor> ArticleVisitor {  get; set; }



	}
}