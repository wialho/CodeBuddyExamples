using MediatrExample.Interface;
using MediatrExample.Models;
using System.Linq.Expressions;

namespace MediatrExample.Service
{
    public class AppService<T> : IAppService<T> where T : BaseEntity
    {
        public ApplicationDbContext _ctx;

        public AppService(ApplicationDbContext context)
        {
            _ctx = context;
        }

        public async Task<Guid> Add(T t)
        {
            _ctx.Set<T>().Add(t);

            await _ctx.SaveChangesAsync();

            return t.Id;
        }

        public async Task AddMany(List<T> list)
        {
            _ctx.Set<T>().AddRange(list);

            await _ctx.SaveChangesAsync();
        }

        public async Task Delete(T t)
        {
            t.IsDeleted = true;
            _ctx.Set<T>().Update(t);

            await _ctx.SaveChangesAsync();
        }

        public async Task<T> Get(Guid id)
        {
            return await _ctx.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _ctx.Set<T>().Where(expression);
        }

        public async Task Update(T t)
        {
            _ctx.Set<T>().Update(t);

            await _ctx.SaveChangesAsync();
        }
    }
}
