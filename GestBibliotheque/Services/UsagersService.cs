using GestBibliotheque.Models;
using GestBibliotheque.Repositories;

namespace GestBibliotheque.Services
{
    public class UsagersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsagersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AjouterUsager(Usagers usager)
        {
            if (usager == null)
                throw new ArgumentNullException(nameof(usager), "L'usager ne peut pas être nul.");

            var usagerExistant = await _unitOfWork.Usagers.FindAsync(u => u.Courriel == usager.Courriel);
            if (usagerExistant.Any())
                throw new InvalidOperationException($"Un usager avec l'email {usager.Courriel} existe déjà.");


            await _unitOfWork.Usagers.AddAsync(usager);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ModifierUsager(Usagers usager)
        {
            if (usager == null)
                throw new ArgumentNullException(nameof(usager), "L'usager à modifier ne peut pas être nul.");

            var usagerAModifier = await _unitOfWork.Usagers.GetByIdAsync(usager.ID);
            if (usagerAModifier == null)
                throw new KeyNotFoundException($"L'usager avec l'ID {usager.ID} n'a pas été trouvé.");

            await _unitOfWork.Usagers.UpdateAsync(usager);
            await _unitOfWork.CompleteAsync();
        }

        public async Task SupprimerUsager(Guid idUsager)
        {
            var usagerASupprimer = await _unitOfWork.Usagers.GetByIdAsync(idUsager);
            if (usagerASupprimer == null)
                throw new KeyNotFoundException($"L'usager avec l'ID {idUsager} n'a pas été trouvé.");

            var empruntsActifs = await _unitOfWork.Emprunts.FindAsync(e => e.IDUsager == idUsager && e.Retours == null);
            if (empruntsActifs.Any())
                throw new InvalidOperationException("L'usager ne peut pas être supprimé tant qu'il a des emprunts actifs.");


            await _unitOfWork.Usagers.DeleteAsync(usagerASupprimer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Usagers> ObtenirUsagerParId(Guid idUsager)
        {
            var usager = await _unitOfWork.Usagers.GetByIdAsync(idUsager);
            if (usager == null)
                throw new KeyNotFoundException($"L'usager avec l'ID {idUsager} n'a pas été trouvé.");
            return usager;
        }

        public async Task<IEnumerable<Usagers>> ObtenirUsagers()
        {
            return await _unitOfWork.Usagers.GetAllAsync();
        }
    }
}
