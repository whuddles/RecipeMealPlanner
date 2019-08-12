using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Day
    {
        public int DayId { get; set; }
        public string DayName { get; set; }
        public Meal Breakfast { get; set; }
        public Meal Lunch { get; set; }
        public Meal Dinner { get; set; }
    }
}
