using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class MealPlanSqlDAL : IMealPlanDAL
    {
        private string connectionString;

        private string sqlAddMealName =           @"INSERT INTO meal(meal_name) 
                                                    VALUES (@mealName);
                                                    SELECT SCOPE_IDENTITY();";
        private string sqlAddMealPlanName =       @"INSERT INTO mealPlan(mealPlan_name) 
                                                    VALUES (@mealPlanName);
                                                    SELECT SCOPE_IDENTITY();";
        private string sqlAddRecipeToMeal =       @"INSERT INTO meal_recipe (meal_id, recipe_id)
                                                    VALUES (@mealId, @recipeId)";
        private string sqlAddMealToMealPlan =     @"INSERT INTO mealPlan_meal (mealPlan_id, meal_id)
                                                    VALUES (@mealPlan_id, @mealId)";
        private string sqlAddMealToUser =         @"INSERT INTO users_meal (user_id, meal_id)
                                                    VALUES (@userId, @mealId)";
        private string sqlAddMealPlanToUser =     @"INSERT INTO users_mealPlan (user_id, mealPlan_id)
                                                    VALUES (@userId, @mealPlanId)";
        private string sqlGetAllMeals =           @"SELECT * 
                                                    FROM meal";
        private string sqlGetMealsInPlan =        @"SELECT *
                                                    FROM meal
                                                    WHERE meal_id = @mealId";
        private string sqlGetRecipesByMealId =    @"SELECT recipe_id, name, description
                                                    FROM users_meal_recipe u
                                                    JOIN recipe r
                                                    ON r.recipe_id = u.recipe_id
                                                    WHERE meal_id = @mealId";
        private string sqlGetMealPlan =           @"SELECT mealPlan_id, mealPlan_name, mealCount
                                                    FROM mealPlan
                                                    WHERE mealPlan_id = @mealPlanId";
        private string sqlGetMealsByMealPlanId =  @"SELECT meal_id, meal_name
                                                    FROM mealPlan_meal";

        public MealPlanSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int CreateMealPlan(MealPlan mealPlan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlAddMealPlanName, conn);
                cmd.Parameters.AddWithValue("@mealPlanName", mealPlan.Name);

                conn.Open();

                mealPlan.MealPlanId = cmd.ExecuteNonQuery();
            }
            
            try
            {
                if(mealPlan.Meals.Count > 0)
                {
                    AddMealsToMealPlan(mealPlan);
                }
            }
            catch(Exception ex)
            {

            }

            return mealPlan.MealPlanId;
        }

        public int CreateMeal(Meal meal)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlAddMealName, conn);
                cmd.Parameters.AddWithValue("@name", meal.Name);

                conn.Open();

                meal.MealId = cmd.ExecuteNonQuery();
            }

            if(meal.Recipes.Count > 0)
            {
                AddRecipesToMeal(meal);
            }

            return meal.MealId;
        }

        public void AddMealsToMealPlan(MealPlan mealPlan)
        {
            for (int i = 0; i < mealPlan.Meals.Count; i++)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlAddMealToMealPlan, conn);
                    cmd.Parameters.AddWithValue("@mealPlanId", mealPlan.MealPlanId);
                    cmd.Parameters.AddWithValue("@mealId", mealPlan.Meals[i].MealId);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddRecipesToMeal(Meal meal)
        {
            for (int i = 0; i < meal.Recipes.Count; i++)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlAddRecipeToMeal, conn);
                    cmd.Parameters.AddWithValue("@mealId", meal.MealId);
                    cmd.Parameters.AddWithValue("@recipeId", meal.Recipes[i].RecipeId);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public MealPlan GetMealPlan(int mealPlanId)
        {
            MealPlan mealPlan = new MealPlan();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetMealPlan, conn);
            }

            return mealPlan;
        }

        public List<Meal> GetAllMeals()
        {
            List<Meal> meals = new List<Meal>();
            Meal meal = new Meal();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetAllMeals, conn);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    meal.MealId = Convert.ToInt32(reader["meal_id"]);
                    meal.Name = Convert.ToString(reader["meal_name"]);

                    meals.Add(meal);
                }
            }
            
            if(meals.Count > 0)
            {
                meals.Sort((x, y) => y.Name.CompareTo(x.Name));
            }

            return meals;
        }

        public List<Meal> GetMealsInMealPlan(int mealPlanId)
        {
            List<Meal> meals = new List<Meal>();
            Meal meal = new Meal();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetRecipesByMealId, conn);
                cmd.Parameters.AddWithValue("@mealPlanId", mealPlanId);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    meal.MealId = Convert.ToInt32(reader["recipe_id"]);
                    meal.Name = reader["name"] as string;
                    meal.Recipes = GetRecipesInMeal(meal.MealId);

                    meals.Add(meal);
                }
            }

            return meals;
        }

        public List<Recipe> GetRecipesInMeal(int mealId)
        {
            List<Recipe> recipes = new List<Recipe>();
            Recipe recipe = new Recipe();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetRecipesByMealId, conn);
            }

            return recipes;
        }

        //public List<Meal> GetMealsInPlan(int mealPlanId)
        //{
        //    List<Meal> meals = new List<Meal>();
        //    Meal meal = new Meal();

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand(sqlGetMealsInPlan, conn);
        //        cmd.Parameters.AddWithValue("@mealPlanId", mealPlanId);

        //        conn.Open();

        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            meal.MealId = Convert.ToInt32(reader["meal_id"]);
        //            meal.Name = reader["meal_name"] as string;
        //            meal.Recipes = GetRecipesInMeal(meal.MealId);

        //            meals.Add(meal);
        //        }
        //    }

        //    return meals;
        //}
    }
}
