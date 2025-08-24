using System.Linq.Expressions;

namespace DotNetMessagingApplication.Server.Data.Repositories.Base;

public interface IRepository<T> where T : class
{
	T? GetById(int id);

	IEnumerable<T> GetAll();

	IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

	void Add(T entity);

	void Update(T entity);

	void Remove(T entity);

	void SaveChanges();
}
