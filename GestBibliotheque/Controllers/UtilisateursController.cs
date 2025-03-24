﻿using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using GestBibliotheque.Services;
using GestBibliotheque.Utilitaires;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestBibliotheque.Controllers
{
    public class UtilisateursController : Controller
    {
        private readonly UtilisateursService _utilisateursService;
        private readonly GenerateurMatriculeUnique _generateurMatricule;

        public UtilisateursController(UtilisateursService utilisateursService, GenerateurMatriculeUnique generateurMatricule)
        {
            _utilisateursService = utilisateursService;
            _generateurMatricule = generateurMatricule;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var utilisateurs = await _utilisateursService.ObtenirUtilisateurs();
            return View(utilisateurs);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var utilisateur = await _utilisateursService.ObtenirUtilisateurParId(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            return View(utilisateur);
        }
        public async Task<IActionResult> Ajouter()
        {
            var utilisateur = new Utilisateurs
            {
                Matricule = await _generateurMatricule.GenererMatriculeUnique()
            };
            return View(utilisateur); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajouter(Utilisateurs utilisateur)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _utilisateursService.AjouterUtilisateur(utilisateur);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    GestionErreurs.GererErreur(ex, this);
                }
            }
            ViewBag.Utilisateurs = await _utilisateursService.ObtenirUtilisateurs();
            return View(utilisateur);
        }

        public async Task<IActionResult> Modifier(Guid id)
        {
            var utilisateur = await _utilisateursService.ObtenirUtilisateurParId(id);
            if (utilisateur == null)
            {
                return NotFound(); 
            }
            return View(utilisateur); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modifier(Utilisateurs utilisateur)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _utilisateursService.ModifierUtilisateur(utilisateur);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    GestionErreurs.GererErreur(ex, this);
                }
            }

            return View(utilisateur); 
        }

        public async Task<IActionResult> Supprimer(Guid id)
        {
            var utilisateur = await _utilisateursService.ObtenirUtilisateurParId(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            return View(utilisateur); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerConfirmation(Guid id)
        {
            try
            {
                await _utilisateursService.SupprimerUtilisateur(id);
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
