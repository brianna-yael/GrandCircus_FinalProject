using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureMiddayMingle.Models;

namespace AzureMiddayMingle.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Suggestions()
        {
            return View();
        }       

        public ActionResult Success(Employee e)
        {
            EmployeesController ec = new EmployeesController();
            ec.Create(e);

            return View();
        }
    }
}