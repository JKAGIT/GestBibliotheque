using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestBibliotheque.Services
{
    public class CategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AjouterCategorie(Categories categorie)
        {
            if (categorie == null)
                throw new ArgumentNullException(nameof(categorie), "La catégorie ne peut pas être nulle. Assurez-vous que toutes les données sont correctement fournies.");

            var categorieExiste = await _unitOfWork.Categories.FindAsync(c => c.Code == categorie.Code);
            if (categorieExiste.Any())
            {
                throw new InvalidOperationException($"Une catégorie avec le code {categorie.Code} existe déjà.");
            }

            await _unitOfWork.Categories.AddAsync(categorie);
            await _unitOfWork.CompleteAsync();
        }
        public async Task ModifierCategorie(Categories categorie)
        {
            if (categorie == null)
                throw new ArgumentNullException(nameof(categorie), "La catégorie à modifier ne peut pas être nulle. Assurez-vous que toutes les données sont correctement fournies.");

            var categorieAModifier = await _unitOfWork.Categories.GetByIdAsync(categorie.ID);
            if (categorieAModifier == null)
                throw new KeyNotFoundException($"La catégorie avec l'ID {categorie.ID} n'a pas été trouvée.");

            await _unitOfWork.Categories.UpdateAsync(categorie);
            await _unitOfWork.CompleteAsync();
        }

        public async Task SupprimerCategorie(Guid idCategorie)
        {
            var categorieASupprimer = await _unitOfWork.Categories
                                            .GetAll()
                                            .Include(c => c.Livres)
                                            .FirstOrDefaultAsync(c => c.ID == idCategorie);

            if (categorieASupprimer == null)
                throw new KeyNotFoundException("La catégorie à supprimer est introuvable.");

            if (categorieASupprimer.Livres != null && categorieASupprimer.Livres.Any())
                throw new InvalidOperationException("Impossible de supprimer une catégorie liée à des livres.");

            await _unitOfWork.Categories.DeleteAsync(categorieASupprimer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Categories>> ObtenirCategories()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<Categories> ObtenirCategorieParId(Guid idCategorie)
        {
            return await _unitOfWork.Categories.GetByIdAsync(idCategorie);
        }
        public async Task<IEnumerable<Categories>> ObtenirCategoriesParCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Le code de la catégorie ne peut pas être vide.");

            var result = await _unitOfWork.Categories.FindAsync(c => c.Code == code);

            if (result == null || !result.Any())
                throw new KeyNotFoundException($"Aucune catégorie trouvée avec le code : {code}");

            return result;
        }
    }


}
