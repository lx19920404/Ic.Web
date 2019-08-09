using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ic.Web.Models
{
    public class EventsMenusContext:DbContext
    {
        //public EventsMenusContext(DbContextOptions options):base(options) { }
        public EventsMenusContext(DbContextOptions<EventsMenusContext> options) : base(options) { }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
