using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welome to tutorial home page";
            return View(); 
        }

        public ActionResult About()
        { return View(); }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Movies()
        {
            ViewBag.Message = "Your movies page.";

            return View();
        }
    }
}
