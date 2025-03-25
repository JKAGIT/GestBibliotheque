using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using GestBibliotheque.Utilitaires;

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
            ValidationService.VerifierNull(usager, nameof(usager), "L'usager");

            if (await _unitOfWork.Usagers.EntiteExiste(u => u.Courriel == usager.Courriel))
                throw new InvalidOperationException(string.Format(ErreurMessage.EntiteExisteDeja, "Un usager", usager.Courriel));

            await _unitOfWork.Usagers.AddAsync(usager);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ModifierUsager(Usagers usager)
        {
            ValidationService.VerifierNull(usager, nameof(usager), "L'usager");

            var usagerAModifier = await _unitOfWork.Usagers.GetByIdAsync(usager.ID);
            ValidationService.EnregistrementNonTrouve(usagerAModifier, "Usagers", usager.ID);

            await _unitOfWork.Usagers.UpdateAsync(usager);
            await _unitOfWork.CompleteAsync();
        }

        public async Task SupprimerUsager(Guid idUsager)
        {
            var usagerASupprimer = await _unitOfWork.Usagers.GetByIdAsync(idUsager);
            ValidationService.EnregistrementNonTrouve(usagerASupprimer, "Usagers", idUsager);

            var empruntsActifs = await _unitOfWork.Emprunts.FindAsync(e => e.IDUsager == idUsager && e.Retours == null);
            if (empruntsActifs.Any())
                throw new InvalidOperationException(string.Format(ErreurMessage.ErreurSuppressionEntiteLiee, "un usager", "emprunts actifs"));

            await _unitOfWork.Usagers.DeleteAsync(usagerASupprimer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Usagers> ObtenirUsagerParId(Guid idUsager)
        {
            var usager = await _unitOfWork.Usagers.GetByIdAsync(idUsager);
            ValidationService.EnregistrementNonTrouve(usager, "Usagers", idUsager);
            return usager;
        }

        public async Task<IEnumerable<Usagers>> ObtenirUsagers()
        {
            try
            {
                return await _unitOfWork.Usagers.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Usagers"), ex);
            }
        }
    }
}
