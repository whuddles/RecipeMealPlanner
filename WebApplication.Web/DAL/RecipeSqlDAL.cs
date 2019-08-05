using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class RecipeSqlDAL : IRecipeDAL
    {
        private string connectionString = "";

        public RecipeSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Recipe> GetRecipes()
        {
            List<Recipe> Recipes = new List<Recipe>();

            return Recipes;

        }

    }
}
