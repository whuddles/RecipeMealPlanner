using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IMealPlanDAL
    {
        int CreateMealPlan(MealPlan mealPlan);
        int CreateMeal(Meal meal);
        void AddRecipesToMeal(Meal meal);
        void AddMealsToMealPlan(MealPlan mealPlan);
        MealPlan GetMealPlanById(int mealPlanId);
        List<Meal> GetAllMeals();
        Day GetDayInMealPlan(int dayId);
        Meal GetMealById(int mealId);
        List<Recipe> GetRecipesInMeal(int mealId);
    }
}
