using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureMiddayMingle.Models;
using System.Configuration;
using System.Data.SqlClient;
using Foursquare.Api;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http;

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
    
        
        public ActionResult CreateProfile()
        {
            return View();
        }

        public ActionResult Success(Employee e)
        {
            EmployeesController ec = new EmployeesController();
            ec.Create(e);

            return View();
        }

        public ActionResult Suggestions()
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://api.foursquare.com/v2/venues/search?client_id=GXLVM2YRQLY4BF5YAY2QN1UVDBDKYU0IL5420SJTGTI4RZ5T&client_secret=12OG4YUDEFBX32OFGYJNUHWOZUPV2JLKZG2RNKU1FPFJNV5H&intent=checkin&near=Grand%20Rapids&categoryId=4bf58dd8d48988d10a941735&radius=8047&v=20171109");
            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            String ApiText = rd.ReadToEnd();
            JObject o = JObject.Parse(ApiText);
        
            string input = o["response"]["venues"][0]["name"].ToString();
               
            ViewBag.SuggestedRestaurants = input;

            return View();
        }
    }
}