using GestBibliotheque.Models;

namespace GestBibliotheque.Repositories
{
    public interface IReservations:IGenericRepository<Reservations>
    {
        Task AnnulerReservation(Guid idReservation);
        Task<IEnumerable<ReservationViewModel>> ObtenirReservations(Guid? usagerId = null);
        Task<IEnumerable<ReservationViewModel>> ObtenirReservationsAvecDisponibilite();
    }
}
