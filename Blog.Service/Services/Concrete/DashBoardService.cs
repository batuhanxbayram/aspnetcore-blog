using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Service.Services.Abstract;

namespace Blog.Service.Services.Concrete
{
	public class DashBoardService : IDashBoardService
	{
		private readonly IUnitOfWork unitOfWork;

		public DashBoardService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public async Task<List<int>> GetYearlyArticleAsync()
		{
			var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x=> !x.IsDeleted);

			var startDate = DateTime.Now.Date;
			startDate = new DateTime(startDate.Year, 1, 1);

			List<int> datas = new();

			for (int i = 1; i <= 12; i++)
			{

				startDate = new DateTime(startDate.Year, i, 1);
				var endDate = startDate.AddMonths(1);

				var data = articles.Where(x => x.CreatedDate < endDate && x.CreatedDate >= startDate).Count();
				datas.Add(data);
			}
			return datas;
		}

		public async Task<int> GetTotalArticleCountAsync()
		{
			return await unitOfWork.GetRepository<Article>().CountAsync();
		}

		public async Task<int> GetTotalCategoryCountAsync()
		{
			return await unitOfWork.GetRepository<Category>().CountAsync();
		}
	}
}
