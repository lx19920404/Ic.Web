using Ic.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Web.Controllers
{
    public class TagHelpersController:Controller
    {
        public IActionResult Index() => View();
        public IActionResult LabelHelper() => View(GetSampleMenu());
        public IActionResult FormHelper() => View(GetSampleMenu());
        [HttpPost]
        public IActionResult FormHelper(Menu m)
        {
            if(!ModelState.IsValid)
            {
                return View(m);
            }
            return View("ValidationHelperResult", m);
        }
        public IActionResult CustomHelper() => View(GetSampleMenus());
        private Menu GetSampleMenu()
        {
            return new Menu
            {
                Id = 1,
                Text = "Schweinsbraten mit Knodel und Sauekraut",
                Price = 6.9,
                Date = new DateTime(2016, 10, 5),
                Category = "Main"
            };
        }
        private List<Menu> GetSampleMenus() =>
            new List<Menu>()
            {
                new Menu
                {
                    Id = 1,
                    Text = "Schweinsbraten mit Knodel und Sauekraut",
                    Price = 6.9,
                    Date = new DateTime(2016, 10, 5),
                    Category = "Main"
                },
                new Menu
                {
                    Id = 2,
                    Text = "Erdapfelgulasch mit Tofu und Geback",
                    Price = 8.5,
                    Date = new DateTime(2016, 10, 6),
                    Category = "Vegetarian"
                },
                new Menu
                {
                    Id = 3,
                    Text = "Tiroler Bauerngrost mit Spidgelei und Krautsalat",
                    Price = 8.5,
                    Date = new DateTime(2016, 10, 7),
                    Category = "Vegetarian"
                }
            };
    }
}
