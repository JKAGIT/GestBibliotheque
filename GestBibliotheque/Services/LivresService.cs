using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using GestBibliotheque.Services;
using GestBibliotheque.Utilitaires;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestBibliotheque.Services
{
    public class LivresService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LivresService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AjouterLivre(Livres livre)
        {
            ValidationService.VerifierNull(livre, nameof(livre), "Le livre");

            if (await _unitOfWork.Livres.EntiteExiste(l => l.Titre == livre.Titre))
                throw new InvalidOperationException(string.Format(ErreurMessage.EntiteExisteDeja, "Un livre", livre.Titre));

            await _unitOfWork.Livres.AddAsync(livre);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ModifierLivre(Livres livre)
        {
            ValidationService.VerifierNull(livre, nameof(livre), "Le livre");

            var livreAModifier = await _unitOfWork.Livres.GetByIdAsync(livre.ID);
            ValidationService.EnregistrementNonTrouve(livreAModifier, "Livres", livre.ID);

            await _unitOfWork.Livres.UpdateAsync(livre);
            await _unitOfWork.CompleteAsync();
        }

        public async Task SupprimerLivre(Guid idLivre)
        {
            var livreASupprimer = await _unitOfWork.Livres.GetByIdAsync(idLivre);
            ValidationService.EnregistrementNonTrouve(livreASupprimer, "Livres", idLivre);

            //Livre en prêt -- non rendu
            var empruntsActifs = await _unitOfWork.Emprunts.FindAsync(e => e.IDLivre == idLivre && e.Retours == null);
            if (empruntsActifs.Any())
            {
                throw new InvalidOperationException(string.Format(ErreurMessage.ErreurSuppressionEntiteLiee, "un livre", "emprunts actifs"));
            }

            await _unitOfWork.Livres.DeleteAsync(livreASupprimer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task MettreAJourStock(Guid idLivre, int nouveauStock)
        {
            var livre = await _unitOfWork.Livres.GetByIdAsync(idLivre);
            ValidationService.EnregistrementNonTrouve(livre, "Livres", idLivre);

            livre.Stock += nouveauStock;
            await _unitOfWork.Livres.UpdateAsync(livre);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> EstDisponible(Guid idLivre)
        {
            var livre = await _unitOfWork.Livres.GetByIdAsync(idLivre);
            ValidationService.EnregistrementNonTrouve(livre, "Livres", idLivre);
            return livre != null && livre.Stock > 0;
        }

        public async Task<Livres> ObtenirLivreParId(Guid idLivre)
        {
            var livre = await _unitOfWork.Livres.GetByIdAsync(idLivre);
            ValidationService.EnregistrementNonTrouve(livre, "Livres", idLivre);
            return livre;

        }
        public async Task<IEnumerable<Livres>> ObtenirLivresParCategorie(Guid idCategorie)
        {
            try
            {
                return await _unitOfWork.Livres.FindAsync(l => l.IDCategorie == idCategorie);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Livres"), ex);
            }
        }

        public async Task<IEnumerable<Livres>> ObtenirLivresEnStock()
        {
            try
            {
                return await _unitOfWork.Livres.FindAsync(l => l.Stock > 0);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Livres"), ex);
            }

        }
        public async Task<IEnumerable<Livres>> ObtenirLivres()  //lazy loading
        {
            try
            {
                return await _unitOfWork.Livres.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Livres"), ex);
            }
        }

        public async Task<IEnumerable<Livres>> ObtenirLivresAvecCategories()   //eager loading
        {
            try
            {
                return await _unitOfWork.Livres
                    .GetAll()
                    .Include(c => c.Categories)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Livres"), ex);
            }
        }
    }
}


