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
        public DbSet<Utilisateurs> Utilisateurs { get; set; }
        public DbSet<Usagers> Usagers { get; set; }
        public DbSet<Emprunts> Emprunts { get; set; }
        public DbSet<Retours> Retours { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Livres>()
                .HasOne(l => l.Categories)
                .WithMany(c => c.Livres)
                .HasForeignKey(l => l.IDCategorie)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Emprunts>()
                .HasOne(e => e.Usager)  
                .WithMany(u => u.Emprunts)  
                .HasForeignKey(e => e.IDUsager)  
                .OnDelete(DeleteBehavior.Cascade);  


            modelBuilder.Entity<Emprunts>()
                .HasOne(e => e.Livre) 
                .WithMany(l => l.Emprunts)  
                .HasForeignKey(e => e.IDLivre)  
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Retours>()
               .HasOne(r => r.Emprunt)
               .WithOne(e => e.Retours)
               .HasForeignKey<Retours>(r => r.IDEmprunt)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}


