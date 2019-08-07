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
        private string connectionString;
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
        private string sqlQueryGetRecipeById = @"SELECT name as recipeName, description, instructions, prep_time, cook_time
                                                FROM recipe
                                                WHERE recipe.recipe_id = @recipeId";
        private string sqlQueryGetIngredientsByRecipeId = @"SELECT ingredient.name as ingredientName, unit.unit, number.number, fraction.fraction
                                                            FROM recipe
                                                            JOIN recipe_ingredient_unit_number_fraction on recipe.recipe_id = recipe_ingredient_unit_number_fraction.recipe_id
                                                            JOIN ingredient on ingredient.ingredient_id = recipe_ingredient_unit_number_fraction.ingredient_id
                                                            JOIN unit on unit.unit_id = recipe_ingredient_unit_number_fraction.unit_id
                                                            JOIN number on number.number_id = recipe_ingredient_unit_number_fraction.number_id
                                                            JOIN fraction on fraction.fraction_id = recipe_ingredient_unit_number_fraction.fraction_id
                                                            WHERE recipe.recipe_id = @recipeId";

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

        public List<Ingredient> AddIdsToIngredients(List<Ingredient> ingredients)
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
                throw;
            }

            return ingredients;
        }

        public int GetUnitId(Ingredient ingredient)
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

        public int GetFractionId(Ingredient ingredient)
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

        public void UpdateCompositeTable(Recipe recipe)
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

        public Recipe GetRecipeById(string recipeId)
        {
            Recipe recipe = new Recipe();

            List<Ingredient> ingredients = new List<Ingredient>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQueryGetRecipeById, conn);
                    cmd.Parameters.AddWithValue("@recipeId", recipeId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                     
                    while (reader.Read())
                    {
                        recipe.Name = Convert.ToString(reader["recipeName"]);
                        recipe.Description = Convert.ToString(reader["description"]);
                        recipe.Instructions = Convert.ToString(reader["instructions"]);
                        recipe.PrepTime = Convert.ToInt32(reader["prep_time"]);
                        recipe.CookTime = Convert.ToInt32(reader["cook_time"]);
                        recipe.Description = Convert.ToString(reader["description"]);
                    }

                    SqlCommand cmd2 = new SqlCommand(sqlQueryGetIngredientsByRecipeId, conn);
                    cmd.Parameters.AddWithValue("@recipeId", recipeId);
                    reader = cmd2.ExecuteReader();

                    while (reader.Read())
                    {
                        ingredients.Add(MapRowToIngredient(reader));
                    }

                    recipe.Ingredients = ingredients;
                }
            }
            catch
            {
                throw;
            }
            return recipe;
        }

        public Ingredient MapRowToIngredient(SqlDataReader reader)
        {
            Ingredient ingredient = new Ingredient();
            ingredient.Name = Convert.ToString(reader["ingredientName"]);
            ingredient.Unit = Convert.ToString(reader["unit"]);
            ingredient.Number = Convert.ToString(reader["number"]);
            ingredient.Fraction = Convert.ToString(reader["fraction"]);

            return ingredient;
        }
    }
}

