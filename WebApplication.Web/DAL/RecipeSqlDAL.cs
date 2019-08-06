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
        private string sqlInsertRecipe = @"INSERT INTO recipe (name, description, instructions, prep_time, cook_time) 
                                           VALUES (@name, @description, @instructions, @prepTime, @cookTime);
                                           SELECT SCOPE_IDENTITY()";
        private string sqlAddIdsToIngredients = @"SELECT ingredient_id
                                                  FROM ingredient
                                                  WHERE name = @name";
        private string sqlGetUnitId = @"SELECT unit_id
                                        FROM unit
                                        WHERE unit = @unit";
        private string sqlGetFractionId = @"SELECT fraction_id
                                            FROM fraction   
                                            WHERE fraction = @fraction";
        private string sqlUpdateCompositeTable = @"INSERT INTO recipe_ingredient_unit_number_fraction (recipe_id, ingredient_id, unit_id, number_id, fraction_id)
                                                   VALUES (@recipeId, @ingredientId, @unitId, @numberId, @fractionId)";

        private IngredientSqlDAL ingredientDal;

        public RecipeSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool AddRecipe(Recipe recipe)
        {
            bool result = false;
            List<Ingredient> newIngredients = ingredientDal.FilterNewIngredients(recipe.Ingredients);

            foreach (Ingredient item in newIngredients)
            {
                ingredientDal.AddIngredient(item.Name);
            }

            recipe.Ingredients = AddIdsToIngredients(recipe.Ingredients);

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

                    recipe.RecipeId = Convert.ToInt32(command.ExecuteScalar());

                    if (recipe.RecipeId != 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }

            if (result)
            {
                UpdateCompositeTable(recipe);
            }

            return result;
        }

        private List<Ingredient> AddIdsToIngredients(List<Ingredient> ingredients)
        {
            try
            {
                for (int i = 0; i < ingredients.Count; i++)
                {
                    if (String.IsNullOrEmpty(ingredients[i].IngredientId.ToString()))
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            SqlCommand cmd = new SqlCommand(sqlAddIdsToIngredients, conn);
                            cmd.Parameters.AddWithValue("@name", ingredients[i].Name);

                            conn.Open();

                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                ingredients[i].IngredientId = Convert.ToInt32(reader["ingredient_id"]);
                            }
                        }
                    }

                    ingredients[i].UnitId = GetUnitId(ingredients[i]);
                    ingredients[i].FractionId = GetFractionId(ingredients[i]);
                }
            }
            catch
            {

            }

            return ingredients;
        }

        private int GetUnitId(Ingredient ingredient)
        {
            int unitId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetUnitId, conn);
                cmd.Parameters.AddWithValue("@unit", ingredient.Unit);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    unitId = Convert.ToInt32(reader["unit_id"]);
                }
            }

            return unitId;
        }

        private int GetFractionId(Ingredient ingredient)
        {
            int unitId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetFractionId, conn);
                cmd.Parameters.AddWithValue("@fraction", ingredient.Fraction);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    unitId = Convert.ToInt32(reader["fraction_id"]);
                }
            }

            return unitId;
        }

        private void UpdateCompositeTable(Recipe recipe)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlUpdateCompositeTable, conn);
                cmd.Parameters.AddWithValue("@recipeId", recipe.RecipeId);

                conn.Open();

                foreach (Ingredient ingredient in recipe.Ingredients)
                {
                    cmd.Parameters.AddWithValue("@ingredientId", ingredient.IngredientId);
                    cmd.Parameters.AddWithValue("@unitId", ingredient.UnitId);
                    cmd.Parameters.AddWithValue("@fractionId", ingredient.FractionId);
                    cmd.Parameters.AddWithValue("@numberId", Convert.ToInt32(ingredient.Number));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

