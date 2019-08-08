using Ic.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Web.ViewComponents
{
    public class EventListViewComponent:ViewComponent
    {
        private readonly EventsAndMenusContext _context;
        public EventListViewComponent(EventsAndMenusContext context)
        {
            _context = context;
        }
        public Task<IViewComponentResult> InvokeAsync(DateTime from,DateTime to)
        {
            return Task.FromResult<IViewComponentResult>(
                View(EventsByDateRange(from, to)));
        }
        private IEnumerable<Event> EventsByDateRange(DateTime from, DateTime to)
        {
            return _context.Events.Where(e => e.Day >= from && e.Day <= to);
        }
    }
}
