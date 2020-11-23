using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Blog.Models
{
    public class BlogSummary
    {
        public string title { get; set; }
        public string summary { get; set; }
        public string image { get; set; }
        public string[] keyword { get; set; }
        public string path { get; set; }
    }
}
