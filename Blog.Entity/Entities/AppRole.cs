using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entity.Entities
{
	[NotMapped]
	public class AppRole:IdentityRole<Guid>
	{
	}
}
