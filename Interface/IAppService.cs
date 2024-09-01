using System.Linq.Expressions;

namespace MediatrExample.Interface
{
    public interface IAppService<T>
    {
        Task<T> Get(Guid id);

        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);

        Task<Guid> Add(T t);

        Task AddMany(List<T> list);

        Task Update(T t);

        Task Delete(T t);
    }
}
