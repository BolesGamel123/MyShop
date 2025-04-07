using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess.Data;
using MyShop.Entities.Repositories;
using System.Linq.Expressions;


namespace MyShop.DataAccess.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? perdicate = null, string? Includeword = null)
        {
            IQueryable<T> query = _dbSet;
            if (perdicate != null)
            {
                query = query.Where(perdicate);
            }
            if (Includeword != null)
            {
                foreach (var item in Includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }
        public T FirstOrDefault(Expression<Func<T, bool>>? perdicate=null, string? Includeword = null)
        {
            IQueryable<T> query = _dbSet;
            if (perdicate != null)
            {
                query = query.Where(perdicate);
            }
            if (Includeword != null)
            {
                foreach (var item in Includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.SingleOrDefault()!;
        }
        public void Add(T entity)
        {
           _dbSet.Add(entity);
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);          
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);

        }
    }
}
