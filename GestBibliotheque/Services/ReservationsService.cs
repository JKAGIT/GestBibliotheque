using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using GestBibliotheque.Services;
using GestBibliotheque.Utilitaires;
using Microsoft.EntityFrameworkCore;

public class ReservationsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly LivresService _livresService;

    public ReservationsService(IUnitOfWork unitOfWork, LivresService livresService)
    {
        _unitOfWork = unitOfWork;
        _livresService = livresService;
    }

    public async Task AjouterReservation(Reservations reservation)
    {
        ValidationService.VerifierNull(reservation, nameof(reservation), "La réservation");

        await _unitOfWork.Reservations.AddAsync(reservation);
        await _unitOfWork.CompleteAsync();
    }

    public async Task ModifierReservation(Reservations reservation)
    {
        ValidationService.VerifierNull(reservation, nameof(reservation), "La réservation");

        var reservationAModifier = await _unitOfWork.Reservations.GetByIdAsync(reservation.ID);
        ValidationService.EnregistrementNonTrouve(reservationAModifier, "Reservations", reservation.ID);

        if (reservationAModifier.Annuler)
             throw new KeyNotFoundException(string.Format(ErreurMessage.ReservationAnnulee));

        reservationAModifier.DateDebut = reservation.DateDebut;
        reservationAModifier.DateRetourEstimee = reservation.DateRetourEstimee;
        reservationAModifier.Annuler = reservation.Annuler;
        reservationAModifier.IDLivre = reservation.IDLivre;

        await _unitOfWork.Reservations.UpdateAsync(reservationAModifier);
        await _unitOfWork.CompleteAsync();
    }


    public async Task AnnulerReservation(Guid idReservation)
    {
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(idReservation);
        ValidationService.EnregistrementNonTrouve(reservation, "Reservations", idReservation);

        if (reservation.Annuler)
            throw new KeyNotFoundException(string.Format(ErreurMessage.ReservationAnnulee));

        if (reservation.Emprunt != null)
            throw new InvalidOperationException(string.Format(ErreurMessage.ErreurSuppressionEntiteLiee, "une réservation", "emprunts"));

        reservation.Annuler = true;
        await _unitOfWork.Reservations.UpdateAsync(reservation);
        await _unitOfWork.CompleteAsync();
    }
    public async Task<Reservations> ObtenirReservationParId(Guid idReservation)
    {
       // var reservation = await _unitOfWork.Reservations.GetByIdAsync(idReservation);
        var reservation = await _unitOfWork.Reservations
       .GetAll() 
       .Include(r => r.Livre) 
       .Include(r => r.Usager) 
       .FirstOrDefaultAsync(r => r.ID == idReservation);

        ValidationService.EnregistrementNonTrouve(reservation, "Reservations", idReservation);

        return reservation;
    }


    public async Task<IEnumerable<ReservationViewModel>> ObtenirReservations(Guid? usagerId = null)
    {
        try
        {
            var reservationsActivesQuery = _unitOfWork.Reservations.GetAll()
            .Include(r => r.Livre)
            .Include(r => r.Usager)
            .Where(r => r.Emprunt == null && !r.Annuler);

            if (usagerId.HasValue)
            {
                reservationsActivesQuery = reservationsActivesQuery.Where(r => r.IDUsager == usagerId.Value);
            }

            var reservationsActives = await reservationsActivesQuery.ToListAsync();

            var viewModel = reservationsActives.Select(r => new ReservationViewModel
            {
                IdReservation = r.ID,
                LivreTitre = r.Livre.Titre,
                UsagerNom = r.Usager.Nom + " " + r.Usager.Prenoms,
                DateDebut = r.DateDebut,
                DatePrevue = r.DateRetourEstimee,
                Annuler = r.Annuler,
            }).ToList();

            return viewModel;
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Réservations actives"), ex);
        }
    }
    public async Task<IEnumerable<ReservationViewModel>> ObtenirReservationsAvecDisponibilite()
    {
        try
        {
            var reservationsActivesQuery = _unitOfWork.Reservations.GetAll()
                .Include(r => r.Livre)
                .Include(r => r.Usager)
                .Where(r => r.Emprunt == null && !r.Annuler);

            var reservationsActives = await reservationsActivesQuery.ToListAsync();

            var viewModel = new List<ReservationViewModel>();

            foreach (var r in reservationsActives)
            {
                var livreDisponible = await _livresService.EstDisponible(r.IDLivre);
                var disponibilite = livreDisponible ? true : false;

                viewModel.Add(new ReservationViewModel
                {
                    IdReservation = r.ID,
                    LivreTitre = r.Livre.Titre,
                    UsagerNom = r.Usager.Nom + " " + r.Usager.Prenoms,
                    DateDebut = r.DateDebut,
                    DatePrevue = r.DateRetourEstimee,
                    Annuler = r.Annuler,
                    EstDisponible = disponibilite  // Ajout de la disponibilité ici
                });
            }

            return viewModel;
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Réservations actives"), ex);
        }
    }


    public async Task<IEnumerable<ReservationViewModel>> ObtenirReservationsEmprunter(Guid? usagerId = null)
    {
        try
        {
            var reservationsActivesQuery = _unitOfWork.Reservations.GetAll()
                .Include(r => r.Livre)
                .Include(r => r.Usager)
                .Include(r => r.Emprunt)
                .Where(r => r.Emprunt != null);

            if (usagerId.HasValue)
            {
                reservationsActivesQuery = reservationsActivesQuery.Where(r => r.IDUsager == usagerId.Value);
            }

            var reservationsActives = await reservationsActivesQuery.ToListAsync();

            var viewModel = reservationsActives.Select(r => new ReservationViewModel
            {
                IdReservation = r.ID,
                LivreTitre = r.Livre.Titre,
                UsagerNom = r.Usager.Nom + " " + r.Usager.Prenoms,
                DateDebut = r.DateDebut,
                DatePrevue = r.DateRetourEstimee,
                Annuler = r.Annuler
            }).ToList();

            return viewModel;
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Réservations actives"), ex);
        }
    }

    public IEnumerable<RetourViewModel> FiltrerReservationsParRecherche(IEnumerable<RetourViewModel> reservations, string recherche)
    {
        try
        {
            if (!string.IsNullOrEmpty(recherche))
            {
                return reservations.Where(r =>
                    r.LivreTitre.Contains(recherche, StringComparison.OrdinalIgnoreCase) ||
                    r.UsagerNom.Contains(recherche, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return reservations;
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Réservations"), ex);
        }
    }

}


