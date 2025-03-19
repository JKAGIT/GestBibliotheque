using GestBibliotheque.Models;

namespace GestBibliotheque.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Livres> Livres { get; }
        IGenericRepository<Categories> Categories { get; }
        Task<int> CompleteAsync();
    }
}
