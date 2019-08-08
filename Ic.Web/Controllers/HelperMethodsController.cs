using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Web.Controllers
{
    public class HelperMethodsController:Controller
    {
        public IActionResult SimpleHelper() => View();
    }
}
