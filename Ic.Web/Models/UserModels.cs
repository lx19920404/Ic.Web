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
        [Form(FormItemType.Text, "")]
        public string Id { get; set; }
        [Form(FormItemType.Text, "")]
        public string Password { get; set; }
        [Form(FormItemType.Text,"")]
        public string Nick { get; set; }
        [Form(FormItemType.Text, "")]
        public string Name { get; set; }
        [Form(FormItemType.Text, "")]
        public string Sex { get; set; }
        [Form(FormItemType.Text, "")]
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
