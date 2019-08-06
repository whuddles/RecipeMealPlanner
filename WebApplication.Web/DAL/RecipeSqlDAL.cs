using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;


namespace WebApplication.Web.DAL
{
    public class RecipeSqlDAL : IRecipeDAL
    {
        private string connectionString = "";
        private string sqlInsertRecipe = "INSERT INTO recipe (name, description, instructions, prep_time, cook_time) VALUES (@name, @description, @instructions, @prepTime, @cookTime)";
        private IIngredientDAL ingredientDal;

        public RecipeSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool AddRecipe(Recipe recipe)
        {
            {
                bool result = false;                
                ingredientDal.
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlInsertRecipe, connection);
                        command.Parameters.AddWithValue("@name", recipe.Name);
                        command.Parameters.AddWithValue("@description", recipe.Description);
                        command.Parameters.AddWithValue("@instructions", recipe.Instructions);
                        command.Parameters.AddWithValue("@prepTime", recipe.PrepTime);
                        command.Parameters.AddWithValue("@cookTime", recipe.CookTime);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;

            }
        }

    }
}

