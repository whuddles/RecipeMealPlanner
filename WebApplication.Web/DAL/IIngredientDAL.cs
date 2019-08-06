using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IIngredientDAL
    {
        List<Ingredient> GetIngredients();

        bool AddIngredient(string name);
    }
}
