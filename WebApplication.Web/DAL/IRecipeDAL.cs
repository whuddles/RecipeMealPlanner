using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IRecipeDAL
    {
        int AddRecipe(Recipe recipe);
        int GetUnitId(Ingredient ingredient);
        int GetFractionId(Ingredient ingredient);
        void UpdateCompositeTable(Recipe recipe);
        Recipe GetRecipeById(int recipeId);

        List<Recipe> GetAllRecipes();
        List<Recipe> GetRecipesByUserId(int userId);
        void AddRecipeToUserAccount(int recipeId, int userId);
        List<Recipe> GetRecipesByIngredient(string searchString);

    }
}
