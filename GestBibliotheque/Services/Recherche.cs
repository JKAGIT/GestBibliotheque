﻿using GestBibliotheque.Donnee;
using GestBibliotheque.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestBibliotheque.Services
{
    public class Recherche<T> : IRecherche<T> where T : class
    {
        private readonly GestBibliothequeDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Recherche(GestBibliothequeDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }
    }
}
