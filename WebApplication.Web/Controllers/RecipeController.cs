using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

namespace WebApplication.Web.Controllers
{
    public class RecipeController : Controller
    {
        //private IRecipeDAL dao;

        //public RecipeController(IRecipeDAL dao)
        //{
        //    this.dao = dao;
        //}

        public IActionResult Detail(string id = "1")
        {
            Recipe recipe = recipeDAL.GetRecipeById(id);

            return View(recipe);
        }

        public IActionResult Create()
        {
            Recipe newRecipe = new Recipe();
            ViewBag.ingredients = ingredientDAL.GetIngredients();
            ViewBag.units = ingredientDAL.GetUnits();
            ViewBag.numbers = ingredientDAL.GetNumbers();
            ViewBag.fractions = ingredientDAL.GetFractions();
            
            return View();
        }
    }
}