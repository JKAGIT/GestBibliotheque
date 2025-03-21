using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestBibliotheque.Services
{
    public class EmpruntsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LivresService _livresService;

        public EmpruntsService(IUnitOfWork unitOfWork, LivresService livresService)
        {
            _unitOfWork = unitOfWork;
            _livresService = livresService;
        }
        public async Task AjouterEmprunt(Emprunts emprunt)
        {
            try
            {
                if (emprunt == null)
                throw new ArgumentNullException(nameof(emprunt), "L'emprunt ne peut pas être nul.");
           
            await _livresService.MettreAJourStock(emprunt.IDLivre, -1);

            await _unitOfWork.Emprunts.AddAsync(emprunt);
            await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                //  journalisation éventuelle a implémenter
                throw new ApplicationException("Une erreur est survenue lors de l'ajout de l'emprunt.", ex);
            }
        }

        public async Task ModifierEmprunt(Emprunts emprunt)
        {
            if (emprunt == null)
                throw new ArgumentNullException(nameof(emprunt), "L'emprunt à modifier ne peut pas être nul.");

            var empruntAModifier = await _unitOfWork.Emprunts.GetByIdAsync(emprunt.ID);
            if (empruntAModifier == null)
                throw new KeyNotFoundException($"L'emprunt avec l'ID {emprunt.ID} n'a pas été trouvé.");

            await _unitOfWork.Emprunts.UpdateAsync(emprunt);
            await _unitOfWork.CompleteAsync();
        }
        public async Task SupprimerEmprunt(Guid idEmprunt)
        {
            var empruntASupprimer = await _unitOfWork.Emprunts.GetByIdAsync(idEmprunt);
            if (empruntASupprimer == null)
                throw new KeyNotFoundException($"L'emprunt avec l'ID {idEmprunt} n'a pas été trouvé.");

            await _livresService.MettreAJourStock(empruntASupprimer.IDLivre, 1); 
            await _unitOfWork.Emprunts.DeleteAsync(empruntASupprimer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Emprunts> ObtenirEmpruntParId(Guid idEmprunt)
        {
            var emprunt = await _unitOfWork.Emprunts.GetByIdAsync(idEmprunt);
            if (emprunt == null)
                throw new KeyNotFoundException($"L'emprunt avec l'ID {idEmprunt} n'a pas été trouvé.");
            return emprunt;
        }
        public async Task<IEnumerable<Emprunts>> ObtenirEmpruntParUsager(Guid idUsager)
        {
            return await _unitOfWork.Emprunts.FindAsync(e => e.IDUsager == idUsager);
        }      
    }
}
