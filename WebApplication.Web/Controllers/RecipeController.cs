using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Web.Controllers
{
    public class RecipeController : Controller
    {
        private IRecipeDAL recipeDAL;
        private IIngredientDAL ingredientDAL;
        private IAuthProvider authProvider;

        public RecipeController(IRecipeDAL recipeDAL, IIngredientDAL ingredientDAL, IAuthProvider authProvider)
        {
            this.recipeDAL = recipeDAL;
            this.ingredientDAL = ingredientDAL;
            this.authProvider = authProvider;
        }

        public IActionResult Detail(int id = 1)
        {
            Recipe recipe = recipeDAL.GetRecipeById(id);

            return View(recipe);
        }

        [HttpGet]
        public IActionResult Create(int id = 0)
        {
            if (authProvider.GetCurrentUser() == null)
            {
                TempData["ErrorMessage"] = "You must login to Create A Recipe!";
                return RedirectToAction("Login", "Account");
            }

            string recipeStatus = HttpContext.Session.GetString("NewRecipeStatus");
            ViewBag.RecipeStatus = recipeStatus;

            ViewBag.ExistingIngredients = ingredientDAL.GetIngredients();
            ViewBag.Units = ingredientDAL.GetUnits();
            ViewBag.Numbers = ingredientDAL.GetNumbers();
            ViewBag.Fractions = ingredientDAL.GetFractions();

            Recipe recipe = recipeDAL.GetRecipeById(id);
            RecipeViewModel viewModel = new RecipeViewModel();
            viewModel.ModelRecipe = recipe;
            ViewBag.IngredientCount = recipe.Ingredients.Count;

            return View("Create", viewModel);
        }

        [HttpPost]
        public IActionResult Create(RecipeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Session.SetString("NewRecipeStatus", "Partial");

                return RedirectToAction("Create");
            }

            List<Ingredient> ingredientList = new List<Ingredient>();
            List<string> ingredientStrArr = viewModel.ModelList;

            Recipe recipe = new Recipe
            {
                Name = ingredientStrArr[0],
                Description = ingredientStrArr[1],
                Instructions = ingredientStrArr[2],
                PrepTime = Convert.ToInt32(ingredientStrArr[3]),
                CookTime = Convert.ToInt32(ingredientStrArr[4])
            };

            for (int i = 5; i < ingredientStrArr.Count; i += 4)
            {
                Ingredient ingredient = new Ingredient
                {
                    Number = ingredientStrArr[i],
                    Fraction = ingredientStrArr[i + 1],
                    Unit = ingredientStrArr[i + 2],
                    Name = ingredientStrArr[i + 3]
                };

                ingredientList.Add(ingredient);
            }

            recipe.Ingredients = ingredientList;

            int recipeId = recipeDAL.AddRecipe(recipe);

            if(recipeId > 0)
            {
                HttpContext.Session.SetString("NewRecipeStatus", "Complete");

                return RedirectToAction("Detail", "Recipe", new { id = Convert.ToString(recipeId) } );
            }

            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult AddIngredient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddIngredient(string newIngredient)
        {
            string[] newIngredients = newIngredient.Split(", ");
            List<Ingredient> existingIngredients = ingredientDAL.GetIngredients();
            string[] existingIngredientsArr = new string[existingIngredients.Count()];
            for(int i = 0; i < existingIngredients.Count(); i++)
            {
                existingIngredientsArr[i] = existingIngredients[i].Name;
            }

            foreach (string str in newIngredients)
            {
                if (!existingIngredientsArr.Contains(str))
                {
                    ingredientDAL.AddIngredient(str);
                }
            }

            return RedirectToAction("Create");
        }

        public IActionResult Modify(int id = 0)
        {
            return RedirectToAction("Create", "Recipe", new { id });
        }

        [HttpGet]
        public IActionResult AllRecipes()
        {
            if (authProvider.GetCurrentUser() == null)
            {
                TempData["ErrorMessage"] = "You must login to View All Recipes!";
                return RedirectToAction("Login", "Account");
            }

            List<Recipe> allRecipes = recipeDAL.GetAllRecipes();

                return View(allRecipes);
            
        }

        [HttpPost]
        public IActionResult AddRecipeToUser(int recipeId)
        {
            User user = authProvider.GetCurrentUser();
            recipeDAL.AddRecipeToUserAccount(recipeId, user.Id);

            return RedirectToAction("MyRecipes", "Recipe", new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult MyRecipes(int userId)
        {
            User user = authProvider.GetCurrentUser();
            if (authProvider.GetCurrentUser() == null)
            {
                TempData["ErrorMessage"] = "You must login to View Your Recipes, Dummy!";
                return RedirectToAction("Login", "Account");
            }
            else if (authProvider.GetCurrentUser().Id != userId)
            {
                return RedirectToAction("MyRecipes", "Recipe", new { userId = user.Id });
            }

            List<Recipe> userRecipes = recipeDAL.GetRecipesByUserId(userId);

            return View(userRecipes);
        }
    }
}