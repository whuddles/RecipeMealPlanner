using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public IActionResult CreateMeal(int id = 0)

        {
            ViewBag.ExistingRecipes = recipeDAL.GetAllRecipes();

            MealViewModel mealViewModel = new MealViewModel();
            Meal meal = mealPlanDAL.GetMealById(id);
            mealViewModel.ModelMeal = meal;
            Console.WriteLine(mealViewModel.ModelMeal.Name);
            ViewBag.RecipeCount = meal.Recipes.Count;

            return View("CreateMeal", mealViewModel);
        }

        [HttpPost]
        public IActionResult CreateMeal(MealViewModel mealViewModel)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Session.SetString("NewMealStatus", "Partial");

                return RedirectToAction("CreateMeal");
            }

            List<Recipe> recipes = new List<Recipe>();
            List<string> recipeStrArr = mealViewModel.ModelList;

            Meal meal = new Meal
            {
                Name = recipeStrArr[0]
            };

            for(int i = 1; i < recipeStrArr.Count; i++)
            {
                Recipe recipe = new Recipe
                {
                    RecipeId = Convert.ToInt32(mealViewModel.ModelList[i])
                };
                recipes.Add(recipe);
            }

            meal.Recipes = recipes;

            int mealId = mealPlanDAL.CreateMeal(meal);

            return RedirectToAction("CreateMeal");
        }

        [HttpGet]
        public IActionResult CreatePlan(int id = 0)
        {
            MealPlanViewModel mealPlanViewModel = new MealPlanViewModel();
            MealPlan mealPlan = new MealPlan();
            List<Day> days = new List<Day>();
            if(id == 0)
            {
                mealPlan.Days = days;
            }
            else
            {
                mealPlan = mealPlanDAL.GetMealPlanById(id);
            }


            mealPlanViewModel.ModelMealPlan = mealPlan;

            ViewBag.ExistingMeals = mealPlanDAL.GetAllMeals();
            ViewBag.ExistingRecipes = recipeDAL.GetAllRecipes();

            return View("CreatePlan", mealPlanViewModel);
        }

        [HttpPost]
        public IActionResult CreatePlan(MealPlanViewModel mealPlanViewModel)
        {
            List<Day> days = new List<Day>();
            List<string> mealPlanList = mealPlanViewModel.ModelList;

            MealPlan mealPlan = new MealPlan
            {
                Name = mealPlanList[0]
            };

            for(int i = 1; i < mealPlanList.Count; i += 3)
            {
                Meal meal1 = new Meal();
                Meal meal2 = new Meal();
                Meal meal3 = new Meal();
                Day day = new Day
                {
                    Breakfast = new Meal(),
                    Lunch = new Meal(),
                    Dinner = new Meal()
                };

                //meal1.MealId = Convert.ToInt32(mealPlanList[i]);
                day.Breakfast.MealId = Convert.ToInt32(mealPlanList[i]); //meal1.MealId;

                //meal2.MealId = Convert.ToInt32(mealPlanList[i + 1]);
                day.Lunch.MealId = Convert.ToInt32(mealPlanList[i + 1]);  //meal2.MealId;

               //meal3.MealId = Convert.ToInt32(mealPlanList[i + 2]);
                day.Dinner.MealId = Convert.ToInt32(mealPlanList[i + 2]);  //meal3.MealId;

                days.Add(day);
            }

            mealPlan.Days = days;

            mealPlan.MealPlanId = mealPlanDAL.CreateMealPlan(mealPlan);
            int id = mealPlan.MealPlanId;
            //int mealPlanId = mealPlanDAL.CreateMealPlan(mealPlanViewModel.ModelMealPlan);
            return RedirectToAction("MealPlanDetail", "Meal", new { id });
        }

        public IActionResult MealPlanDetail(int id = 2)
        {
            MealPlan mealPlan = mealPlanDAL.GetMealPlanById(id);
            return View(mealPlan);
        }

        public IActionResult ModifyMealPlan(int id = 0)
        {
            //MealPlanViewModel mealPlanViewModel = new MealPlanViewModel
            //{
            //    ModelMealPlan = mealPlanDAL.GetMealPlanById(mealPlanId)
            //};

            return RedirectToAction("CreatePlan", "Meal", new { id });
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