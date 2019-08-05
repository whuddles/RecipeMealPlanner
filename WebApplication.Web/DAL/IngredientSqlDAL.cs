using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class IngredientSqlDAL : IIngredientDAL
    {
        private string connectionString = "";
        private string sqlQueryGetIngredients = ""; //todo create SQL query to get name, quantity, unit type from db
        private string sqlInsertIngredient = "INSERT INTO ingredient VALUES(@name)"; //todo create sql insert to add ingredient to db

        public IngredientSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Ingredient> GetIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQueryGetIngredients, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ingredients.Add(MapRowToIngredient(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ingredients;
        }

        private Ingredient MapRowToIngredient(SqlDataReader reader)
        {
            Ingredient ingredient = new Ingredient();
            ingredient.Name = Convert.ToString(reader["name"]);
            ingredient.Quantity = Convert.ToString(reader["quantity"]);
            ingredient.UnitType = Convert.ToString(reader["type"]);     
            return ingredient;
        }

        public bool AddIngredient(string ingredient)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlInsertIngredient, connection);
                    command.Parameters.AddWithValue("@name", ingredient);                    

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
