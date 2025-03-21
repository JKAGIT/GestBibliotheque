using GestBibliotheque.Models;
using GestBibliotheque.Repositories;

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
            if (utilisateur == null)
                throw new ArgumentNullException(nameof(utilisateur), "L'utilisateur ne peut pas être nul.");


            var utilisateurExiste = await _unitOfWork.Utilisateurs.FindAsync(u => u.Matricule == utilisateur.Matricule);
            if (utilisateurExiste.Any())
            {
                throw new InvalidOperationException($"Un utilisateur avec le matricule {utilisateur.Matricule} existe déjà.");
            }           

            await _unitOfWork.Utilisateurs.AddAsync(utilisateur);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ModifierUtilisateur(Utilisateurs utilisateur)
        {
            if (utilisateur == null)
            {
                throw new ArgumentNullException(nameof(utilisateur), "L'utilisateur à modifier ne peut pas être nul.");
            }

            var utilisateurAModifier = await _unitOfWork.Utilisateurs.GetByIdAsync(utilisateur.ID);
            if (utilisateurAModifier == null)
            {
                throw new KeyNotFoundException($"L'utilisateur avec l'ID {utilisateur.ID} n'a pas été trouvé.");
            }

            await _unitOfWork.Utilisateurs.UpdateAsync(utilisateur);
            await _unitOfWork.CompleteAsync();
        }

        public async Task SupprimerUtilisateur(Guid idUtilisateur)
        {
            var utilisateurASupprimer = await _unitOfWork.Utilisateurs.GetByIdAsync(idUtilisateur);
            if (utilisateurASupprimer == null)
            {
                throw new KeyNotFoundException("L'utilisateur à supprimer n'a pas été trouvé.");
            }

            await _unitOfWork.Utilisateurs.DeleteAsync(utilisateurASupprimer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Utilisateurs> ObtenirUtilisateurParId(Guid idUtilisateur)
        {
            var utilisateur = await _unitOfWork.Utilisateurs.GetByIdAsync(idUtilisateur);
            if (utilisateur == null)
                throw new KeyNotFoundException($"L'utilisateur avec l'ID {idUtilisateur} n'a pas été trouvé.");
            return utilisateur;
        }

        public async Task<IEnumerable<Utilisateurs>> ObtenirUtilisateurs()
        {
            return await _unitOfWork.Utilisateurs.GetAllAsync();
        }

    }
}
