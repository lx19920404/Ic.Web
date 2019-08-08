using Ic.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Web.Controllers
{
    public class ViewsDemoController:Controller
    {
        private EventsAndMenusContext _context;
        public ViewsDemoController(EventsAndMenusContext context) //根据类型依赖注入创建控制器
        {
            _context = context;
        }
        public IActionResult Index() => View();
        public IActionResult LayoutSample() => View();
        public IActionResult LayoutUsingSections() => View();
        public IActionResult PassingAModel()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Id = 1,
                    Text = "Schweinsbraten mit knodel und Sauerkraut",
                    Price = 6.9,
                    Category = "Main"
                },
                new Menu
                {
                    Id = 2,
                    Text = "Erdapfelgulasch mit Tofu und Geback",
                    Price = 6.9,
                    Category = "Vegetarian"
                },
                new Menu
                {
                    Id = 3,
                    Text = "Tiroler Bauerngrost 'l mit Spiegelei und Krautsalat",
                    Price = 6.9,
                    Category = "Main"
                }
            };
            return View(menus);
        }

        public IActionResult UseAPartialView1() => View(_context);//使用服务器端代码返回部分视图
        public IActionResult UseAPartialView2() => View();
        public IActionResult ShowEvents()
        {
            ViewBag.EventsTitle = "Live Events";
            return PartialView(_context.Events);
        }

    }
}
