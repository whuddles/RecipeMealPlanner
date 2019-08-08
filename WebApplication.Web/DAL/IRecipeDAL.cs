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
        Recipe GetRecipeById(string recipeId);

        List<Recipe> GetAllRecipes();
        List<Recipe> GetRecipesByUserId(int userId);
        bool AddRecipeToUserAccount(int recipeId, int userId);
    }
}
