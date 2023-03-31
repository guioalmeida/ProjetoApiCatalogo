using Microsoft.EntityFrameworkCore;
using ProjetoApiCatalogo.Context;
using System.Linq.Expressions;

namespace ProjetoApiCatalogo.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _context;
        public Repository(AppDbContext contexto)
        {
            _context = contexto;
        }
        public IQueryable<T> Get() 
        {
            return _context.Set<T>().AsNoTracking();
        }
        public T GetById(Expression<Func<T, bool>> predicate) 
        {
            return _context.Set<T>().SingleOrDefault(predicate);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Add(T entity)
        {
           _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

    }
}
