using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ic.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ic.Web.Extensions
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("div",Attributes = ItemsAttributesName)]
    public class NavTagHelper : TagHelper
    {
        private const string ItemsAttributesName = "nav-config";
        [HtmlAttributeName(ItemsAttributesName)]
        public mNavConfig Nav { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder nav = new TagBuilder("div");
            base.Process(context, output);
            nav.GenerateId(context.UniqueId, "id");
            var otherAttributes = context.AllAttributes.Where(p => p.Name != ItemsAttributesName).ToDictionary(p => p.Name);
            nav.MergeAttributes(otherAttributes);

            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("layui-nav");
            ul.Attributes.Add(new KeyValuePair<string, string>("lay-filter", ""));
            foreach (NavGroup group in Nav.NavGroups)
            {
                TagBuilder li = new TagBuilder("li");
                li.AddCssClass("layui-nav-item");
                TagBuilder a = new TagBuilder("a");
                a.Attributes.Add(new KeyValuePair<string, string>("href", "#"));
                a.InnerHtml.Append(group.Title);
                li.InnerHtml.AppendHtml(a);
                TagBuilder dl = new TagBuilder("dl");
                dl.AddCssClass("layui-nav-child");
                foreach (NavMenu menu in group.Menus)
                {
                    TagBuilder dd = new TagBuilder("dd");
                    TagBuilder a1 = new TagBuilder("a");
                    a1.Attributes.Add(new KeyValuePair<string, string>("href", $"/{menu.Controller}/{menu.Action}"));
                    a1.InnerHtml.Append(menu.Title);
                    dd.InnerHtml.AppendHtml(a1);
                    dl.InnerHtml.AppendHtml(dd);
                    li.InnerHtml.AppendHtml(dl);
                }
                ul.InnerHtml.AppendHtml(li);
            }
            nav.InnerHtml.AppendHtml(ul);
            output.Content.AppendHtml(nav.InnerHtml);
        }
    }
}
