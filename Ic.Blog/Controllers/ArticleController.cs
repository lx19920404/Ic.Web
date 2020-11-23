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
            path = Path.Combine(path, "wwwroot", "md");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();

            ViewData["Blogs"] = GetBlogSummaries();

            string blog = Path.GetFileNameWithoutExtension(files.FirstOrDefault().FullName);
            ViewData["BlogName"] = blog;
            ViewData["Path"] = path;
            return View();
        }
        [HttpGet("{blog}")]
        public IActionResult Index(string blog)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(path, "wwwroot", "md");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            ViewData["Blogs"] = GetBlogSummaries();


            ViewData["BlogName"] = blog;
            ViewData["Path"] = path;
            return View();
        }
        [HttpGet("{keyword}")]
        public IActionResult List(string keyword)
        {
            ViewData["Blogs"] = GetBlogSummaries();
            return View();
        }
        public IActionResult List()
        {
            ViewData["Blogs"] = GetBlogSummaries();
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

        private List<BlogSummary> GetBlogSummaries()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(path, "wwwroot", "md");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            return files.Where(p => p.Extension.ToUpper() == ".MD").Select(p =>
            {
                BlogSummary blog = new BlogSummary();
                blog.title = Path.GetFileNameWithoutExtension(p.FullName);
                StreamReader sr = new StreamReader(p.FullName);
                string summary = sr.ReadToEnd();
                if (summary.Length > 200)
                    summary = summary.Substring(0, 200);
                blog.summary = summary;
                blog.keyword = null;
                return blog;
            }).OrderBy(p => p.title).ToList();
        }
    }
}
