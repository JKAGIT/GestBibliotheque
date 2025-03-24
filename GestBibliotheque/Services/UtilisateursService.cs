using GestBibliotheque.Models;
using GestBibliotheque.Repositories;
using GestBibliotheque.Utilitaires;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace GestBibliotheque.Services
{
    public class UtilisateursService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UtilisateursService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AjouterUtilisateur(Utilisateurs utilisateur)
        {
            ValidationService.VerifierNull(utilisateur, nameof(utilisateur), "L'utilisateur");

            if (await _unitOfWork.Utilisateurs.EntiteExiste(u => u.Matricule == utilisateur.Matricule))
                throw new InvalidOperationException(string.Format(ErreurMessage.EntiteExisteDeja, "Un utilisateur", utilisateur.Matricule));

            if (await _unitOfWork.Utilisateurs.EntiteExiste(u => u.Courriel == utilisateur.Courriel))
                throw new InvalidOperationException(string.Format(ErreurMessage.EntiteExisteDeja, "Un utilisateur", utilisateur.Courriel));


            await _unitOfWork.Utilisateurs.AddAsync(utilisateur);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ModifierUtilisateur(Utilisateurs utilisateur)
        {
            ValidationService.VerifierNull(utilisateur, nameof(utilisateur), "L'utilisateur");

            var utilisateurAModifier = await _unitOfWork.Utilisateurs.GetByIdAsync(utilisateur.ID);
            ValidationService.EnregistrementNonTrouve(utilisateurAModifier, "Utilisateurs", utilisateur.ID);

            await _unitOfWork.Utilisateurs.UpdateAsync(utilisateur);
            await _unitOfWork.CompleteAsync();
        }

        public async Task SupprimerUtilisateur(Guid idUtilisateur)
        {
            var utilisateurASupprimer = await _unitOfWork.Utilisateurs.GetByIdAsync(idUtilisateur);
            ValidationService.EnregistrementNonTrouve(utilisateurASupprimer, "Utilisateurs", idUtilisateur);

            await _unitOfWork.Utilisateurs.DeleteAsync(utilisateurASupprimer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Utilisateurs> ObtenirUtilisateurParId(Guid idUtilisateur)
        {
            var utilisateur = await _unitOfWork.Utilisateurs.GetByIdAsync(idUtilisateur);
            ValidationService.EnregistrementNonTrouve(utilisateur, "Utilisateurs", idUtilisateur);
            return utilisateur;
        }

        public async Task<IEnumerable<Utilisateurs>> ObtenirUtilisateurs()
        {
            try
            {
                return await _unitOfWork.Utilisateurs.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, "Utilisateurs"), ex);
            }
        }
    }
}
