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

        public RecipeSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool AddRecipe(string name, string description, string instructions, int prepTime, int cookTime, List<Ingredient> ingredients)
        {
            {
                bool result = false;                

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlInsertRecipe, connection);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@instructions", instructions);
                        command.Parameters.AddWithValue("@prepTime", prepTime);
                        command.Parameters.AddWithValue("@cookTime", cookTime);

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

