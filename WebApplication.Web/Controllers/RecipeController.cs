using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using Newtonsoft.Json;

namespace WebApplication.Web.Controllers
{
    public class RecipeController : Controller
    {
        private IRecipeDAL recipeDAL;
        private IIngredientDAL ingredientDAL;

        public RecipeController(IRecipeDAL recipeDAL, IIngredientDAL ingredientDAL)
        {
            this.recipeDAL = recipeDAL;
            this.ingredientDAL = ingredientDAL;
        }

        public IActionResult Detail(int id = 1)
        {
            Recipe recipe = recipeDAL.GetRecipeById(id);

            return View(recipe);
        }

        [HttpGet]
        public IActionResult Create(int id = 0)
        {
            string recipeStatus = HttpContext.Session.GetString("NewRecipeStatus");
            ViewBag.RecipeStatus = recipeStatus;

            ViewBag.ExistingIngredients = ingredientDAL.GetIngredients();
            ViewBag.Units = ingredientDAL.GetUnits();
            ViewBag.Numbers = ingredientDAL.GetNumbers();
            ViewBag.Fractions = ingredientDAL.GetFractions();

            Recipe recipe = recipeDAL.GetRecipeById(id);
            ViewModel viewModel = new ViewModel();
            viewModel.ModelRecipe = recipe;
            viewModel.ModelList = new List<string>();
            ViewBag.IngredientCount = recipe.Ingredients.Count;

            return View("Create", viewModel);
        }

        [HttpPost]
        public IActionResult Create(ViewModel viewModel)
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

            bool recipeAdded = recipeDAL.AddRecipe(recipe);

            if (recipeAdded)
            {
                HttpContext.Session.SetString("NewRecipeStatus", "Complete");

                return RedirectToAction("Detail");
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
            Recipe recipe = recipeDAL.GetRecipeById(id);

            return View(recipe);
        }
    }
}