﻿using GestBibliotheque.Models;

namespace GestBibliotheque.Repositories
{
    public interface IRetours: IGenericRepository<Retours>
    {
        Task<IEnumerable<RetourViewModel>> ObtenirEmpruntsActif(Guid? usagerId = null);
        Task<IEnumerable<RetourViewModel>> ObtenirEmpruntsInActif();
        IEnumerable<RetourViewModel> FiltrerEmpruntsParRecherche(IEnumerable<RetourViewModel> emprunts, string recherche);
    }
}
