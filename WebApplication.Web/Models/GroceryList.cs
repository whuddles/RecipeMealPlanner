using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class GroceryList
    {
        public int GroceryListId { get; set; }
        public string GroceryListName { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<int> IngredientIds { get; set; }
    }
}
