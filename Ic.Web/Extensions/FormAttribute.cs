using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =false,Inherited =false)]
    public class FormAttribute:Attribute
    {
        public FormItemType Type { get; set; }
        public string Para { get; set; }
        public FormAttribute()
        {

        }
        public FormAttribute(FormItemType type,string para)
        {
            Type = type;
            Para = para;
        }
    }
    public enum FormItemType
    {
        CheckBox,
        Text,
        ComboBox
    }
}
