using Ic.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Web.Controllers
{
    public class SubmitDataController:Controller
    {
        public IActionResult PassingData()
        {
            ViewBag.MyData = "Hello from the controller";
            return View();
        }
        public IActionResult CreateMenu() => View();
        [HttpPost]
        public IActionResult CreateMenu(int id,string text,double price,string category)
        {
            var m = new { Id = id, Text = text, Price = price,Category = category };
            ViewBag.Info =
                $"menu created:{m.Text},Price:{m.Price},CateGory:{m.Category}";
            return View("Index");
        }
        [HttpPost]
        public IActionResult CreateMenu2(Menu m)
        {
            ViewBag.Info =
                $"menu created: {m.Text}, Price:{m.Price}, category:{m.Category}";
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CreateMenu3Result()
        {
            var m = new Menu();
            bool updated = await TryUpdateModelAsync<Menu>(m);
            if(updated)
            {
                ViewBag.Info = 
                    $"menu created: {m.Text}, Price:{m.Price}, category:{m.Category}";
                return View("Index");
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public IActionResult CreateMenu4(Menu m)
        {
            if(ModelState.IsValid)
            {
                ViewBag.Info = 
                    $"menu created: {m.Text}, Price:{m.Price}, category:{m.Category}";
            }
            else
            {
                ViewBag.Info = "not valid";
            }
            return View("Index");
        }
    }
}
