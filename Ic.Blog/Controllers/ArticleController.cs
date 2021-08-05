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
            ViewData["Blogs"] = GetBlogSummaries();
            ViewData["Tags"] = allTags;
            ViewData["Blog"] = (ViewData["Blogs"] as List<BlogSummary>)[0];
            return View();
        }
        [HttpGet("{blog}")]
        public IActionResult Index(string blog)
        {
            ViewData["Blogs"] = GetBlogSummaries();
            ViewData["Tags"] = allTags;
            ViewData["Blog"] = (ViewData["Blogs"] as List<BlogSummary>).FirstOrDefault(p => p.title == blog);
            return View();
        }
        [HttpGet("{keyword}")]
        public IActionResult List(string keyword)
        {
            ViewData["Blogs"] = GetBlogSummaries().Where(p => p.keyword.Contains(keyword)).ToList();
            ViewData["Tags"] = allTags;
            return View();
        }
        public IActionResult List()
        {
            ViewData["Blogs"] = GetBlogSummaries();
            ViewData["Tags"] = allTags;
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
                StreamReader sr2 = new StreamReader(p.FullName);
                string tags = sr2.ReadLine();
                blog.keyword = new string[] {};
                if (tags != null && tags.Trim().ToUpper().Contains("TAG"))
                {
                    blog.keyword = tags.Split(" ").Skip(1).ToArray();
                    for(int i= 0; i < blog.keyword.Length; i++)
                    {
                        if (!allTags.Contains(blog.keyword[i]))
                            allTags.Add(blog.keyword[i]);
                    }
                    if (string.IsNullOrEmpty(blog.image))
                    {
                        blog.image = blog.keyword[0] + ".jpg";
                    }
                }
                blog.path = p.FullName;
                return blog;
            }).OrderBy(p => p.title).ToList();
        }

        private List<string> allTags = new List<string>();
    }
}
