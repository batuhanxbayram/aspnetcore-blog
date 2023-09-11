using System.Linq.Expressions;
using Blog.Core.Entities;

namespace Blog.Data.Repositories.Abstractions;

public interface IRepository<T> where T : class,IEntityBase, new()
{
	Task AddAsync(T entity);
	Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
	Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
	Task<T> GetByGuidAsync(Guid id);
	Task<T> UpdateAsync(T entity);
	Task<bool> DeleteAsync(T entity);
	Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
	Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
}