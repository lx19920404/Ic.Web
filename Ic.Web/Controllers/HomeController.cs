using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Ic.Web.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Index() => View();
        public string Hello() => "Hello, ASP.NET MVC 6";
        public string GetString(string name) => HtmlEncoder.Default.Encode($"Hello,{name}");
        public int Add(int x, int y) => x + y;
    }
}
