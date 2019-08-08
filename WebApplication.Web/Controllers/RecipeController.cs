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

        public IActionResult Detail(string id = "1")
        {
            Recipe recipe = recipeDAL.GetRecipeById(id);

            return View(recipe);
        }

        [HttpGet]
        public IActionResult Create()
        {            
            string recipeStatus = HttpContext.Session.GetString("NewRecipeStatus");
            ViewBag.RecipeStatus = recipeStatus;

            ViewBag.ExistingIngredients = ingredientDAL.GetIngredients();
            ViewBag.Units = ingredientDAL.GetUnits();
            ViewBag.Numbers = ingredientDAL.GetNumbers();
            ViewBag.Fractions = ingredientDAL.GetFractions();
            
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Recipe recipe, List<Ingredient> ingredients)
        {
            if(!ModelState.IsValid)
            {
                HttpContext.Session.SetString("NewRecipeStatus", "Partial");

                return RedirectToAction("Create");
            }

            recipe.Ingredients = ingredients;

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
            foreach(string str in newIngredients)
            {
                ingredientDAL.AddIngredient(str);
            }

            return RedirectToAction("Create");
        }

        public IActionResult Modify(string id = "1")
        {
            Recipe recipe = recipeDAL.GetRecipeById(id);

            return View(recipe);
        }

        [HttpGet]
        public IActionResult AllRecipes()
        {
            if (authProvider.GetCurrentUser() == null)
            {
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

            return RedirectToAction("AllRecipes");
        }
    }
}