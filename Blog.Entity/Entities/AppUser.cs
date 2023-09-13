using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entity.Entities
{ 
	public class AppUser:IdentityUser<Guid>,IEntityBase
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		//public Guid ImageId { get; set; } = Guid.Parse("4084c97a-2aa1-4675-b519-69f6fe410633");
		//public Image Image { get; set; }

		public ICollection<Article> Articles { get; set; }
	}
}
