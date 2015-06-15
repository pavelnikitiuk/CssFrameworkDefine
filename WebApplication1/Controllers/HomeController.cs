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
            return View(new FrameworkInfoModel { Frameworks = new List<FrameworkModel>() });
        }
        [HttpPost]
        public ActionResult Index(string url)
        {
            t.Start();
            FrameworkInfoModel model = new FrameworkInfoModel
            {
                Frameworks = new List<FrameworkModel>()
            };

            Definer p = new Definer(url);

            foreach (var framework in frameworks)
            {
                p.SearchinFramework = framework;
                p.Define();
                model.Frameworks.Add(new FrameworkModel
                {
                    Name = framework.FrameworkName,
                    MatchCount = framework.UsesClassesCount,
                    UsePersent = framework.MatchesCss.Count
                });
            }


            return View(model);
        }
    }
}