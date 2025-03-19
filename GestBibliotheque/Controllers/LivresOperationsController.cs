using GestBibliotheque.Models;
using GestBibliotheque.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestBibliotheque.Controllers
{
    public class LivresOperationsController : Controller
    {
        private readonly LivresService _livresService;

        public LivresOperationsController(LivresService livresService)
        {
            _livresService = livresService;
        }

        private async Task ChargerLivres()
        {
            var livres = await _livresService.ObtenirLivres();
            ViewData["Livres"] = new SelectList(livres, "ID", "Titre");
        }

        [HttpGet]
        public async Task<IActionResult> AjouterStock()
        {
            await ChargerLivres();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AjouterStock(Guid idLivre, int quantite)
        {
            try
            {
                await _livresService.MettreAJourStock(idLivre, quantite);
                return RedirectToAction("Index", "Livres");
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await ChargerLivres();
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> VerifierDisponibilite()
        {
            await ChargerLivres();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifierDisponibilite(Guid idLivre)
        {
            await ChargerLivres();
            var estDisponible = await _livresService.EstDisponible(idLivre);
            ViewData["MessageDisponibilite"] = estDisponible ? "Le livre est disponible !" : "Le livre n'est pas disponible.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LivresEnStock()
        {
            var livresEnStock = await _livresService.ObtenirLivresEnStock();
            return View(livresEnStock);
        }
    }

}
