using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Blog.DbContexts
{
    [Table("Blogs")]
    public class Blog
    {
        
        [Column("ID")]
        public int ID { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Content")]
        public string Content { get; set; }
    }
}
