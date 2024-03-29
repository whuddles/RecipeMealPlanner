﻿using System;
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
        private string sqlAddRecipe = @"INSERT INTO recipe (name, description, instructions, prep_time, cook_time) 
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
                                                                    VALUES ((SELECT recipe_id
                                                                            FROM recipe
                                                                            WHERE recipe_id = @recipeId), 
                                                                            (SELECT ingredient_id
                                                                            FROM ingredient
                                                                            WHERE ingredient_id = @ingredientId), 
                                                                            (SELECT unit_id
                                                                            FROM unit
                                                                            WHERE unit_id = @unitId), 
                                                                            (SELECT number_id
                                                                            FROM number
                                                                            WHERE number_id = @numberId), 
                                                                            (SELECT fraction_id
                                                                            FROM fraction
                                                                            WHERE fraction_id = @fractionId))";
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
        private string sqlQueryGetAllRecipes = @"SELECT * FROM recipe";
        private string sqlInsertUserIdAndRecipeIdToUsersRecipe = @"INSERT INTO users_recipe (id, recipe_id) 
                                                                    VALUES (@userId, @recipeId)";
        private string sqlQueryGetRecipesByUserId = @"SELECT recipe.name as name, recipe.description, recipe.instructions, recipe.prep_time, recipe.cook_time, recipe.recipe_id
                                                                    FROM recipe
                                                                    JOIN users_recipe ON recipe.recipe_id = users_recipe.recipe_id
                                                                    WHERE users_recipe.id = @userId";
        private string sqlCheckUserAccountForRecipe = @"SELECT COUNT(recipe_id) as recipeCount 
                                                                    FROM users_recipe
                                                                    WHERE id = @userId
                                                                    AND recipe_id = @recipeId";
        private string sqlGetRecipesByIngredient = @"SELECT *
                                                     FROM recipe r
                                                     JOIN recipe_ingredient_unit_number_fraction riunf
                                                     ON r.recipe_id = riunf.recipe_id
                                                     JOIN ingredient i
                                                     ON riunf.ingredient_id = i.ingredient_id
                                                     WHERE i.name LIKE @searchString";
        private string sqlGetRecipesByIngredientAndUserId = @"SELECT *
                                                              FROM recipe r
                                                              JOIN recipe_ingredient_unit_number_fraction riunf
                                                              ON r.recipe_id = riunf.recipe_id
                                                              JOIN ingredient i
                                                              ON riunf.ingredient_id = i.ingredient_id
                                                              JOIN users_recipe 
                                                              ON r.recipe_id = users_recipe.recipe_id
                                                              WHERE users_recipe.id = @userId
                                                              AND i.name LIKE @searchString";
        private string sqlGetCategoriesByRecipeId = @"SELECT category_name
                                                        FROM category c
                                                        JOIN recipe_category r
                                                        ON c.category_id = r.category_id
                                                        WHERE r.recipe_id = @recipeId";
        private string sqlUpdateRecipeCategory = @"INSERT INTO recipe_category(recipe_id, category_id)
                                                   VALUES ((SELECT recipe_id
                                                            FROM recipe
                                                            WHERE recipe_id = @recipeId),
                                                           (SELECT category_id
                                                            FROM category
                                                            WHERE category_name = @categoryName))";
        private string sqlGetAllCategories = @"SELECT category_name FROM category";
        private string sqlGetRecipesByCategory = @" SELECT *
                                                    FROM recipe r
                                                    JOIN recipe_category rc
                                                    ON r.recipe_id = rc.recipe_id   
                                                    JOIN category c
                                                    ON c.category_id = rc.category_id
                                                    WHERE c.category_name LIKE @searchString";
        private string sqlGetRecipesByCategoryAndUserId = @"SELECT *
                                                            FROM recipe r
                                                            JOIN recipe_category rc
                                                            ON r.recipe_id = rc.recipe_id
                                                            JOIN category c
                                                            ON rc.category_id = c.category_id
                                                            JOIN users_recipe 
                                                            ON r.recipe_id = users_recipe.recipe_id
                                                            WHERE users_recipe.id = @userId
                                                            AND c.category_name LIKE @searchString";

        public RecipeSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int AddRecipe(Recipe recipe)
        {
            bool result = false;

            recipe.Ingredients = AddIdsToIngredients(recipe.Ingredients);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlAddRecipe, connection);
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
                try
                {
                    UpdateCompositeTable(recipe);
                    UpdateRecipeCategories(recipe.Categories, recipe.RecipeId);
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }

            return recipe.RecipeId;
        }

        public List<Ingredient> AddIdsToIngredients(List<Ingredient> ingredients)
        {
            try
            {
                for (int i = 0; i < ingredients.Count; i++)
                {
                    if (String.IsNullOrEmpty(ingredients[i].IngredientId.ToString()) || ingredients[i].IngredientId == 0)
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
            foreach (Ingredient ingredient in recipe.Ingredients)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {

                        SqlCommand cmd = new SqlCommand(sqlUpdateCompositeTable, conn);
                        cmd.Parameters.AddWithValue("@recipeId", recipe.RecipeId);
                        cmd.Parameters.AddWithValue("@ingredientId", ingredient.IngredientId);
                        cmd.Parameters.AddWithValue("@unitId", ingredient.UnitId);
                        cmd.Parameters.AddWithValue("@fractionId", ingredient.FractionId);
                        cmd.Parameters.AddWithValue("@numberId", Convert.ToInt32(ingredient.Number));

                        conn.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void UpdateRecipeCategories(List<string> categories, int recipeId)
        {
            foreach (string str in categories)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(sqlUpdateRecipeCategory, conn);
                        cmd.Parameters.AddWithValue("@recipeId", recipeId);
                        cmd.Parameters.AddWithValue("@categoryName", str);

                        conn.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public Recipe GetRecipeById(int recipeId)
        {
            Recipe recipe = new Recipe();
            List<Ingredient> ingredients = new List<Ingredient>();
            List<string> categories = new List<string>();

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
                    reader.Close();

                    SqlCommand cmd2 = new SqlCommand(sqlQueryGetIngredientsByRecipeId, conn);
                    cmd2.Parameters.AddWithValue("@recipeId", recipeId);

                    reader = cmd2.ExecuteReader();
                    while (reader.Read())
                    {
                        ingredients.Add(MapRowToIngredient(reader));
                    }
                    reader.Close();

                    SqlCommand cmd3 = new SqlCommand(sqlGetCategoriesByRecipeId, conn);
                    cmd3.Parameters.AddWithValue("@recipeId", recipeId);

                    reader = cmd3.ExecuteReader();
                    while (reader.Read())
                    {
                        categories.Add(Convert.ToString(reader["category_name"]));
                    }

                    while (categories.Count < 3)
                    {
                        categories.Add("-");
                    }

                    recipe.Categories = categories;
                    recipe.Ingredients = ingredients;
                    recipe.TotalTime = recipe.PrepTime + recipe.CookTime;
                    recipe.RecipeId = recipeId;
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

        public List<Recipe> GetAllRecipes()
        {
            List<Recipe> allRecipes = new List<Recipe>();
            Recipe recipe = new Recipe();
            List<Ingredient> ingredients = new List<Ingredient>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQueryGetAllRecipes, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        allRecipes.Add(MapRowToRecipe(reader));
                    }

                }
            }
            catch
            {
                throw;
            }

            if (allRecipes.Count > 0)
            {
                allRecipes.Sort((x, y) => x.Name.CompareTo(y.Name));
            }

            return allRecipes;
        }

        public List<string> GetAllCategories()
        {
            List<string> categories = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetAllCategories, conn);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(Convert.ToString(reader["category_name"]));
                }
            }

            return categories;
        }

        public Recipe MapRowToRecipe(SqlDataReader reader)
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            Recipe recipe = new Recipe();

            recipe.Name = Convert.ToString(reader["name"]);
            recipe.RecipeId = Convert.ToInt32(reader["recipe_id"]);
            recipe.Description = Convert.ToString(reader["description"]);
            recipe.Instructions = Convert.ToString(reader["instructions"]);
            recipe.PrepTime = Convert.ToInt32(reader["prep_time"]);
            recipe.CookTime = Convert.ToInt32(reader["cook_time"]);
            recipe.TotalTime = recipe.PrepTime + recipe.CookTime;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand(sqlQueryGetIngredientsByRecipeId, conn);
                    cmd.Parameters.AddWithValue("@recipeId", recipe.RecipeId);
                    conn.Open();
                    reader = cmd.ExecuteReader();

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

        public void AddRecipeToUserAccount(int recipeId, int userId)
        {
            if (!CheckUserAccountForRecipe(recipeId, userId))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlInsertUserIdAndRecipeIdToUsersRecipe, conn);
                    cmd.Parameters.AddWithValue("@recipeId", recipeId);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool CheckUserAccountForRecipe(int recipeId, int userId)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlCheckUserAccountForRecipe, conn);
                cmd.Parameters.AddWithValue("@recipeId", recipeId);
                cmd.Parameters.AddWithValue("@userId", userId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToBoolean(reader["recipeCount"]);
                }
            }
            return result;
        }

        public List<Recipe> GetRecipesByUserId(int userId)
        {
            List<Recipe> userRecipes = new List<Recipe>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQueryGetRecipesByUserId, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        userRecipes.Add(MapRowToRecipe(reader));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return userRecipes;
        }

        public List<Recipe> GetRecipesByIngredient(string searchString)
        {
            List<Recipe> recipes = new List<Recipe>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetRecipesByIngredient, conn);
                cmd.Parameters.AddWithValue("@searchString", "%" + searchString + "%");
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    recipes.Add(MapRowToRecipe(reader));
                }
            }
            return recipes;
        }

        public List<Recipe> GetRecipesByIngredientAndUserId(string searchString, int userId)
        {
            List<Recipe> recipes = new List<Recipe>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetRecipesByIngredientAndUserId, conn);
                cmd.Parameters.AddWithValue("@searchString", "%" + searchString + "%");
                cmd.Parameters.AddWithValue("@userId", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    recipes.Add(MapRowToRecipe(reader));
                }
            }
            return recipes;
        }

        public List<Recipe> GetRecipesByCategory(string searchString)
        {
            List<Recipe> recipes = new List<Recipe>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetRecipesByCategory, conn);
                cmd.Parameters.AddWithValue("@searchString", "%" + searchString + "%");
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    recipes.Add(MapRowToRecipe(reader));
                }
            }
            return recipes;
        }

        public List<Recipe> GetRecipesByCategoryAndUserId (string searchString, int userId)
        {
            List<Recipe> recipes = new List<Recipe>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetRecipesByCategoryAndUserId, conn);
                cmd.Parameters.AddWithValue("@searchString", "%" + searchString + "%");
                cmd.Parameters.AddWithValue("@userId", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    recipes.Add(MapRowToRecipe(reader));
                }
            }
            return recipes;
        }
    }
}

