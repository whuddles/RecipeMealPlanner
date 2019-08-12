using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class MealPlan
    {
        public int UserId { get; set; }
        public int MealPlanId { get; set; }
        public string Name { get; set; }
        public List<Meal> Meals { get; set; }
    }
}
