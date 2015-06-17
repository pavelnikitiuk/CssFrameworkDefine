using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using CssFrameworkDefine;
using CssFrameworkDefine.Old;
using CssFrameworkDefine.Model;
using System.Diagnostics;
using HtmlAgilityPack;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        List<CssFrameworkIdentity> frameworks;
        CssFrameworkDefine.Definer definer = new CssFrameworkDefine.Definer();
        Stopwatch t;
        public HomeController()
        {
            frameworks = Plugins.FrameworkLoader.Load(System.Web.HttpContext.Current.Server.MapPath("~/Content/css/"));
            

            

            foreach (var name in frameworks)
            {
                definer.AddFramework(name.FrameworkName, name.Paths.ToArray());


            }

        }
        // GET: Home
        public ActionResult Index()
        {


            return View(new FrameworkInfoModel { Frameworks = new List<FrameworkModel>() });
        }
        [HttpPost]
        public ActionResult Index(string url)
        {
            Stopwatch timer = new Stopwatch();
            
            FrameworkInfoModel model = new FrameworkInfoModel { Frameworks = new List<FrameworkModel>() };
            timer.Start();
            CssLoader cssLoader = new CssLoader(url);
            timer.Stop();
            foreach(var css in cssLoader.LoadLinks())
            {
                var res = definer.Define(css, out t);
                foreach (var result in res)
                {
                    model.Frameworks.Add(new FrameworkModel { Name = result.Key, MatchCount = result.Value, UsePersent = t.ElapsedMilliseconds });
                }
            }
            

            
            
            
            model.Frameworks.Last().UsePersent = timer.ElapsedMilliseconds;

            return View(model);
        }
    }
}