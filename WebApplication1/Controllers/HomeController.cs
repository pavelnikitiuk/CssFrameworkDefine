using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using CssFrameworkDefine;
using System.Diagnostics;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        List<CssFrameworkIdentity> frameworks;
        public HomeController()
        {
            frameworks = Plugins.FrameworkLoader.Load(System.Web.HttpContext.Current.Server.MapPath("~/Content/css/"));
        }
        public void FillFramework()
        {
            frameworks.Add(new CssFrameworkIdentity("Bootstrap", new List<string> { System.Web.HttpContext.Current.Server.MapPath("~/Content/bootstrap.min.css") }));

            frameworks.Add(new CssFrameworkIdentity("Foundation", new List<string> { System.Web.HttpContext.Current.Server.MapPath("~/Content/css/foundation.css"),
                System.Web.HttpContext.Current.Server.MapPath("~/Content/css/foundation.mvc.css"),
                System.Web.HttpContext.Current.Server.MapPath("~/Content/css/foundation.min.css"),
            }));

            frameworks.Add(new CssFrameworkIdentity("Uikit", new List<string> { 
                System.Web.HttpContext.Current.Server.MapPath("~/Content/css/uikit.almost-flat.min.css"),
                System.Web.HttpContext.Current.Server.MapPath("~/Content/css/uikit.min.css"), 
                System.Web.HttpContext.Current.Server.MapPath("~/Content/css/uikit.gradient.min.css") 
            }));

            //frameworks.Add()
        }
        // GET: Home
        public ActionResult Index()
        {
            return View(new FrameworkInfoModel { Frameworks = new List<FrameworkModel>() });
        }
        [HttpPost]
        public ActionResult Index(string url)
        {
            Stopwatch t = new Stopwatch();
            t.Start();
            FrameworkInfoModel model = new FrameworkInfoModel
            {
                Frameworks = new List<FrameworkModel>()
            };
            Parser p = new Parser(url);
            foreach (var framework in frameworks)
            {
                p.SearchinFramework = framework;
                p.Parse();
                model.Frameworks.Add(new FrameworkModel
                {
                    Name = framework.FrameworkName,
                    MatchCount = framework.UsesClassesCount,
                    UsePersent = framework.matchesCss.Count
                });
            }

            t.Stop();

            return View(model);
        }
    }
}