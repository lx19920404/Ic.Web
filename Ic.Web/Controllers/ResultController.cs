using Ic.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Ic.Web.Controllers
{
    public class ResultController:Controller
    {
        public IActionResult ContentDemo() => Content("Hello World", "text/plain");
        public IActionResult JsonDemo()
        {
            var m = new Menu
            {
                Id = 3,
                Text = "Grilled sausage with sauerkraut and potatoes",
                Price = 12.90,
                Date = new DateTime(2016, 3, 31),
                Category = "Main"
            };
            return Json(m);
        }
        public IActionResult RedirectDemo() => Redirect("http://www.baidu.com");
        public IActionResult RedirectRouteDemo() => RedirectToRoute(new { controller = "Home", action = "Hello" });
        public IActionResult FileDemo() => File("~/images/Mattias.jpg", "image/jpeg");
    }
}
