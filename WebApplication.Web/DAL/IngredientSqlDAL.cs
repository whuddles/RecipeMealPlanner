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
        private string connectionString;
        private string sqlQueryGetIngredients = "SELECT * FROM ingredient";
        private string sqlInsertIngredient = "INSERT INTO ingredient VALUES(@name)";        

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

            ingredient.IngredientId = Convert.ToInt32(reader["ingredient_id"]);
            ingredient.Name = Convert.ToString(reader["name"]);

            return ingredient;
        }

        public bool AddIngredient(string name)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlInsertIngredient, connection);
                    command.Parameters.AddWithValue("@name", name);

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

        public List<Ingredient> FilterNewIngredients(List<Ingredient> ingredients)
        {
            List<Ingredient> newIngredients = new List<Ingredient>();
            List<Ingredient> existingIngredients = GetIngredients();

            foreach (Ingredient item in ingredients)
            {
                if (!existingIngredients.Exists(x => x.Name == item.Name))
                {
                    newIngredients.Add(item);
                }
            }

            return newIngredients;

        }
    }
}
