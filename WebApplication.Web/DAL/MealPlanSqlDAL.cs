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
        private string sqlAdd7DayMealPlan =       @"INSERT INTO mealPlan(mealPlan_name, day1, day2, day3, day4, day5, day6, day7) 
                                                    VALUES (@mealPlanName, (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day1), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day2), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day3), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day4), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day5), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day6), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day7));
                                                    SELECT SCOPE_IDENTITY()";
        private string sqlAdd6DayMealPlan =       @"INSERT INTO mealPlan(mealPlan_name, day1, day2, day3, day4, day5, day6) 
                                                    VALUES (@mealPlanName, (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day1), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day2), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day3), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day4), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day5), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day6));
                                                    SELECT SCOPE_IDENTITY()";
        private string sqlAdd5DayMealPlan =       @"INSERT INTO mealPlan(mealPlan_name, day1, day2, day3, day4, day5) 
                                                    VALUES (@mealPlanName, (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day1), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day2), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day3), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day4), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day5));
                                                    SELECT SCOPE_IDENTITY()";
        private string sqlAdd4DayMealPlan =       @"INSERT INTO mealPlan(mealPlan_name, day1, day2, day3, day4) 
                                                    VALUES (@mealPlanName, (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day1), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day2), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day3), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day4));
                                                    SELECT SCOPE_IDENTITY()";
        private string sqlAdd3DayMealPlan =       @"INSERT INTO mealPlan(mealPlan_name, day1, day2, day36) 
                                                    VALUES (@mealPlanName, (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day1), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day2), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day3));
                                                    SELECT SCOPE_IDENTITY()";
        private string sqlAdd2DayMealPlan =       @"INSERT INTO mealPlan(mealPlan_name, day1, day2) 
                                                    VALUES (@mealPlanName, (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day1), 
                                                                           (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day2));
                                                    SELECT SCOPE_IDENTITY()";
        private string sqlAdd1DayMealPlan = @"INSERT INTO mealPlan(mealPlan_name, day1) 
                                                    VALUES (@mealPlanName, (SELECT day_id
                                                                            FROM day
                                                                            WHERE day_id = @day1));
                                                    SELECT SCOPE_IDENTITY()";
        private string sqlAddDay = @"INSERT INTO day(breakfast, lunch, dinner)
                                                    VALUES ((SELECT meal_id
                                                             FROM meal
                                                             WHERE meal_id = @breakfast),
                                                            (SELECT meal_id
                                                             FROM meal
                                                             WHERE meal_id = @lunch), 
                                                            (SELECT meal_id
                                                             FROM meal
                                                             WHERE meal_id = @dinner));
                                                    SELECT SCOPE_IDENTITY();";
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
        private string sqlGetMealsInDay = @"SELECT day_id, breakfast, lunch, dinner
                                                    FROM day
                                                    WHERE day_id = @dayId";

        private string sqlGetRecipesByMealId = @"SELECT r.recipe_id, r.name, r.description
                                                    FROM recipe r
                                                    JOIN meal_recipe mr
                                                    ON r.recipe_id = mr.recipe_id
                                                    WHERE meal_id = @mealId";

        private string sqlGetMealPlanById = @"SELECT mealPlan_id, mealPlan_name, day1 /*, day2, day3, day4, day5, day6, day7 */
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
                SqlCommand cmd0 = new SqlCommand(sqlAdd7DayMealPlan, conn);
                SqlCommand cmd1 = new SqlCommand(sqlAddDay, conn);
                SqlCommand cmd2 = new SqlCommand(sqlAddDay, conn);
                SqlCommand cmd3 = new SqlCommand(sqlAddDay, conn);
                SqlCommand cmd4 = new SqlCommand(sqlAddDay, conn);
                SqlCommand cmd5 = new SqlCommand(sqlAddDay, conn);
                SqlCommand cmd6 = new SqlCommand(sqlAddDay, conn);
                SqlCommand cmd7 = new SqlCommand(sqlAddDay, conn);

                conn.Open();

                if (mealPlan.Days.Count == 7)
                {
                    cmd1 = new SqlCommand(sqlAddDay, conn);
                    cmd1.Parameters.AddWithValue("@breakfast", mealPlan.Days[0].Breakfast.MealId);
                    cmd1.Parameters.AddWithValue("@lunch", mealPlan.Days[0].Lunch.MealId);
                    cmd1.Parameters.AddWithValue("@dinner", mealPlan.Days[0].Dinner.MealId);
                    mealPlan.Days[0].DayId = Convert.ToInt32(cmd1.ExecuteScalar());

                    cmd2 = new SqlCommand(sqlAddDay, conn);
                    cmd2.Parameters.AddWithValue("@breakfast", mealPlan.Days[1].Breakfast.MealId);
                    cmd2.Parameters.AddWithValue("@lunch", mealPlan.Days[1].Lunch.MealId);
                    cmd2.Parameters.AddWithValue("@dinner", mealPlan.Days[1].Dinner.MealId);
                    mealPlan.Days[1].DayId = Convert.ToInt32(cmd2.ExecuteScalar());

                    cmd3 = new SqlCommand(sqlAddDay, conn);
                    cmd3.Parameters.AddWithValue("@breakfast", mealPlan.Days[2].Breakfast.MealId);
                    cmd3.Parameters.AddWithValue("@lunch", mealPlan.Days[2].Lunch.MealId);
                    cmd3.Parameters.AddWithValue("@dinner", mealPlan.Days[2].Dinner.MealId);
                    mealPlan.Days[2].DayId = Convert.ToInt32(cmd3.ExecuteScalar());

                    cmd4 = new SqlCommand(sqlAddDay, conn);
                    cmd4.Parameters.AddWithValue("@breakfast", mealPlan.Days[3].Breakfast.MealId);
                    cmd4.Parameters.AddWithValue("@lunch", mealPlan.Days[3].Lunch.MealId);
                    cmd4.Parameters.AddWithValue("@dinner", mealPlan.Days[3].Dinner.MealId);
                    mealPlan.Days[3].DayId = Convert.ToInt32(cmd4.ExecuteScalar());

                    cmd5 = new SqlCommand(sqlAddDay, conn);
                    cmd5.Parameters.AddWithValue("@breakfast", mealPlan.Days[4].Breakfast.MealId);
                    cmd5.Parameters.AddWithValue("@lunch", mealPlan.Days[4].Lunch.MealId);
                    cmd5.Parameters.AddWithValue("@dinner", mealPlan.Days[4].Dinner.MealId);
                    mealPlan.Days[4].DayId = Convert.ToInt32(cmd5.ExecuteScalar());

                    cmd6 = new SqlCommand(sqlAddDay, conn);
                    cmd6.Parameters.AddWithValue("@breakfast", mealPlan.Days[5].Breakfast.MealId);
                    cmd6.Parameters.AddWithValue("@lunch", mealPlan.Days[5].Lunch.MealId);
                    cmd6.Parameters.AddWithValue("@dinner", mealPlan.Days[5].Dinner.MealId);
                    mealPlan.Days[5].DayId = Convert.ToInt32(cmd6.ExecuteScalar());

                    cmd7 = new SqlCommand(sqlAddDay, conn);
                    cmd7.Parameters.AddWithValue("@breakfast", mealPlan.Days[6].Breakfast.MealId);
                    cmd7.Parameters.AddWithValue("@lunch", mealPlan.Days[6].Lunch.MealId);
                    cmd7.Parameters.AddWithValue("@dinner", mealPlan.Days[6].Dinner.MealId);
                    mealPlan.Days[6].DayId = Convert.ToInt32(cmd7.ExecuteScalar());

                    cmd0 = new SqlCommand(sqlAdd7DayMealPlan, conn);
                    cmd0.Parameters.AddWithValue("@mealPlanName", mealPlan.Name);
                    cmd0.Parameters.AddWithValue("@day1", mealPlan.Days[0].DayId);
                    cmd0.Parameters.AddWithValue("@day2", mealPlan.Days[1].DayId);
                    cmd0.Parameters.AddWithValue("@day3", mealPlan.Days[2].DayId);
                    cmd0.Parameters.AddWithValue("@day4", mealPlan.Days[3].DayId);
                    cmd0.Parameters.AddWithValue("@day5", mealPlan.Days[4].DayId);
                    cmd0.Parameters.AddWithValue("@day6", mealPlan.Days[5].DayId);
                    cmd0.Parameters.AddWithValue("@day7", mealPlan.Days[6].DayId);
                }
                else if (mealPlan.Days.Count == 6)
                {
                    cmd1 = new SqlCommand(sqlAddDay, conn);
                    cmd1.Parameters.AddWithValue("@breakfast", mealPlan.Days[0].Breakfast.MealId);
                    cmd1.Parameters.AddWithValue("@lunch", mealPlan.Days[0].Lunch.MealId);
                    cmd1.Parameters.AddWithValue("@dinner", mealPlan.Days[0].Dinner.MealId);
                    mealPlan.Days[0].DayId = Convert.ToInt32(cmd1.ExecuteScalar());

                    cmd2 = new SqlCommand(sqlAddDay, conn);
                    cmd2.Parameters.AddWithValue("@breakfast", mealPlan.Days[1].Breakfast.MealId);
                    cmd2.Parameters.AddWithValue("@lunch", mealPlan.Days[1].Lunch.MealId);
                    cmd2.Parameters.AddWithValue("@dinner", mealPlan.Days[1].Dinner.MealId);
                    mealPlan.Days[1].DayId = Convert.ToInt32(cmd2.ExecuteScalar());

                    cmd3 = new SqlCommand(sqlAddDay, conn);
                    cmd3.Parameters.AddWithValue("@breakfast", mealPlan.Days[2].Breakfast.MealId);
                    cmd3.Parameters.AddWithValue("@lunch", mealPlan.Days[2].Lunch.MealId);
                    cmd3.Parameters.AddWithValue("@dinner", mealPlan.Days[2].Dinner.MealId);
                    mealPlan.Days[2].DayId = Convert.ToInt32(cmd3.ExecuteScalar());

                    cmd4 = new SqlCommand(sqlAddDay, conn);
                    cmd4.Parameters.AddWithValue("@breakfast", mealPlan.Days[3].Breakfast.MealId);
                    cmd4.Parameters.AddWithValue("@lunch", mealPlan.Days[3].Lunch.MealId);
                    cmd4.Parameters.AddWithValue("@dinner", mealPlan.Days[3].Dinner.MealId);
                    mealPlan.Days[3].DayId = Convert.ToInt32(cmd4.ExecuteScalar());

                    cmd5 = new SqlCommand(sqlAddDay, conn);
                    cmd5.Parameters.AddWithValue("@breakfast", mealPlan.Days[4].Breakfast.MealId);
                    cmd5.Parameters.AddWithValue("@lunch", mealPlan.Days[4].Lunch.MealId);
                    cmd5.Parameters.AddWithValue("@dinner", mealPlan.Days[4].Dinner.MealId);
                    mealPlan.Days[4].DayId = Convert.ToInt32(cmd5.ExecuteScalar());

                    cmd6 = new SqlCommand(sqlAddDay, conn);
                    cmd6.Parameters.AddWithValue("@breakfast", mealPlan.Days[5].Breakfast.MealId);
                    cmd6.Parameters.AddWithValue("@lunch", mealPlan.Days[5].Lunch.MealId);
                    cmd6.Parameters.AddWithValue("@dinner", mealPlan.Days[5].Dinner.MealId);
                    mealPlan.Days[5].DayId = Convert.ToInt32(cmd6.ExecuteScalar());

                    cmd0 = new SqlCommand(sqlAdd6DayMealPlan, conn);
                    cmd0.Parameters.AddWithValue("@mealPlanName", mealPlan.Name);
                    cmd0.Parameters.AddWithValue("@day1", mealPlan.Days[0].DayId);
                    cmd0.Parameters.AddWithValue("@day2", mealPlan.Days[1].DayId);
                    cmd0.Parameters.AddWithValue("@day3", mealPlan.Days[2].DayId);
                    cmd0.Parameters.AddWithValue("@day4", mealPlan.Days[3].DayId);
                    cmd0.Parameters.AddWithValue("@day5", mealPlan.Days[4].DayId);
                    cmd0.Parameters.AddWithValue("@day6", mealPlan.Days[5].DayId);
                }
                else if (mealPlan.Days.Count == 5)
                {
                    cmd1 = new SqlCommand(sqlAddDay, conn);
                    cmd1.Parameters.AddWithValue("@breakfast", mealPlan.Days[0].Breakfast.MealId);
                    cmd1.Parameters.AddWithValue("@lunch", mealPlan.Days[0].Lunch.MealId);
                    cmd1.Parameters.AddWithValue("@dinner", mealPlan.Days[0].Dinner.MealId);
                    mealPlan.Days[0].DayId = Convert.ToInt32(cmd1.ExecuteScalar());

                    cmd2 = new SqlCommand(sqlAddDay, conn);
                    cmd2.Parameters.AddWithValue("@breakfast", mealPlan.Days[1].Breakfast.MealId);
                    cmd2.Parameters.AddWithValue("@lunch", mealPlan.Days[1].Lunch.MealId);
                    cmd2.Parameters.AddWithValue("@dinner", mealPlan.Days[1].Dinner.MealId);
                    mealPlan.Days[1].DayId = Convert.ToInt32(cmd2.ExecuteScalar());

                    cmd3 = new SqlCommand(sqlAddDay, conn);
                    cmd3.Parameters.AddWithValue("@breakfast", mealPlan.Days[2].Breakfast.MealId);
                    cmd3.Parameters.AddWithValue("@lunch", mealPlan.Days[2].Lunch.MealId);
                    cmd3.Parameters.AddWithValue("@dinner", mealPlan.Days[2].Dinner.MealId);
                    mealPlan.Days[2].DayId = Convert.ToInt32(cmd3.ExecuteScalar());

                    cmd4 = new SqlCommand(sqlAddDay, conn);
                    cmd4.Parameters.AddWithValue("@breakfast", mealPlan.Days[3].Breakfast.MealId);
                    cmd4.Parameters.AddWithValue("@lunch", mealPlan.Days[3].Lunch.MealId);
                    cmd4.Parameters.AddWithValue("@dinner", mealPlan.Days[3].Dinner.MealId);
                    mealPlan.Days[3].DayId = Convert.ToInt32(cmd4.ExecuteScalar());

                    cmd5 = new SqlCommand(sqlAddDay, conn);
                    cmd5.Parameters.AddWithValue("@breakfast", mealPlan.Days[4].Breakfast.MealId);
                    cmd5.Parameters.AddWithValue("@lunch", mealPlan.Days[4].Lunch.MealId);
                    cmd5.Parameters.AddWithValue("@dinner", mealPlan.Days[4].Dinner.MealId);
                    mealPlan.Days[4].DayId = Convert.ToInt32(cmd5.ExecuteScalar());

                    cmd0 = new SqlCommand(sqlAdd5DayMealPlan, conn);
                    cmd0.Parameters.AddWithValue("@mealPlanName", mealPlan.Name);
                    cmd0.Parameters.AddWithValue("@day1", mealPlan.Days[0].DayId);
                    cmd0.Parameters.AddWithValue("@day2", mealPlan.Days[1].DayId);
                    cmd0.Parameters.AddWithValue("@day3", mealPlan.Days[2].DayId);
                    cmd0.Parameters.AddWithValue("@day4", mealPlan.Days[3].DayId);
                    cmd0.Parameters.AddWithValue("@day5", mealPlan.Days[4].DayId);
                }
                else if (mealPlan.Days.Count == 4)
                {
                    cmd1 = new SqlCommand(sqlAddDay, conn);
                    cmd1.Parameters.AddWithValue("@breakfast", mealPlan.Days[0].Breakfast.MealId);
                    cmd1.Parameters.AddWithValue("@lunch", mealPlan.Days[0].Lunch.MealId);
                    cmd1.Parameters.AddWithValue("@dinner", mealPlan.Days[0].Dinner.MealId);
                    mealPlan.Days[0].DayId = Convert.ToInt32(cmd1.ExecuteScalar());

                    cmd2 = new SqlCommand(sqlAddDay, conn);
                    cmd2.Parameters.AddWithValue("@breakfast", mealPlan.Days[1].Breakfast.MealId);
                    cmd2.Parameters.AddWithValue("@lunch", mealPlan.Days[1].Lunch.MealId);
                    cmd2.Parameters.AddWithValue("@dinner", mealPlan.Days[1].Dinner.MealId);
                    mealPlan.Days[1].DayId = Convert.ToInt32(cmd2.ExecuteScalar());

                    cmd3 = new SqlCommand(sqlAddDay, conn);
                    cmd3.Parameters.AddWithValue("@breakfast", mealPlan.Days[2].Breakfast.MealId);
                    cmd3.Parameters.AddWithValue("@lunch", mealPlan.Days[2].Lunch.MealId);
                    cmd3.Parameters.AddWithValue("@dinner", mealPlan.Days[21].Dinner.MealId);
                    mealPlan.Days[2].DayId = Convert.ToInt32(cmd3.ExecuteScalar());

                    cmd4 = new SqlCommand(sqlAddDay, conn);
                    cmd4.Parameters.AddWithValue("@breakfast", mealPlan.Days[3].Breakfast.MealId);
                    cmd4.Parameters.AddWithValue("@lunch", mealPlan.Days[3].Lunch.MealId);
                    cmd4.Parameters.AddWithValue("@dinner", mealPlan.Days[3].Dinner.MealId);
                    mealPlan.Days[3].DayId = Convert.ToInt32(cmd4.ExecuteScalar());

                    cmd0 = new SqlCommand(sqlAdd4DayMealPlan, conn);
                    cmd0.Parameters.AddWithValue("@mealPlanName", mealPlan.Name);
                    cmd0.Parameters.AddWithValue("@day1", mealPlan.Days[0].DayId);
                    cmd0.Parameters.AddWithValue("@day2", mealPlan.Days[1].DayId);
                    cmd0.Parameters.AddWithValue("@day3", mealPlan.Days[2].DayId);
                    cmd0.Parameters.AddWithValue("@day4", mealPlan.Days[3].DayId);
                }
                else if (mealPlan.Days.Count == 3)
                {
                    cmd1 = new SqlCommand(sqlAddDay, conn);
                    cmd1.Parameters.AddWithValue("@breakfast", mealPlan.Days[0].Breakfast.MealId);
                    cmd1.Parameters.AddWithValue("@lunch", mealPlan.Days[0].Lunch.MealId);
                    cmd1.Parameters.AddWithValue("@dinner", mealPlan.Days[0].Dinner.MealId);
                    mealPlan.Days[0].DayId = Convert.ToInt32(cmd1.ExecuteScalar());

                    cmd2 = new SqlCommand(sqlAddDay, conn);
                    cmd2.Parameters.AddWithValue("@breakfast", mealPlan.Days[1].Breakfast.MealId);
                    cmd2.Parameters.AddWithValue("@lunch", mealPlan.Days[1].Lunch.MealId);
                    cmd2.Parameters.AddWithValue("@dinner", mealPlan.Days[1].Dinner.MealId);
                    mealPlan.Days[1].DayId = Convert.ToInt32(cmd2.ExecuteScalar());

                    cmd3 = new SqlCommand(sqlAddDay, conn);
                    cmd3.Parameters.AddWithValue("@breakfast", mealPlan.Days[2].Breakfast.MealId);
                    cmd3.Parameters.AddWithValue("@lunch", mealPlan.Days[2].Lunch.MealId);
                    cmd3.Parameters.AddWithValue("@dinner", mealPlan.Days[2].Dinner.MealId);
                    mealPlan.Days[2].DayId = Convert.ToInt32(cmd3.ExecuteScalar());

                    cmd0 = new SqlCommand(sqlAdd3DayMealPlan, conn);
                    cmd0.Parameters.AddWithValue("@mealPlanName", mealPlan.Name);
                    cmd0.Parameters.AddWithValue("@day1", mealPlan.Days[0].DayId);
                    cmd0.Parameters.AddWithValue("@day2", mealPlan.Days[1].DayId);
                    cmd0.Parameters.AddWithValue("@day3", mealPlan.Days[2].DayId);
                }
                else if (mealPlan.Days.Count == 2)
                {
                    cmd1 = new SqlCommand(sqlAddDay, conn);
                    cmd1.Parameters.AddWithValue("@breakfast", mealPlan.Days[0].Breakfast.MealId);
                    cmd1.Parameters.AddWithValue("@lunch", mealPlan.Days[0].Lunch.MealId);
                    cmd1.Parameters.AddWithValue("@dinner", mealPlan.Days[0].Dinner.MealId);
                    mealPlan.Days[0].DayId = Convert.ToInt32(cmd1.ExecuteScalar());

                    cmd2 = new SqlCommand(sqlAddDay, conn);
                    cmd2.Parameters.AddWithValue("@breakfast", mealPlan.Days[1].Breakfast.MealId);
                    cmd2.Parameters.AddWithValue("@lunch", mealPlan.Days[1].Lunch.MealId);
                    cmd2.Parameters.AddWithValue("@dinner", mealPlan.Days[1].Dinner.MealId);
                    mealPlan.Days[1].DayId = Convert.ToInt32(cmd2.ExecuteScalar());

                    cmd0 = new SqlCommand(sqlAdd2DayMealPlan, conn);
                    cmd0.Parameters.AddWithValue("@mealPlanName", mealPlan.Name);
                    cmd0.Parameters.AddWithValue("@day1", mealPlan.Days[0].DayId);
                    cmd0.Parameters.AddWithValue("@day2", mealPlan.Days[1].DayId);
                }
                else if (mealPlan.Days.Count == 1)
                {
                    cmd1 = new SqlCommand(sqlAddDay, conn);
                    cmd1.Parameters.AddWithValue("@breakfast", mealPlan.Days[0].Breakfast.MealId);
                    cmd1.Parameters.AddWithValue("@lunch", mealPlan.Days[0].Lunch.MealId);
                    cmd1.Parameters.AddWithValue("@dinner", mealPlan.Days[0].Dinner.MealId);
                    mealPlan.Days[0].DayId = Convert.ToInt32(cmd1.ExecuteScalar());

                    cmd0 = new SqlCommand(sqlAdd1DayMealPlan, conn);
                    cmd0.Parameters.AddWithValue("@mealPlanName", mealPlan.Name);
                    cmd0.Parameters.AddWithValue("@day1", mealPlan.Days[0].DayId);
                }

                mealPlan.MealPlanId = Convert.ToInt32(cmd0.ExecuteScalar());
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
            List<Day> days = new List<Day>();
            mealPlan.Days = days;

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
                    //Day day2 = GetDayInMealPlan(Convert.ToInt32(reader["day2"]));
                    //mealPlan.Days.Add(day2);
                    //Day day3 = GetDayInMealPlan(Convert.ToInt32(reader["day3"]));
                    //mealPlan.Days.Add(day3);
                    //Day day4 = GetDayInMealPlan(Convert.ToInt32(reader["day4"]));
                    //mealPlan.Days.Add(day4);
                    //Day day5 = GetDayInMealPlan(Convert.ToInt32(reader["day5"]));
                    //mealPlan.Days.Add(day5);
                    //Day day6 = GetDayInMealPlan(Convert.ToInt32(reader["day6"]));
                    //mealPlan.Days.Add(day6);
                    //Day day7 = GetDayInMealPlan(Convert.ToInt32(reader["day7"]));
                    //mealPlan.Days.Add(day7);

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
            Day day = new Day
            {
                Breakfast = new Meal(),
                Lunch = new Meal(),
                Dinner = new Meal()
            };

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
