using GestBibliotheque.Models;
using Microsoft.EntityFrameworkCore;

namespace GestBibliotheque.Donnee
{
    public class GestBibliothequeDbContext : DbContext
    {
        public GestBibliothequeDbContext(DbContextOptions<GestBibliothequeDbContext> options) : base(options)
        {
        }
        public DbSet<Livres> Livres { get; set; }
        public DbSet<Categories> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Livres>()
                .HasOne(l => l.Categories)
                .WithMany(c => c.Livres)
                .HasForeignKey(l => l.IDCategorie)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
