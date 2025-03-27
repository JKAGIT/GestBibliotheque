using System.Linq.Expressions;

namespace GestBibliotheque.Repositories
{
    public interface IEntityValidationService<T> where T : class
    {
        Task<bool> VerifierExistenceAsync(Expression<Func<T, bool>> predicate);
    }
}
