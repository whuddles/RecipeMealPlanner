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
        private string sqlQueryGetUnits = "SELECT unit FROM unit";
        private string sqlQueryGetFractions = "SELECT fraction FROM fraction";
        private string sqlQueryGetNumbers = "SELECT number FROM number";

        public IngredientSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Ingredient> GetIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlQueryGetIngredients, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

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

            if(ingredients.Count > 0)
            {
                ingredients.Sort((x, y) => x.Name.CompareTo(y.Name));
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

        private string MapRowToUnit(SqlDataReader reader)
        {
            string unit;

            unit = Convert.ToString(reader["unit"]);

            return unit;
        }

        private string MapRowToFraction(SqlDataReader reader)
        {            
            string fraction = Convert.ToString(reader["fraction"]);

            return fraction;
        }

        private string MapRowToNumber(SqlDataReader reader)
        {
            string number = Convert.ToString(reader["number"]);

            return number;
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

        public List<string> GetUnits()
        {
            List<string> units = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQueryGetUnits, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        units.Add(MapRowToUnit(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return units;
        }

        public List<string> GetFractions()
        {
            List<string> fractions = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQueryGetFractions, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        fractions.Add(MapRowToFraction(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return fractions;
        }

        public List<string> GetNumbers()
        {
            List<string> numbers = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlQueryGetNumbers, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        numbers.Add(MapRowToNumber(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return numbers;
        }
    }
}
