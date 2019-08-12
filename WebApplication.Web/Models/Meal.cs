using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Meal
    {
        public int UserId { get; set; }
        public int MealId { get; set; }
        public string Name { get; set; }
        public List<Recipe> Recipes { get; set; }  
    }
}
