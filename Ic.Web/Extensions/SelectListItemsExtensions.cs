using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Web.Extensions
{
    public static class SelectListItemsExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(
            this IDictionary<int,string> dict,int selectedId)
        {
            return dict.Select(item =>
                new SelectListItem
                {
                    Selected = item.Key == selectedId,
                    Text = item.Value,
                    Value = item.Key.ToString()
                });
        }
    }
}
