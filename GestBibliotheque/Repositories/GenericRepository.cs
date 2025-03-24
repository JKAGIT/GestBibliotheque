using GestBibliotheque.Donnee;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestBibliotheque.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly GestBibliothequeDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(GestBibliothequeDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IQueryable<T> GetAll() 
        {
            return _dbSet.AsQueryable();
        }
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
     
        public async Task UpdateAsync(T entity)
        {
            var entry = _context.Entry(entity);
            var keyValues = entry.Metadata.FindPrimaryKey().Properties
                .Select(p => entry.Property(p.Name).CurrentValue).ToArray();

            var tracked = _context.Set<T>().Local.FirstOrDefault(e =>
                _context.Entry(e).Metadata.FindPrimaryKey().Properties
                    .Select(p => _context.Entry(e).Property(p.Name).CurrentValue)
                    .SequenceEqual(keyValues));

            if (tracked != null && tracked != entity)
            {
                _context.Entry(tracked).State = EntityState.Detached;
            }

            _dbSet.Update(entity);

        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
       
        public async Task<bool> EntiteExiste(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
