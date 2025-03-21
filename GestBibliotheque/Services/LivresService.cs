using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using GestBibliotheque.Services;
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

        private async Task<bool> LivreExiste(string titre)
        {
            var livreExistant = await _unitOfWork.Livres.FindAsync(l => l.Titre == titre);
            return livreExistant.Any();
        }
        public async Task AjouterLivre(Livres livre)
        {
            if (livre == null)
                throw new ArgumentNullException(nameof(livre), "Le livre ne peut pas être nul. Assurez-vous que toutes les données sont correctement fournies.");


            if (await LivreExiste(livre.Titre))
                throw new InvalidOperationException($"Un livre avec le même titre '{livre.Titre}' existe déjà.");

            await _unitOfWork.Livres.AddAsync(livre);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ModifierLivre(Livres livre)
        {
            if (livre == null)
                throw new ArgumentNullException(nameof(livre), "Le livre à modifier ne peut pas être nul. Assurez-vous que toutes les données sont correctement fournies.");

            var livreAModifier = await _unitOfWork.Livres.GetByIdAsync(livre.ID);
            if (livreAModifier == null)
                throw new KeyNotFoundException($"La catégorie avec l'ID {livre.ID} n'a pas été trouvée.");


            await _unitOfWork.Livres.UpdateAsync(livre);
            await _unitOfWork.CompleteAsync();
        }

        public async Task SupprimerLivre(Guid idLivre)
        {
            var livreASupprimer = await _unitOfWork.Livres.GetByIdAsync(idLivre);
            if (livreASupprimer == null)
                throw new KeyNotFoundException("Le livre à supprimer non trouvé.");

            if (livreASupprimer != null)
            {
                await _unitOfWork.Livres.DeleteAsync(livreASupprimer);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task MettreAJourStock(Guid idLivre, int nouveauStock)
        {
            var livre = await _unitOfWork.Livres.GetByIdAsync(idLivre);
            if (livre == null)
                throw new KeyNotFoundException($"La livre spécifié non trouvé");

            livre.Stock += nouveauStock;
            await _unitOfWork.Livres.UpdateAsync(livre);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> EstDisponible(Guid idLivre)
        {
            var livre = await _unitOfWork.Livres.GetByIdAsync(idLivre);
            return livre != null && livre.Stock > 0;
        }

        public async Task<Livres> ObtenirLivreParId(Guid idLivre)
        {
            return await _unitOfWork.Livres.GetByIdAsync(idLivre);
        }
        public async Task<IEnumerable<Livres>> ObtenirLivresParCategorie(Guid idCategorie)
        {
            return await _unitOfWork.Livres.FindAsync(l => l.IDCategorie == idCategorie);
        }

        public async Task<IEnumerable<Livres>> ObtenirLivresEnStock()
        {
            return await _unitOfWork.Livres.FindAsync(l => l.Stock > 0);
        }
        public async Task<IEnumerable<Livres>> ObtenirLivres()  //lazy loading
        {
            return await _unitOfWork.Livres.GetAllAsync();
        }

        public async Task<IEnumerable<Livres>> ObtenirLivresAvecCategories()   //eager loading
        {
            return await _unitOfWork.Livres
                .GetAll()
                .Include(c => c.Categories)
                .ToListAsync();
        }
    }
}


