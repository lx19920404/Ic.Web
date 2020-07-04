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
    public class ArticleController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private BlogDbContext DbContext;

        public ArticleController(ILogger<ArticleController> logger, BlogDbContext dbContext)
        {
            _logger = logger;
            DbContext = dbContext;
        }

        public IActionResult Index()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(path, "..", "..", "..", "..", "..", "..");
            path = Path.Combine(path, "blog", "Blog");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            ViewData["Blogs"] = files.Where(p => p.Extension.ToUpper() == ".MD").Select(p => new string[] { Path.GetFileNameWithoutExtension(p.FullName), p.FullName }).OrderBy(p => p[0]).ToList();
            string blog = Path.GetFileNameWithoutExtension(files.FirstOrDefault().FullName);
            ViewData["BlogName"] = blog;
            ViewData["Path"] = path;
            return View();
        }
        [HttpGet("{blog}")]
        public IActionResult Index(string blog)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(path, "..", "..", "..", "..", "..", "..");
            path = Path.Combine(path, "blog", "Blog");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            ViewData["Blogs"] = files.Where(p => p.Extension.ToUpper() == ".MD").Select(p => new string[] { Path.GetFileNameWithoutExtension(p.FullName), p.FullName }).OrderBy(p => p[0]).ToList();

            ViewData["BlogName"] = blog;
            ViewData["Path"] = path;
            return View();
        }
        public IActionResult List(string keyword)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(path, "..", "..", "..", "..", "..", "..");
            path = Path.Combine(path, "blog", "Blog");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            ViewData["Blogs"] = files.Where(p => p.Extension.ToUpper() == ".MD").Select(p => new string[] { Path.GetFileNameWithoutExtension(p.FullName), p.FullName }).OrderBy(p => p[0]).ToList();
            return View();
        }
        public IActionResult List()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(path, "..", "..", "..", "..", "..", "..");
            path = Path.Combine(path, "blog", "Blog");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            ViewData["Blogs"] = files.Where(p => p.Extension.ToUpper() == ".MD").Select(p => new string[] { Path.GetFileNameWithoutExtension(p.FullName), p.FullName }).OrderBy(p => p[0]).ToList();
            string blog = Path.GetFileNameWithoutExtension(files.FirstOrDefault().FullName);
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
