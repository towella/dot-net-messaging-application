using System.Linq.Expressions;

namespace DotNetMessagingApplication.Server.Data.Repositories.Base;

public class Repository<T> : IRepository<T> where T : class
{
	protected readonly MessagingAppContext _context;

	public Repository(MessagingAppContext context)
	{
		_context = context;
	}

	public void Add(T entity)
	{
		_context.Set<T>().Add(entity);
	}

	public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
	{
		return _context.Set<T>().Where(predicate).ToList();
	}

	public IEnumerable<T> GetAll()
	{
		return _context.Set<T>().ToList();
	}

	public T? GetById(int id)
	{
		return _context.Set<T>().Find(id);
	}

	public void Remove(T entity)
	{
		_context.Set<T>().Remove(entity);
	}

	public void SaveChanges()
	{
		_context.SaveChanges();
	}

	public void Update(T entity)
	{
		_context.Set<T>().Update(entity);
	}
}
