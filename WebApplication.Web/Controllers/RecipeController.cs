using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Web.Controllers
{
    public class RecipeController : Controller
    {
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}