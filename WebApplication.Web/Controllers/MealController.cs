using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

namespace WebApplication.Web.Controllers
{
    public class MealController : Controller
    {
        private IRecipeDAL recipeDAL;
        private IMealPlanDAL mealPlanDAL;

        public MealController(IRecipeDAL recipeDAL, IMealPlanDAL mealPlanDAL)
        {
            this.recipeDAL = recipeDAL;
            this.mealPlanDAL = mealPlanDAL;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateMeal(int mealId = 0)
        {
            ViewBag.ExistingRecipes = recipeDAL.GetAllRecipes();

            MealViewModel mealViewModel = new MealViewModel();
            Meal meal = mealPlanDAL.GetMealById(mealId);
            mealViewModel.ModelMeal = meal;
            ViewBag.RecipeCount = meal.Recipes.Count;

            return View("CreateMeal", mealViewModel);
        }

        [HttpPost]
        public IActionResult CreateMeal(MealViewModel mealViewModel)
        {
            return RedirectToAction("CreateMeal");
        }

        [HttpGet]
        public IActionResult CreatePlan(int mealPlanId = 0)
        {
            MealPlanViewModel mealPlanViewModel = new MealPlanViewModel();
            MealPlan mealPlan = mealPlanDAL.GetMealPlanById(mealPlanId);
            mealPlanViewModel.ModelMealPlan = mealPlan;

            ViewBag.ExistingMeals = mealPlanDAL.GetAllMeals();
            ViewBag.ExistingRecipes = recipeDAL.GetAllRecipes();

            return View("CreatePlan", mealPlanViewModel);
        }

        [HttpPost]
        public IActionResult CreatePlan(MealPlanViewModel mealPlanViewModel)
        {
            int mealPlanId = mealPlanDAL.CreateMealPlan(mealPlanViewModel.ModelMealPlan);
            return RedirectToAction("CreatePlan", new { mealPlanId });
        }

        public IActionResult MealPlanDetail(int mealPlanId = 0)
        {
            MealPlan mealPlan = mealPlanDAL.GetMealPlanById(mealPlanId);
            return View();
        }

        public IActionResult ModifyMealPlan()
        {
            return View();
        }

        public IActionResult ViewMyMealPlans()
        {
            return View();
        }

        public IActionResult GenerateGroceryList()
        {
            return View();
        }
    }
}