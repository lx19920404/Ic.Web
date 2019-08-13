using Ic.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Web.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        [Form(FormItemType.Text,"")]
        public string Nick { get; set; }
        public string Name { get; set; }
        //[Form(FormItemType.ComboBox,"男，女")]
        public string Sex { get; set; }
        public string Mail { get; set; }
    }
    public class UsersContext : DbContext
    {
        //public UsersContext()
        //{

        //}
        public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=mydb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        //}
    }
}
