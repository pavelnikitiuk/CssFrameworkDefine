using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CssFrameworkDefine;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            return View("~/Views/Home/Index.cshtml", null, null);
        }
        [HttpPost]
        public ActionResult Index(string url)
        {
            CssFrameworkIdentity bootstrap = new CssFrameworkIdentity
            {
                SearchDivClasses = new List<string> { @"col-[mdxslgm]{2}-\d{1,2}" },
                SearchLinks = new List<string> { "bootstrap" },
                NameOfClassInCss = new List<string>{@"col-[mdxslgm]{2}-\d{1,2}","Bootstrap"}
            };

            Parser p = new Parser(url);
            p.SearchinFramework.Add(bootstrap);
            p.Parse();
            return View("~/Views/Home/Index.cshtml", null, null);
        }
    }
}