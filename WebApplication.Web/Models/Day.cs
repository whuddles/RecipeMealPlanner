using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Day
    {
        int DayId { get; set; }
        string DayName { get; set; }
        Meal Breakfast { get; set; }
        Meal Lunch { get; set; }
        Meal Dinner { get; set; }
    }
}
