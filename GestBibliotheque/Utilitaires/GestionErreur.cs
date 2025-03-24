using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GestBibliotheque.Utilitaires
{  

    public static class GestionErreurs
    {
        public static void GererErreur(Exception ex, Controller controller)
        {
            string messageErreur;

            if (ex is ArgumentNullException || ex is InvalidOperationException || ex is KeyNotFoundException)
            {
                messageErreur = ex.Message;
            }
            else
            {
                messageErreur = $"Une erreur inattendue est survenue. Détails : {ex.Message}";
            }


            //  -- A faire-----Journalisation
            //if (!string.IsNullOrWhiteSpace(messageErreur))
            //{
            //   
            //}

            controller.ModelState.AddModelError(string.Empty, messageErreur);
        }
    }

}
