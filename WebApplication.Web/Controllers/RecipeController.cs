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

        public IActionResult Detail()
        {
           IList<Recipe> recipe = new List<Recipe>();
            return View(recipe);
        }
        public IActionResult Create()
        {
            Recipe newRecipe = new Recipe();
            return View();
        }
    }
}