using GestBibliotheque.Models;
using GestBibliotheque.Utilitaires;
using System.Linq.Expressions;

namespace GestBibliotheque.Repositories
{
    public interface ICategories: IGenericRepository<Categories>
    {
        Task<Categories> ObtenirCategorieParCode(string code);
        Task<bool> VerifierExistenceDansLivres(Guid categorieId);
        Task<PaginatedResult<Categories>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
