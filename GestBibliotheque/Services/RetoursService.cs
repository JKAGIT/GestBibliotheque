using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using GestBibliotheque.Utilitaires;
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

        public async Task AjouterRetour(Retours retours)
        {
            ValidationService.VerifierNull(retours, nameof(retours), "Le retour");

            ValidationService.VerifierDate(retours.DateRetour, "de retour");

            var emprunt = await _empruntsService.ObtenirEmpruntParId(retours.IDEmprunt);
            ValidationService.EnregistrementNonTrouve(emprunt, "Emprunts", retours.IDEmprunt);

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
            ValidationService.VerifierDate(nouvelleDateRetour, "de retour");

            var retour = await _unitOfWork.Retours.GetByIdAsync(Id);
            ValidationService.EnregistrementNonTrouve(retour, "Retours", Id);

            retour.DateRetour = nouvelleDateRetour;

            await _unitOfWork.Retours.UpdateAsync(retour);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Retours> ObtenirRetourParId(Guid id)
        {
            var retours = await _unitOfWork.Retours.GetByIdAsync(id);
            ValidationService.EnregistrementNonTrouve(retours, "Retours", id);
            return retours;
        }

        public async Task<IEnumerable<RetourViewModel>> ObtenirEmpruntsActif(Guid? usagerId = null)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Emprunts"), ex);
            }
        }

        public async Task<IEnumerable<RetourViewModel>> ObtenirEmpruntsInActif()
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Emprunts"), ex);
            }
        }

        /// <summary>
        /// Filtre pour recherche des emprunts (par titre de livre ou nom usager)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RetourViewModel> FiltrerEmpruntsParRecherche(IEnumerable<RetourViewModel> emprunts, string recherche)// 
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Emprunts"), ex);
            }
        }
    }
}
