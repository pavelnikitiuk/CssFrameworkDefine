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
        // GET: Home
        public ActionResult Index()
        {
            Definer definer = new Definer();

            Stopwatch t;

            definer.AddFramework("Bootstrap",out t, frameworks.Find(x => x.FrameworkName == "Bootstrap").Paths.ToArray());

            return View(new FrameworkInfoModel { Frameworks = new List<FrameworkModel>() });
        }
        [HttpPost]
        public ActionResult Index(string url)
        {
            

            return View();
        }
    }
}