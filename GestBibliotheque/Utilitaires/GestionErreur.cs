using Microsoft.AspNetCore.Mvc;

namespace GestBibliotheque.Utilitaires
{
        public static class GestionErreurs
        {
            public static void GererErreur(Exception ex, Controller controller)
            {
                string messageErreur = ex switch
                {
                    ArgumentNullException or KeyNotFoundException => ex.Message,
                    _ => $"Une erreur inattendue est survenue : {ex.Message}"
                };

                controller.ModelState.AddModelError(string.Empty, messageErreur);
            }
        }

}
