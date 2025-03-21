using AspNetCoreGeneratedDocument;
using GestBibliotheque.Models;
using GestBibliotheque.Services;
using GestBibliotheque.Utilitaires;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestBibliotheque.Controllers
{
    public class RetoursController : Controller
    {
        private readonly RetoursService _retoursService;
        private readonly EmpruntsService _empruntsService;
        private readonly LivresService _livresService;
        private readonly UsagersService _usagersService;

        public RetoursController(RetoursService retoursService, EmpruntsService empruntsService, LivresService livresService, UsagersService usagersService)
        {
            _retoursService = retoursService;
            _empruntsService = empruntsService;
            _livresService = livresService;
            _usagersService = usagersService;
        }
        public async Task<IActionResult> EmpruntActif()
        {
            var viewModel = await _retoursService.ObtenirEmpruntsActif();
            return View(viewModel);
        }
        public async Task<IActionResult> EmpruntInActif()
        {
            var viewModel = await _retoursService.ObtenirEmpruntsInActif();
            return View(viewModel);
        }
        public async Task<IActionResult> RechercherEmprunt(string recherche)
        {
            var empruntsActifs = await _retoursService.ObtenirEmpruntsActif();

            var empruntsFiltres = _retoursService.FiltrerEmpruntsParRecherche(empruntsActifs, recherche);
            ViewData["Recherche"] = recherche;

            return View(empruntsFiltres);
        }
        public async Task<IActionResult> Ajouter(Guid empruntId)
        {

            var emprunt = await _empruntsService.ObtenirEmpruntParId(empruntId);
            if (emprunt == null)
            {
                return NotFound();
            }

            var livre = await _livresService.ObtenirLivreParId(emprunt.IDLivre);
            var usager = await _usagersService.ObtenirUsagerParId(emprunt.IDUsager);

            if (livre == null || usager == null)
            {
                return BadRequest("Les informations du livre ou de l'emprunteur sont manquantes.");
            }

            var viewModel = new RetourViewModel
            {
                IDEmprunt = empruntId,
                LivreTitre = livre.Titre,
                UsagerNom = usager.Nom + " " + usager.Prenoms
              //  DateRetour = DateTime.Now  
            };

            return View(viewModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajouter(Retours retour)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _retoursService.AjouterRetour(retour);
                    return RedirectToAction("Index", "Emprunts");
                }
                catch (Exception ex)
                {
                    GestionErreurs.GererErreur(ex, this);
                }
            }
                var emprunt = await _empruntsService.ObtenirEmpruntParId(retour.IDEmprunt);
                var livre = await _livresService.ObtenirLivreParId(emprunt.IDLivre);
                var usager = await _usagersService.ObtenirUsagerParId(emprunt.IDUsager);
                
                var viewModel = new RetourViewModel
                {
                    LivreTitre = livre.Titre,
                    UsagerNom = usager.Nom + " " + usager.Prenoms,
                };

                return View(viewModel);
        }

        public async Task<IActionResult> Modifier(Guid id)
        {
            var retour = await _retoursService.ObtenirRetourParId(id);
            if (retour == null)
            {
                return NotFound();
            }

            return View(retour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modifier(Guid id, DateTime nouvelleDateRetour)
        {
            if (nouvelleDateRetour == default)
            {
                ModelState.AddModelError("", "La date de retour ne peut pas être invalide.");
                return View();
            }

            try
            {
                await _retoursService.ModifierRetour(id, nouvelleDateRetour);
                return RedirectToAction("Index", "Emprunts");
            }
            catch (Exception ex)
            {
                GestionErreurs.GererErreur(ex, this);
            }
            return View();
        }
    }
}
