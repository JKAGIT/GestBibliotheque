using GestBibliotheque.Models;
using GestBibliotheque.Services;
using GestBibliotheque.Utilitaires;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestBibliotheque.Controllers
{
    public class EmpruntsController : Controller
    {
        private readonly EmpruntsService _empruntsService;
        private readonly UsagersService _usagersService;
        private readonly LivresService _livresService;
        private readonly RetoursService _retoursService;

        public EmpruntsController(EmpruntsService empruntsService, UsagersService usagersService, LivresService livresService, RetoursService retoursService)
        {
            _empruntsService = empruntsService;
            _usagersService = usagersService;
            _livresService = livresService;
            _retoursService = retoursService;
        }
        public async Task<IActionResult> Index()
        {
            var empruntsActifs = await _retoursService.ObtenirEmpruntsActif();
            var empruntsInactifs = await _retoursService.ObtenirEmpruntsInActif();

            var viewModel = new EmpruntsIndexViewModel
            {
                EmpruntsActifs = empruntsActifs,
                EmpruntsInactifs = empruntsInactifs
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var emprunt = await _empruntsService.ObtenirEmpruntParId(id);
            if (emprunt == null) return NotFound();

            await ChargeAssociationEntite(emprunt);

            return View(emprunt);
        }


        public async Task<IActionResult> Ajouter()
        {
            await RemplirViewBags();
            return View();
        }         
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajouter(Emprunts emprunts)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empruntsService.AjouterEmprunt(emprunts);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    GestionErreurs.GererErreur(ex, this);
                }
            }

            await RemplirViewBags();
            return View(emprunts);
        }

        public async Task<IActionResult> Modifier(Guid id)
        {
            var emprunt = await _empruntsService.ObtenirEmpruntParId(id);
            if (emprunt == null) return NotFound();

            await RemplirViewBags();

            return View(emprunt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modifier(Emprunts emprunt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empruntsService.ModifierEmprunt(emprunt);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    GestionErreurs.GererErreur(ex, this);
                }
            }
            await RemplirViewBags();

            return View(emprunt);
        }


        public async Task<IActionResult> Supprimer(Guid id)
        {
            var emprunt = await _empruntsService.ObtenirEmpruntParId(id);
            if (emprunt == null) return NotFound();
            return View(emprunt);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerConfirmation(Guid id)
        {
            try
            {
                await _empruntsService.SupprimerEmprunt(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                GestionErreurs.GererErreur(ex, this);
            }
            return RedirectToAction(nameof(Index));
        }

        #region Private Methods

        // Remplir les ViewBags avec Usagers et Livres
        private async Task RemplirViewBags()
        {
            var usagers = await _usagersService.ObtenirUsagers();
            var livres = await _livresService.ObtenirLivresEnStock();

            ViewBag.Usagers = usagers.Select(u => new SelectListItem
            {
                Value = u.ID.ToString(),
                Text = $"{u.Nom} {u.Prenoms}"
            }).ToList();

            ViewBag.Livres = livres.Select(l => new SelectListItem
            {
                Value = l.ID.ToString(),
                Text = l.Titre
            }).ToList();
        }

        // Charger les entités associées (Livre et Usager)
        private async Task ChargeAssociationEntite(Emprunts emprunt)
        {
            if (emprunt.Usager == null)
            {
                emprunt.Usager = await _usagersService.ObtenirUsagerParId(emprunt.IDUsager);
            }

            if (emprunt.Livre == null)
            {
                emprunt.Livre = await _livresService.ObtenirLivreParId(emprunt.IDLivre);
            }
        }

        #endregion
    }
}
