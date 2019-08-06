using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IRecipeDAL
    {
        bool AddRecipe(string name, string description, string instructions, int prepTime, int cookTime, List<Ingredient> ingredients);


    }
}
