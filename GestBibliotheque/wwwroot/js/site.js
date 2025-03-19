// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Fonction de filtrage pour les livres dans le DropDownList
function filtreLivres() {
    var valeurDeRecherche = document.getElementById("livreRechercher").value.toLowerCase();
    var options = document.getElementById("livreId").options;

    for (var i = 0; i < options.length; i++) {
        var optionText = options[i].text.toLowerCase();
        if (optionText.includes(valeurDeRecherche)) {
            options[i].style.display = 'block'; // Affiche l'option correspondante
        } else {
            options[i].style.display = 'none'; // Cache l'option non correspondante
        }
    }
}
