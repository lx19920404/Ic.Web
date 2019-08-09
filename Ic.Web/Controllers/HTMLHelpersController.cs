using Ic.Web.Extensions;
using Ic.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Web.Controllers
{
    public class HTMLHelpersController:Controller
    {
        private EventsMenusContext _context;
        public HTMLHelpersController(EventsMenusContext eventsMenusContext)
        {
            _context = eventsMenusContext;
        }
        public IActionResult HelperWithMenu() => View(GetSampleMenu());
        public IActionResult StronglyTypedMenu() => View(GetSampleMenu());
        public IActionResult Display() => View(GetSampleMenu());
        public IActionResult HelperList()
        {
            var cars = new Dictionary<int, string>();
            cars.Add(1, "Red Bull Racing");
            cars.Add(2, "McLaren");
            cars.Add(3, "Mercedes");
            cars.Add(4, "Ferrari");
            return View(cars.ToSelectListItems(4));
        }
        private Menu GetSampleMenu() => _context.Menus.FirstOrDefault();
            //new Menu
            //{
            //    Id = 1,
            //    Text = "Schweinsbraten mit Knodel und Sauerkraut",
            //    Price = 6.9,
            //    Category = "Main"
            //};
    }
}
