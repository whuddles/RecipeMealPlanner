using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IIngredientDAL
    {
        List<Ingredient> GetIngredients();
        List<string> GetUnits();
        List<string> GetFractions();
        List<string> GetNumbers();
        bool AddIngredient(string name);
        List<Ingredient> GetIngredientsByMealPlan(MealPlan mealPlan);
    }
}
