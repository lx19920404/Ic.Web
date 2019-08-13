using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ic.Web.Extensions
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("form",Attributes = AttributesName)]
    public class FormTagHelper : TagHelper
    {
        private const string AttributesName = "type";
        [HtmlAttributeName(AttributesName)]
        public object type { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder form = new TagBuilder("form");
            base.Process(context, output);
            form.GenerateId(context.UniqueId, "id");
            var otherAttributes = context.AllAttributes.Where(p => p.Name != AttributesName).ToDictionary(p => p.Name);
            form.MergeAttributes(otherAttributes);
            Type tp = type.GetType();
            PropertyInfo[] propertyInfos = tp.GetProperties();
            foreach (PropertyInfo propertyinfo in propertyInfos)
            {
                Attribute[] attributes = propertyinfo.GetCustomAttributes().ToArray();
                foreach (Attribute attribute in attributes)
                {
                    if (attribute is FormAttribute)
                    {
                        FormAttribute formAttribute = attribute as FormAttribute;
                        switch (formAttribute.Type)
                        {
                            case FormItemType.CheckBox:
                                break;
                            case FormItemType.Text:
                                TagBuilder div0 = new TagBuilder("div");
                                div0.Attributes.Add(new KeyValuePair<string, string>("asp-validation-summary", "ModelOnly"));
                                div0.Attributes.Add(new KeyValuePair<string, string>("class", "text-danger"));
                                TagBuilder div1 = new TagBuilder("div");
                                div1.Attributes.Add(new KeyValuePair<string, string>("class", "layui-form-item"));
                                TagBuilder label = new TagBuilder("label");
                                label.Attributes.Add(new KeyValuePair<string, string>("class", "layui-form-label"));
                                label.Attributes.Add(new KeyValuePair<string, string>("asp-for", propertyinfo.Name));
                                div1.InnerHtml.AppendHtml(label);
                                TagBuilder div2 = new TagBuilder("div");
                                div2.Attributes.Add(new KeyValuePair<string, string>("class", "layui-input-block"));
                                TagBuilder input = new TagBuilder("input");
                                input.Attributes.Add(new KeyValuePair<string, string>("class", "layui-input"));
                                input.Attributes.Add(new KeyValuePair<string, string>("asp-for", "layui-input"));
                                div2.InnerHtml.AppendHtml(input);
                                TagBuilder span = new TagBuilder("span");
                                span.Attributes.Add(new KeyValuePair<string, string>("class", "text-danger"));
                                span.Attributes.Add(new KeyValuePair<string, string>("asp-validation-for", propertyinfo.Name));
                                div2.InnerHtml.AppendHtml(span);
                                div1.InnerHtml.AppendHtml(div2);
                                form.InnerHtml.AppendHtml(div0);
                                form.InnerHtml.AppendHtml(div1);
                                break;
                            case FormItemType.ComboBox:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            output.Content.AppendHtml(form.InnerHtml);
        }
    }
}
