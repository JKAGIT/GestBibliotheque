using GestBibliotheque.Repositories;
using GestBibliotheque.Donnee;
using Microsoft.EntityFrameworkCore;
using GestBibliotheque.Models;
using GestBibliotheque.Utilitaires;

namespace GestBibliotheque.Services
{
    public class GenerateurMatriculeUnique
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecherche<Utilisateurs> _recherche;

        public GenerateurMatriculeUnique(IUnitOfWork unitOfWork, IRecherche<Utilisateurs> recherche)
        {
            _unitOfWork = unitOfWork;
            _recherche = recherche;
        }

        public async Task<string> GenererMatriculeUnique()
        {
            string matricule;
            bool matriculeExiste;
            try
            {
                do
                {
                    matricule = GenererMatriculeAleatoire();
                    var result = await _recherche.FindAsync(u => u.Matricule == matricule);
                    matriculeExiste = result.Any();
                }
                while (matriculeExiste);

                return matricule;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErreurMessage.ErreurRecherche, ""), ex);
            }
        }

        private string GenererMatriculeAleatoire()
        {
            var prefixe = "MATR-";
            var randomNumber = new Random().Next(10000, 100000);
            var matricule = $"{prefixe}{randomNumber}";

            return matricule;
        }

    }
}

