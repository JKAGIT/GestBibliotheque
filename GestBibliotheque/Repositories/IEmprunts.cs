using GestBibliotheque.Models;

namespace GestBibliotheque.Repositories
{
    public interface IEmprunts:IGenericRepository<Emprunts>
    {
        Task<IEnumerable<Emprunts>> ObtenirEmpruntParUsager(Guid idUsager);
        Task AjouterEmpruntReservation(Guid idReservation);
    }
}
