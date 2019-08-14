using Ic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;


namespace Ic.Web.Models
{
    public class mNavConfig : mConfigFile
    {
        public mNavConfig(string fileName) : base(fileName)
        {
            if (!ConfigFileInfo.Exists)
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigFileInfo.FullName);
            XmlNode root = doc.SelectSingleNode("NavConfig");
            XmlNodeList navGroups = root.SelectNodes("NavGroup");
            NavGroups = new List<NavGroup>();
            foreach (XmlNode navGroup in navGroups)
                NavGroups.Add(CreateGroup(navGroup));
        }
        private NavGroup CreateGroup(XmlNode navGroup)
        {
            NavGroup group = new NavGroup();
            group.Title = (navGroup as XmlElement).GetAttribute("Title");
            group.Menus = new List<NavMenu>();
            foreach (XmlNode navMenu in navGroup)
                group.Menus.Add(CreateMenu(navMenu));
            return group;
        }
        private NavMenu CreateMenu(XmlNode navMenu)
        {
            NavMenu menu = new NavMenu();
            XmlElement element = navMenu as XmlElement;
            menu.Title = element.GetAttribute("Title");
            menu.Link = element.GetAttribute("Link");
            menu.Controller = element.GetAttribute("Controller");
            menu.Action = element.GetAttribute("Action");
            menu.Para = element.GetAttribute("Para");
            if (navMenu.HasChildNodes)
            {
                menu.Childrens = new List<NavMenu>();
                foreach (XmlNode child in navMenu)
                    menu.Childrens.Add(CreateMenu(child));
            }
            return menu;
        }
        public List<NavGroup> NavGroups { get; set; }
    }
    public class NavGroup
    {
        public string Title { get; set; }
        public List<NavMenu> Menus { get; set; }
    }
    public class NavMenu
    {
        public string Title { get; set; }
        //如果有link优先,没有的话走控制器
        public string Link { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Para { get; set; }
        public List<NavMenu> Childrens { get; set; }
    }
}
