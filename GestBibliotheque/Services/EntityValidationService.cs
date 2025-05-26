using GestBibliotheque.Donnee;
using GestBibliotheque.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestBibliotheque.Services
{
    public class EntityValidationService<T> : IEntityValidationService<T> where T : class
    {
        private readonly GestBibliothequeDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityValidationService(GestBibliothequeDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<bool> VerifierExistenceAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var dbSet = _context.Set<T>();

            return await dbSet.AnyAsync(predicate);
        }
    }
}


