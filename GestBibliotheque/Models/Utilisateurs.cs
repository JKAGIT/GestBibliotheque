﻿using System.ComponentModel.DataAnnotations;

namespace GestBibliotheque.Models
{
    public class Utilisateurs
    {
        [Key]
        public Guid ID { get; set; } 

        [Required(ErrorMessage = "Le matricule est obligatoire.")]
        [StringLength(10)]
        public string Matricule { get; set; } 

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50)]
        public string Nom { get; set; } 

        [Required(ErrorMessage = "Les prénoms sont obligatoires.")]
        [StringLength(100)]
        public string Prenoms { get; set; } 

        [Required(ErrorMessage = "Le courriel est obligatoire.")]
        [EmailAddress(ErrorMessage = "Veuillez fournir un courriel valide.")]
        public string Courriel { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone est requis.")]
        [RegularExpression(@"^(\d{3})\s?(\d{3})\s?(\d{4})$", ErrorMessage = "Le numéro de téléphone doit être au format 418 256 1234 ou 4182561234.")]
        [StringLength(12, ErrorMessage = "Le numéro de téléphone ne doit pas dépasser 12 caractères.")]
        [MinLength(10, ErrorMessage = "Le numéro de téléphone doit comporter au moins 10 chiffres.")]
        public string Telephone { get; set; }   

    }
}
