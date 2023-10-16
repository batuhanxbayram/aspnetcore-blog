using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Abstract
{
	public interface IDashBoardService
	{
		Task<List<int>> GetYearlyArticleAsync();
		Task<int> GetTotalArticleCountAsync();
		Task<int> GetTotalCategoryCountAsync();
	}
}
