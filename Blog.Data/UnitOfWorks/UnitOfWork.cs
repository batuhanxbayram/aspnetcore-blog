using Blog.Core.Entities;
using Blog.Data.Context;
using Blog.Data.Repositories;
using Blog.Data.Repositories.Abstractions;
using Blog.Data.Repositories.Concretes;

namespace Blog.Data.UnitOfWorks
{
	public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async ValueTask DisposeAsync()
        {
            await dbContext.DisposeAsync();
        }

        public int Save()
        {
            return dbContext.SaveChanges();
        }
		// IRepository<T> IUnitOfWork.GetRepository<T>() bu da olabilir 
		public IRepository<T> GetRepository<T>()
	        where T : class, IEntityBase, new()
        {
	        return new Repository<T>(dbContext);
        }

        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

       
    }
}
