using GestBibliotheque.Models;
using GestBibliotheque.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestBibliotheque.Utilitaires;

namespace GestBibliotheque.Controllers
{
    public class LivresController : Controller
    {
        private readonly LivresService _livresService;
        private readonly CategoriesService _categoriesService;


        public LivresController(LivresService livresService, CategoriesService categoriesService)
        {
            _livresService = livresService;
            _categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index()
        {
            var livres = await _livresService.ObtenirLivresAvecCategories();
            return View(livres);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var livre = await _livresService.ObtenirLivreParId(id);
            if (livre == null) return NotFound();
            if (livre.Categories == null)
            {
                var categorie = await _categoriesService.ObtenirCategorieParId(livre.IDCategorie);
                livre.Categories = categorie;
            }
            return View(livre);
        }
        public async Task<IActionResult> AjouterAsync()
        {
            var categories = await _categoriesService.ObtenirCategories();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajouter(Livres livre)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _livresService.AjouterLivre(livre);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                   GestionErreurs.GererErreur(ex, this);
                }
            }
            ViewBag.Categories = await _categoriesService.ObtenirCategories();
            return View(livre);
        }

        public async Task<IActionResult> Modifier(Guid id)
        {
            var livre = await _livresService.ObtenirLivreParId(id);
            if (livre == null) return NotFound();


            var categories = await _categoriesService.ObtenirCategories();
            ViewBag.Categories = new SelectList(categories, "ID", "Libelle", livre.IDCategorie);

            return View(livre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modifier(Livres livre)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _livresService.ModifierLivre(livre);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    GestionErreurs.GererErreur(ex, this);
                }
            }
        
            var categories = await _categoriesService.ObtenirCategories();
            ViewBag.Categories = new SelectList(categories, "ID", "Libelle", livre.IDCategorie);
            return View(livre);
        }

        public async Task<IActionResult> Supprimer(Guid id)
        {
            var livre = await _livresService.ObtenirLivreParId(id);
            if (livre == null) return NotFound();
            return View(livre);
        }

        public async Task<IActionResult> SupprimerConfirmation(Guid id)
        {
            try
            {
                await _livresService.SupprimerLivre(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                GestionErreurs.GererErreur(ex, this);
            }
            return RedirectToAction(nameof(Index));
        }

    }

}
