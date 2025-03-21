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

// Status des Emprunts + badge approprié
function afficherStatutEmprunt(datePrevu, elementId) {
    if (typeof datePrevu === 'string') {
        datePrevu = new Date(datePrevu);
    }

    const maintenant = new Date();
    const element = document.getElementById(elementId);

    if (datePrevu < maintenant) {
        element.innerHTML = '<span class="badge bg-danger">En retard</span>';
    } else {
        element.innerHTML = '<span class="badge bg-success">À jour</span>';
    }
}

// Fonction pour initialiser plusieurs statuts d'emprunts en utilisant des attributs data
function initialiserStatutsEmprunts() {
    document.querySelectorAll('[data-date-prevu]').forEach(function (element) {
        const datePrevu = new Date(element.getAttribute('data-date-prevu'));

        if (datePrevu < new Date()) {
            element.innerHTML = '<span class="badge bg-danger">En retard</span>';
        } else {
            element.innerHTML = '<span class="badge bg-success">À jour</span>';
        }
    });
}

// Exécuter l'initialisation au chargement de la page
document.addEventListener('DOMContentLoaded', function () {
    initialiserStatutsEmprunts();
});