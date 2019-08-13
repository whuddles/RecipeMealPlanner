// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Create Recipe - Begin

$(function () {
    $(".ingredient-name-dropdown").chosen();
});

let ingredientCount = parseInt($("#visibleIngredients").attr("value"));
let elementsPerIngredient = parseInt($("#elementsPerIngredient").attr("value"));
let elementsPerRecipe = parseInt($("#elementsPerRecipe").attr("value"));
let extraIngredientIndex = ingredientCount * elementsPerIngredient + elementsPerRecipe;

function addNextIngredientField() {
    let ingredientContainer = document.getElementById("ingredient-container");
    let ingredient = document.importNode(document.querySelector("template").content, true);

    ingredientCount++;

    ingredient.querySelector(".ingredient-number-dropdown").setAttribute('name', 'ModelList[' + parseInt(extraIngredientIndex) + ']');
    ingredient.querySelector(".ingredient-fraction-dropdown").setAttribute('name', 'ModelList[' + parseInt(extraIngredientIndex + 1) + ']');
    ingredient.querySelector(".ingredient-unit-dropdown").setAttribute('name', 'ModelList[' + parseInt(extraIngredientIndex + 2) + ']');
    ingredient.querySelector(".ingredient-name-dropdown").setAttribute('name', 'ModelList[' + parseInt(extraIngredientIndex + 3) + ']');

    ingredient.querySelector(".ingredient-number-dropdown").setAttribute('id', 'ModelList[' + parseInt(extraIngredientIndex) + ']');
    ingredient.querySelector(".ingredient-fraction-dropdown").setAttribute('id', 'ModelListt[' + parseInt(extraIngredientIndex + 1) + ']');
    ingredient.querySelector(".ingredient-unit-dropdown").setAttribute('id', 'ModelList[' + parseInt(extraIngredientIndex + 2) + ']');
    ingredient.querySelector(".ingredient-name-dropdown").setAttribute('id', 'ModelList[' + parseInt(extraIngredientIndex + 3) + ']');

    extraIngredientIndex += elementsPerIngredient;

    ingredientContainer.appendChild(document.createElement("br"));
    ingredientContainer.appendChild(ingredient);
    //$("#ingredient-container").append('<div class="ingredient-description form-inline"><select class="ingredient-number-dropdown col-sm-1" id="IngredientNumber' + ingredientCount + '" name="Model.Ingredients[' + ingredientCount + '].Number"><option selected disabled >...</option>@for (int j = 0; j < ViewBag.Numbers.Count; j++){<option>@ViewBag.Numbers[j]</option>}</select ><select class="ingredient-fraction-dropdown col-sm-1" id="IngredientFraction' + ingredientCount + '" name="Model.Ingredients[' + ingredientCount + '].Fraction"><option selected disabled>...</option>\n@for (int j = 0; j < ViewBag.Fractions.Count; j++)\n{<option>@ViewBag.Fractions[j]</option>}</select><select class="ingredient-unit-dropdown col-sm-1" id="IngredientUnit' + ingredientCount + '" name="Model.Ingredients[' + ingredientCount + '].Unit"><option selected disabled>...</option>\n@for (int j = 0; j < ViewBag.Units.Count; j++)\n{<option>@ViewBag.Units[j]</option>}</select><select class= "ingredient-name-dropdown col-sm-8" id="IngredientName' + ingredientCount + '" name="Model.Ingredients[' + ingredientCount + '].Name"><option selected disabled>Choose an ingredient...</option>\n@for (int j = 0; j < ViewBag.ExistingIngredients.Count; j++)\n{<option>@ViewBag.ExistingIngredients[j].Name</option>}</select><button class="btn btn-danger ingredient-delete-button col-sm-1">Delete Ingredient</button></ div>');
    $(".ingredient-name-dropdown").chosen();
}

if (document.getElementById("nextIngredientButton") != null) {
    let nextIngredientButton = document.getElementById("nextIngredientButton");

    nextIngredientButton.addEventListener("click", (event) => {
        event.preventDefault();
        event.stopPropagation();
        addNextIngredientField();
    });
}

// Create Recipe - End

// Create Meal - Begin

$(function () {
    $(".recipe-name-dropdown").chosen();
});

let recipeCount = parseInt($("#visibleRecipes").attr("value"));
let elementsPerMeal = parseInt($("#elementsPerMeal").attr("value"));
let elementsPerMealRecipe = parseInt($("#elementsPerRecipe").attr("value"));
let extraRecipeIndex = recipeCount * elementsPerMealRecipe + elementsPerMeal;

