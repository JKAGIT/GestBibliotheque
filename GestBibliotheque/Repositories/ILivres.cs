using GestBibliotheque.Models;

namespace GestBibliotheque.Repositories
{
    public interface ILivres : IGenericRepository<Livres>
    {
        Task MettreAJourStock(Guid idLivre, int nouveauStock);
        Task<bool> EstDisponible(Guid idLivre);
        Task<IEnumerable<Livres>> ObtenirLivresParCategorie(Guid idCategorie);
        Task<IEnumerable<Livres>> ObtenirLivresEnStock();
        Task<IEnumerable<Livres>> ObtenirLivresAvecCategories(Guid? id = null);
    }
}
