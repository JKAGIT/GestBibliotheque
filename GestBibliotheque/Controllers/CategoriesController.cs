using GestBibliotheque.Models;
using GestBibliotheque.Services;
using GestBibliotheque.Utilitaires;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestBibliotheque.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoriesService _categoriesService;
        private readonly LivresService _livresService;

        public CategoriesController(CategoriesService categoriesService, LivresService livresService)
        {
            _categoriesService = categoriesService;
            _livresService = livresService;
        }


        // Exemple d'une méthode utilitaire pour obtenir une catégorie par son ID
        //private async Task<Categories> ObtenirCategorieOuNotFound(Guid id)
        //{
        //    var categorie = await _categoriesService.ObtenirCategorieParId(id);
        //    if (categorie == null) return NotFound();
        //    return categorie;
        //}

        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.ObtenirCategories();
            return View(categories);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var categories = await _categoriesService.ObtenirCategorieParId(id);
            if (categories == null) return NotFound();

            var livres = await _livresService.ObtenirLivresParCategorie(id);
            categories.Livres = livres.ToList();

            return View(categories);
        }
        public IActionResult Ajouter()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajouter(Categories categorie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriesService.AjouterCategorie(categorie);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    GestionErreurs.GererErreur(ex, this);
                }

            }
            return View(categorie);
        }

        public async Task<IActionResult> Modifier(Guid id)
        {
            var categorie = await _categoriesService.ObtenirCategorieParId(id);
            if (categorie == null) return NotFound();
            return View(categorie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modifier(Categories categorie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriesService.ModifierCategorie(categorie);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    GestionErreurs.GererErreur(ex, this);
                }
            }
            return View(categorie);
        }

        public async Task<IActionResult> Supprimer(Guid id)
        {
            var categorie = await _categoriesService.ObtenirCategorieParId(id);
            if (categorie == null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerConfirmation(Guid id)
        {
            try
            {
                await _categoriesService.SupprimerCategorie(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                GestionErreurs.GererErreur(ex, this);
                var categorie = await _categoriesService.ObtenirCategorieParId(id);
                return View("Supprimer", categorie);
            }
        }

    }
}