function addNextRecipeField() {
    let recipeContainer = document.getElementById("recipe-container");
    let recipe = document.importNode(document.querySelector("#addRecipeTemplate").content, true);

    recipeCount++;

    recipe.querySelector(".recipe-name-dropdown").setAttribute('name', 'ModelList[' + parseInt(extraRecipeIndex) + ']');
    recipe.querySelector(".recipe-name-dropdown").setAttribute('id', 'ModelList[' + parseInt(extraRecipeIndex) + ']');

    recipeContainer.appendChild(document.createElement("br"));
    recipeContainer.appendChild(recipe);
    $(".recipe-name-dropdown").chosen();
}
if (document.getElementById("nextRecipeButton") != null) {
    let nextRecipeButton = document.getElementById("nextRecipeButton");

    nextRecipeButton.addEventListener("click", (event) => {
        event.preventDefault();
        event.stopPropagation();
        addNextRecipeField();
    });
}

// Create Meal - End

// Create MealPlan - Begin

$(function () {
    $(".meal-name-dropdown").chosen();
})

let mealDetailButton = document.getElementsByClassName("show-meal-details");

let dayCount = parseInt($("#visibleDays").attr("value"));
let elementsPerPlan = parseInt($("#elementsPerPlan").attr("value"));
let elementsPerDay = parseInt($("#elementsPerDay").attr("value"));
let extraDayIndex = dayCount * elementsPerDay + elementsPerPlan;

function addNextDay() {
    let mealPlanContainer = document.getElementById("mealPlan-container");
    let day = document.importNode(document.querySelector("#dayTemplate").content, true);

    dayCount++;

    day.querySelector(".breakfast-select").setAttribute('name', 'ModelList[' + parseInt(extraDayIndex) + ']');
    day.querySelector(".lunch-select").setAttribute('name', 'ModelList[' + parseInt(extraDayIndex + 1) + ']');
    day.querySelector(".dinner-select").setAttribute('name', 'ModelList[' + parseInt(extraDayIndex + 2) + ']');

    day.querySelector(".breakfast-select").setAttribute('id', 'ModelList[' + parseInt(extraDayIndex) + ']');
    day.querySelector(".lunch-select").setAttribute('id', 'ModelList[' + parseInt(extraDayIndex + 1) + ']');
    day.querySelector(".dinner-select").setAttribute('id', 'ModelList[' + parseInt(extraDayIndex + 2) + ']');

    day.querySelector(".day-number").innerHTML = "Day " + dayCount;

    mealPlanContainer.appendChild(document.createElement("br"));
    mealPlanContainer.appendChild(day);

    $(".breakfast-select").chosen();
    $(".lunch-select").chosen();
    $(".dinner-select").chosen();
}

if (document.getElementById("nextDayButton") != null) {
    let nextDayButton = document.getElementById("nextDayButton");

    nextDayButton.addEventListener("click", (event) => {
        event.preventDefault();
        event.stopPropagation();
        addNextDay();
    })
}

// Create MealPlan - End




//let ingredient = {
//    Number: "",
//    Fraction: "",
//    Unit: "",
//    Name: ""
//};

//let ingredientArr = [];
//let ingredientNumber = document.querySelector(".ingredient-number-dropdown");
//let ingredientFraction = document.querySelector(".ingredient-fraction-dropdown");
//let ingredientUnit = document.querySelector(".ingredient-unit-dropdown");
//let ingredientName = document.querySelector(".ingredient-name-dropdown");

//ingredientNumber.addEventListener("change", (event) => {
//    ingredient.Number = event.target.value;
//});
//ingredientFraction.addEventListener("change", (event) => {
//    ingredient.Fraction = event.target.value;
//});
//ingredientUnit.addEventListener("change", (event) => {
//    ingredient.Unit = event.target.value;
//});
//ingredientName.addEventListener("change", (event) => {
//    ingredient.Name = event.target.value;
//    ingredientArr.push(ingredient);
//});

//let recipe = {
//    Name: "",
//    Description: "",
//    Instructions: "",
//    PrepTime: 0,
//    CookTime: 0,
//}

//let recipeName = document.querySelector(".recipe-name");
//let recipeDescription = document.querySelector(".recipe-description");
//let recipeInstructions = document.querySelector(".recipe-instructions");
//let recipePrepTime = document.querySelector(".recipe-preptime");
//let recipeCookTime = document.querySelector(".recipe-cooktime");

//recipeName.addEventListener("change", (event) => {
//    recipe.Name = event.target.value;
//});
//recipeDescription.addEventListener("change", (event) => {
//    recipe.Description = event.target.value;
//})
//recipeInstructions.addEventListener("change", (event) => {
//    recipe.Instructions = event.target.value;
//});
//recipePrepTime.addEventListener("change", (event) => {
//    recipe.PrepTime = event.target.value;
//});
//recipeCookTime.addEventListener("change", (event) => {
//    recipe.CookTime = event.target.value;
//});

//let submitButton = document.querySelector(".submit-button");

//submitButton.addEventListener("click", () => {
//    let ingredientsToPost = JSON.stringify(ingredientArr);
//    let recipeToPost = JSON.stringify(recipe);

//    $.ajax({
//        type: "POST",
//        url: "Create",
//        datatype: JSON,
//        data: {
//                recipe: recipeToPost,
//                ingredients: ingredientsToPost,
//            },
//        traditional: true
//    });
//})