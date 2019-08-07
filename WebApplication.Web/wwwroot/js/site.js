// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $(".ingredient-name-dropdown").chosen();
});

function addNextIngredientField() {
    let container = document.getElementById("ingredient-container");
    let ingredient = document.importNode(document.querySelector("template").content, true);

    container.appendChild(document.createElement("br"));
    container.appendChild(ingredient);
    $(".ingredient-name-dropdown").chosen();
}

let nextIngredientButton = document.getElementById("nextIngredientButton");

nextIngredientButton.addEventListener("click", (event) => {
    event.preventDefault();
    event.stopPropagation();
    addNextIngredientField();
});

