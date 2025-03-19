using System;
using System.Linq.Expressions;

namespace GestBibliotheque.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(); //Eager Loading
        IQueryable<T> GetAll(); // possibilité d'ajouter des filtres --Lazy Loading
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
