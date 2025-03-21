using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestBibliotheque.Services
{
    public class RetoursService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmpruntsService _empruntsService;
        private readonly LivresService _livresService;
        private readonly UsagersService _usagersService;

        public RetoursService(IUnitOfWork unitOfWork, LivresService livresService, EmpruntsService empruntsService, UsagersService usagersService)
        {
            _unitOfWork = unitOfWork;
            _empruntsService = empruntsService;
            _livresService = livresService;
            _usagersService = usagersService;
        }

        private void VerifierDateRetour(DateTime? dateRetour)
        {
            if (dateRetour == null || dateRetour == default)
                throw new ArgumentNullException(nameof(dateRetour), "La date de retour doit être valide.");
        }
        public async Task AjouterRetour(Retours retours)
        {
            if (retours == null)
                throw new ArgumentNullException(nameof(retours), "Le retour ne peut pas être nul.");

            VerifierDateRetour(retours.DateRetour);

            var emprunt = await _empruntsService.ObtenirEmpruntParId(retours.IDEmprunt);
            if (emprunt == null)
                throw new ArgumentException("Emprunt non trouvé.");


            var retour = new Retours
            {
                IDEmprunt = retours.IDEmprunt,
                DateRetour = retours.DateRetour
            };
            await _livresService.MettreAJourStock(emprunt.IDLivre, 1);
            await _unitOfWork.Retours.AddAsync(retours);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ModifierRetour(Guid Id, DateTime nouvelleDateRetour)
        {

            VerifierDateRetour(nouvelleDateRetour);

            var retour = await _unitOfWork.Retours.GetByIdAsync(Id);
            if (retour == null)
                throw new ArgumentException("Retour non trouvé.");

            retour.DateRetour = nouvelleDateRetour;

            await _unitOfWork.Retours.UpdateAsync(retour);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Retours> ObtenirRetourParId(Guid id)
        {
            var retours = await _unitOfWork.Retours.GetByIdAsync(id);
            if (retours == null)
                throw new KeyNotFoundException($"L'emprunt avec l'ID {id} n'a pas été trouvé.");
            return retours;
        }


        //public async Task<IEnumerable<RetourViewModel>> ObtenirEmpruntsActif()
        //{
        //    var empruntsActifs = await _unitOfWork.Emprunts.GetAll()
        //         .Include(e => e.Livre)
        //         .Include(e => e.Usager)
        //         .Where(e => e.Retours == null)
        //         .ToListAsync();


        //    var viewModel = empruntsActifs.Select(e => new RetourViewModel
        //    {
        //        IDEmprunt = e.ID,
        //        LivreTitre = e.Livre.Titre,
        //        UsagerNom = e.Usager.Nom + " " + e.Usager.Prenoms,
        //        DateEmprunt = e.DateDebut,
        //        DatePrevu = e.DateRetourPrevue
        //    }).ToList();

        //    return viewModel;
        //}


        //public async Task<IEnumerable<RetourViewModel>> ObtenirEmpruntsActif(Guid usagerId)
        //{
        //    var empruntsActifs = await _unitOfWork.Emprunts.GetAll()
        //        .Include(e => e.Livre)
        //        .Include(e => e.Usager)
        //        .Where(e => e.Retours == null && e.IDUsager == usagerId)  
        //        .ToListAsync();

        //    var viewModel = empruntsActifs.Select(e => new RetourViewModel
        //    {
        //        IDEmprunt = e.ID,
        //        LivreTitre = e.Livre.Titre,
        //        UsagerNom = e.Usager.Nom + " " + e.Usager.Prenoms,
        //        DateEmprunt = e.DateDebut,
        //        DatePrevu = e.DateRetourPrevue
        //    }).ToList();

        //    return viewModel;
        //}


        public async Task<IEnumerable<RetourViewModel>> ObtenirEmpruntsActif(Guid? usagerId = null)
        {
            var empruntsActifsQuery = _unitOfWork.Emprunts.GetAll()
                .Include(e => e.Livre)
                .Include(e => e.Usager)
                .Where(e => e.Retours == null); 


            if (usagerId.HasValue)
            {
                empruntsActifsQuery = empruntsActifsQuery.Where(e => e.IDUsager == usagerId.Value);
            }

            var empruntsActifs = await empruntsActifsQuery.ToListAsync();

            var viewModel = empruntsActifs.Select(e => new RetourViewModel
            {
                IDEmprunt = e.ID,
                LivreTitre = e.Livre.Titre,
                UsagerNom = e.Usager.Nom + " " + e.Usager.Prenoms,
                DateEmprunt = e.DateDebut,
                DatePrevu = e.DateRetourPrevue
            }).ToList();

            return viewModel;
        }



        public async Task<IEnumerable<RetourViewModel>> ObtenirEmpruntsInActif()
        {
            var retours = await _unitOfWork.Retours.GetAll()
                .Include(r => r.Emprunt)
                .ThenInclude(e => e.Livre)
                .Include(r => r.Emprunt.Usager)
                .Where(r => r.DateRetour != null)
                .ToListAsync();

            var viewModel = retours.Select(retour => new RetourViewModel
            {
                IDEmprunt = retour.IDEmprunt,
                LivreTitre = retour.Emprunt.Livre.Titre,
                UsagerNom = retour.Emprunt.Usager.Nom + " " + retour.Emprunt.Usager.Prenoms,
                DateRetour = retour.DateRetour,
                DateEmprunt = retour.Emprunt.DateDebut,
                DatePrevu = retour.Emprunt.DateRetourPrevue
            }).ToList();

            return viewModel;
        }

        // Méthode pour faire une recherche sur les emprunts (par titre de livre ou nom usager)
        public IEnumerable<RetourViewModel> FiltrerEmpruntsParRecherche(IEnumerable<RetourViewModel> emprunts, string recherche)
        {
            if (!string.IsNullOrEmpty(recherche))
            {
                return emprunts.Where(e =>
                    e.LivreTitre.Contains(recherche, StringComparison.OrdinalIgnoreCase) ||
                    e.UsagerNom.Contains(recherche, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return emprunts;
        }

    }
}
