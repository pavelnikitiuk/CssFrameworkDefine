using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using CssFrameworkDefine;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        List<CssFrameworkIdentity> frameworks = new List<CssFrameworkIdentity>();
        public void FillFramework()
        {
            frameworks.Add(new CssFrameworkIdentity
            {
                FrameworkName = "Bootstrap",
                Paths = new List<string> { System.Web.HttpContext.Current.Server.MapPath("~/Content/bootstrap.min.css") },
            });
            frameworks.Add(new CssFrameworkIdentity
            {
                FrameworkName = "Foundation",
                Paths = new List<string> { System.Web.HttpContext.Current.Server.MapPath("~/Content/foundation/foundation.css") },
            });
            frameworks.Add(new CssFrameworkIdentity
            {
                FrameworkName = "Uikit",
                Paths = new List<string> { System.Web.HttpContext.Current.Server.MapPath("~/Content/css/uikit.almost-flat.min.css") },
            });
        }
        // GET: Home
        public ActionResult Index()
        {
            return View(new FrameworkInfoModel { Frameworks = new List<FrameworkModel>()});
        }
        [HttpPost]
        public ActionResult Index(string url)
        {
            FillFramework();
            FrameworkInfoModel model = new FrameworkInfoModel
            {
                Frameworks = new List<FrameworkModel>()
            };
            Parser p = new Parser
            {
                Url = url,

            };
            foreach (var framework in frameworks)
            {
                p.SearchinFramework = framework;
                p.Parse();
                model.Frameworks.Add(new FrameworkModel
                {
                    Name = framework.FrameworkName,
                    MatchCount = framework.UsesClassesCount,
                    UsePersent = (double)framework.matchesCss.Count / framework.AllClassCount * 100
                });
            }
            try
            {

            }
            catch
            {

            }

            return View(model);
        }
    }
}