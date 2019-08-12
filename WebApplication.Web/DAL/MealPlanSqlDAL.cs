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

        private string sqlAddMealName = @"INSERT INTO meal(meal_name) 
                                                    VALUES (@mealName);
                                                    SELECT SCOPE_IDENTITY()";
        private string sqlAddMealPlanName = @"INSERT INTO mealPlan(mealPlan_name) 
                                                    VALUES (@mealPlanName);
                                                    SELECT SCOPE_IDENTITY()";
        private string sqlAddRecipeToMeal = @"INSERT INTO meal_recipe (meal_id, recipe_id)
                                                    VALUES (@mealId, @recipeId)";
        private string sqlAddMealToMealPlan = @"INSERT INTO mealPlan_meal (mealPlan_id, meal_id)
                                                    VALUES (@mealPlan_id, @mealId)";
        private string sqlAddMealToUser = @"INSERT INTO users_meal (user_id, meal_id)
                                                    VALUES (@userId, @mealId)";
        private string sqlAddMealPlanToUser = @"INSERT INTO users_mealPlan (user_id, mealPlan_id)
                                                    VALUES (@userId, @mealPlanId)";
        private string sqlGetAllMeals = @"SELECT * 
                                                    FROM meal";
        private string sqlGetMealById = @"SELECT meal_id, meal_name
                                                    FROM meal
                                                    WHERE meal_id = @mealId";
        //private string sqlGetMealsInPlan = @"SELECT meal.meal_id, meal.meal_name
        //                                     FROM meal
        //                                     JOIN mealPlan_meal ON meal.meal_id = mealPlan_meal.meal_id
        //                                     WHERE mealPlan_id = @mealPlanId";
        private string sqlGetMealsInDay = @"SELECT breakfast, lunch, dinner
                                            FROM day
                                            WHERE day_id = @dayId";
                                             
        private string sqlGetRecipesByMealId = @"SELECT r.recipe_id, r.name, r.description
                                                 FROM recipe r
                                                 JOIN meal_recipe mr
                                                 ON r.recipe_id = mr.recipe_id
                                                 WHERE meal_id = @mealId";

        private string sqlGetMealPlanById = @"SELECT mealPlan_id, mealPlan_name
                                                    FROM mealPlan
                                                    WHERE mealPlan_id = @mealPlanId";
        private string sqlGetMealsByMealPlanId = @"SELECT meal_id, meal_name
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

                mealPlan.MealPlanId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            try
            {
                if (mealPlan.Days.Count > 0)
                {
                    AddMealsToMealPlan(mealPlan);
                }
            }
            catch (Exception ex)
            {

            }

            return mealPlan.MealPlanId;
        }

        public int CreateMeal(Meal meal)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlAddMealName, conn);
                cmd.Parameters.AddWithValue("@mealName", meal.Name);

                conn.Open();

                meal.MealId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (meal.Recipes.Count > 0)
            {
                AddRecipesToMeal(meal);
            }

            return meal.MealId;
        }

        public void AddMealsToMealPlan(MealPlan mealPlan)
        {
            for (int i = 0; i < mealPlan.Days.Count; i++)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlAddMealToMealPlan, conn);
                    cmd.Parameters.AddWithValue("@mealPlanId", mealPlan.MealPlanId);
                    cmd.Parameters.AddWithValue("@mealId", mealPlan.Days[i].Breakfast);

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

        public MealPlan GetMealPlanById(int mealPlanId)
        {
            MealPlan mealPlan = new MealPlan();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetMealPlanById, conn);
                cmd.Parameters.AddWithValue("@mealPlanId", mealPlanId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    mealPlan.MealPlanId = Convert.ToInt32(reader["mealPlan_id"]);
                    mealPlan.Name = Convert.ToString(reader["mealPlan_name"]);

                    Day day1 = GetDayInMealPlan(Convert.ToInt32(reader["day1"]));   
                    mealPlan.Days.Add(day1);
                    Day day2 = GetDayInMealPlan(Convert.ToInt32(reader["day2"]));
                    mealPlan.Days.Add(day2);
                    Day day3 = GetDayInMealPlan(Convert.ToInt32(reader["day3"]));
                    mealPlan.Days.Add(day3);
                    Day day4 = GetDayInMealPlan(Convert.ToInt32(reader["day4"]));
                    mealPlan.Days.Add(day4);
                    Day day5 = GetDayInMealPlan(Convert.ToInt32(reader["day5"]));
                    mealPlan.Days.Add(day5);
                    Day day6 = GetDayInMealPlan(Convert.ToInt32(reader["day6"]));
                    mealPlan.Days.Add(day6);
                    Day day7 = GetDayInMealPlan(Convert.ToInt32(reader["day7"]));
                    mealPlan.Days.Add(day7);

                }
            }

            return mealPlan;
        }

        public List<Meal> GetAllMeals()
        {
            List<Meal> meals = new List<Meal>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetAllMeals, conn);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Meal meal = new Meal();
                    meal.MealId = Convert.ToInt32(reader["meal_id"]);
                    meal.Name = Convert.ToString(reader["meal_name"]);

                    meals.Add(meal);
                }
            }

            if (meals.Count > 0)
            {
                meals.Sort((x, y) => x.Name.CompareTo(y.Name));
            }

            return meals;
        }

        public Day GetDayInMealPlan(int dayId)
        {
            Day day = new Day();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand(sqlGetMealsInDay, conn);
                cmd.Parameters.AddWithValue("@dayId", dayId);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    day.DayId = Convert.ToInt32(reader["day_id"]);
                    day.Breakfast.MealId = Convert.ToInt32(reader["breakfast"]);
                    day.Lunch.MealId = Convert.ToInt32(reader["lunch"]);
                    day.Dinner.MealId = Convert.ToInt32(reader["dinner"]);
                }
            }

            return day;
        }

        public Meal GetMealById(int mealId)
        {
            Meal meal = new Meal();
            List<Recipe> recipes = new List<Recipe>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlGetMealById, conn);
                    cmd.Parameters.AddWithValue("@mealId", mealId);

                    SqlCommand cmd2 = new SqlCommand(sqlGetRecipesByMealId, conn);
                    cmd2.Parameters.AddWithValue("@mealId", mealId);

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        meal.MealId = Convert.ToInt32(reader["meal_id"]);
                        meal.Name = Convert.ToString(reader["meal_name"]);
                    }
                    reader.Close();

                    reader = cmd2.ExecuteReader();
                    while (reader.Read())
                    {
                        Recipe recipe = new Recipe();
                        recipe.RecipeId = Convert.ToInt32(reader["recipe_id"]);
                        recipe.Name = Convert.ToString(reader["name"]);
                        recipe.Description = Convert.ToString(reader["description"]);

                        recipes.Add(recipe);
                    }

                    meal.Recipes = recipes;
                }

            }
            catch
            {
                throw;
            }

            return meal;
        }

        public List<Recipe> GetRecipesInMeal(int mealId)
        {
            List<Recipe> recipes = new List<Recipe>();


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlGetRecipesByMealId, conn);
                cmd.Parameters.AddWithValue("@mealId", mealId);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Recipe recipe = new Recipe();
                    recipe.Name = Convert.ToString(reader["name"]);
                    recipe.RecipeId = Convert.ToInt32(reader["recipe_id"]);
                    recipe.Description = Convert.ToString(reader["description"]);

                    recipes.Add(recipe);
                }
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
