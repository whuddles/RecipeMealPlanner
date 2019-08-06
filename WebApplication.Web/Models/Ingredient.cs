using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public int UnitId { get; set; }
        public string Unit { get; set; }
        public string Number { get; set; }
        public int FractionId { get; set; }
        public string Fraction { get; set; }
    }
}
