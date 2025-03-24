using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using GestBibliotheque.Utilitaires;
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
            ValidationService.VerifierNull(categorie, nameof(categorie), "La catégorie");

            if (await _unitOfWork.Categories.EntiteExiste(c => c.Code == categorie.Code))
                throw new InvalidOperationException(string.Format(ErreurMessage.EntiteExisteDeja, "Une catégorie", categorie.Code));

            // throw new Exception("Erreur simulée dans le service lors de l'ajout de la catégorie.");

            await _unitOfWork.Categories.AddAsync(categorie);
            await _unitOfWork.CompleteAsync();

        }
        public async Task ModifierCategorie(Categories categorie)
        {
            ValidationService.VerifierNull(categorie, nameof(categorie), "La catégorie");

            var categorieAModifier = await _unitOfWork.Categories.GetByIdAsync(categorie.ID);
            ValidationService.EnregistrementNonTrouve(categorieAModifier, "Categories", categorie.ID);

            await _unitOfWork.Categories.UpdateAsync(categorie);
            await _unitOfWork.CompleteAsync();
        }

        public async Task SupprimerCategorie(Guid idCategorie)
        {
            var categorieASupprimer = await _unitOfWork.Categories
                                            .GetAll()
                                            .Include(c => c.Livres)
                                            .FirstOrDefaultAsync(c => c.ID == idCategorie);

            ValidationService.EnregistrementNonTrouve(categorieASupprimer, "Categories", idCategorie);

            if (categorieASupprimer.Livres != null && categorieASupprimer.Livres.Any())
                throw new InvalidOperationException(string.Format(ErreurMessage.ErreurSuppressionEntiteLiee, "une catégorie", "livres"));

            await _unitOfWork.Categories.DeleteAsync(categorieASupprimer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Categories>> ObtenirCategories()
        {
            try
            {
                return await _unitOfWork.Categories.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Categories"), ex);
            }
        }

        public async Task<Categories> ObtenirCategorieParId(Guid idCategorie)
        {
            var categorie = await _unitOfWork.Categories.GetByIdAsync(idCategorie);
            ValidationService.EnregistrementNonTrouve(categorie, "Categories", idCategorie);
            return categorie;
        }
    }

}
