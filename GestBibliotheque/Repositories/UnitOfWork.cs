using GestBibliotheque.Donnee;
using GestBibliotheque.Models;
using Microsoft.EntityFrameworkCore;

namespace GestBibliotheque.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GestBibliothequeDbContext _context;
        private IGenericRepository<Livres> _livres;
        private IGenericRepository<Categories> _categories;

        public UnitOfWork(GestBibliothequeDbContext context)
        {
            _context = context;
            _livres = new GenericRepository<Livres>(_context);
            _categories  = new GenericRepository<Categories>(_context);

        }
        public IGenericRepository<Livres> Livres => _livres;
        public IGenericRepository<Categories> Categories => _categories;
           
        public async Task<int> CompleteAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync(); 
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(); 
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
