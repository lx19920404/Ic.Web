using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ic.Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Ic.Blog.DbContexts;
using Westwind.AspNetCore.Markdown;
using System.IO;

namespace Ic.Blog.Controllers
{
    //[Authorize]
    [Route("[Controller]/[Action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BlogDbContext DbContext;

        public HomeController(ILogger<HomeController> logger, BlogDbContext dbContext)
        {
            _logger = logger;
            DbContext = dbContext;
        }

        public IActionResult Index()
        {
            ViewData["BlogTitle"] = DbContext.Blogs.FirstOrDefault().Title;
            string path = @"D:\Code\Note\Note\";
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            ViewData["Blogs"] = files.Where(p => p.Extension.ToUpper() == ".MD").Select(p => new string[] { Path.GetFileNameWithoutExtension(p.FullName), p.FullName }).ToList();
            string blog = Path.GetFileNameWithoutExtension(files.FirstOrDefault().FullName);
            ViewData["BlogName"] = blog;

            return View();
        }
        [HttpGet("{blog}")]
        public IActionResult Index(string blog)
        {
            ViewData["BlogTitle"] = DbContext.Blogs.FirstOrDefault().Title;
            string path = @"D:\Code\Note\Note\";
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            ViewData["Blogs"] = files.Where(p => p.Extension.ToUpper() == ".MD").Select(p => new string[] { Path.GetFileNameWithoutExtension(p.FullName), p.FullName }).ToList();

            ViewData["BlogName"] = blog;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
