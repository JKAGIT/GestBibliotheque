using GestBibliotheque.Models;
using GestBibliotheque.Services;
using GestBibliotheque.Utilitaires;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestBibliotheque.Controllers
{
    public class UsagersController : Controller
    {
        private readonly UsagersService _usagersService;
        private readonly EmpruntsService _empruntsService;
        private readonly RetoursService _retourService;

        public UsagersController(UsagersService usagersService, EmpruntsService empruntsService, RetoursService retourService   )
        {
            _usagersService = usagersService;
            _empruntsService = empruntsService;
            _retourService = retourService;
        }

        public async Task<IActionResult> Index()
        {
            var usagers = await _usagersService.ObtenirUsagers();
            return View(usagers);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var usager = await _usagersService.ObtenirUsagerParId(id);
            if (usager == null) return NotFound();


            var empruntsActifs = await _retourService.ObtenirEmpruntsActif(id);

            var viewModel = new UsagerDetailsViewModel
            {
                UsagerId = usager.ID,
                Nom = usager.Nom,
                Prenoms = usager.Prenoms,
                Courriel = usager.Courriel,
                Telephone = usager.Telephone,
                Emprunts = empruntsActifs
            };

            return View(viewModel);

            //return View(usager);
        }
       
        public IActionResult Ajouter()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajouter(Usagers usager)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _usagersService.AjouterUsager(usager);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    GestionErreurs.GererErreur(ex, this);
                }
            }
            return View(usager);
        }
       
        public async Task<IActionResult> Modifier(Guid id)
        {
            var usager = await _usagersService.ObtenirUsagerParId(id);
            if (usager == null)
            {
                return NotFound();
            }
            return View(usager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modifier(Usagers usager)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _usagersService.ModifierUsager(usager);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    GestionErreurs.GererErreur(ex, this);
                }
            }
            return View(usager);
        }

        public async Task<IActionResult> Supprimer(Guid id)
        {
            var usager = await _usagersService.ObtenirUsagerParId(id);
            if (usager == null)
            {
                return NotFound();
            }

            return View(usager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerConfirmation(Guid id)
        {
            try
            {
                await _usagersService.SupprimerUsager(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                GestionErreurs.GererErreur(ex, this);
                var usager = await _usagersService.ObtenirUsagerParId(id);
                return View("Supprimer", usager);
            }
        }












































        //// GET: UsagersController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: UsagersController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: UsagersController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: UsagersController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UsagersController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: UsagersController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UsagersController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: UsagersController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
