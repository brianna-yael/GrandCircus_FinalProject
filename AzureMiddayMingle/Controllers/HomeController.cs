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
            ViewBag.Message = "About Midday Mingle";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Us!";

            return View();
        }


        public ActionResult CreateProfile()
        {
            return View();
        }

        public ActionResult MyProfile(Employee e)
        {
            EmployeesController ec = new EmployeesController();
            ec.Create(e);

            MiddayMingleAzureEntities db = new MiddayMingleAzureEntities();

            var employees = from f in db.Employees
                            where f.EmployeeEmail.Contains(e.EmployeeEmail)
                            select f;

            Employee me = db.Employees.ToList().Last();
            Session["EmployeeID"] = me.EmployeeID;
            return View(employees.ToList());
        }

        public ActionResult Suggestions()
        {
            MiddayMingleAzureEntities db = new MiddayMingleAzureEntities();
            Employee e = db.Employees.Find(Convert.ToInt32(Session["EmployeeID"]));
            if (e == null)
            {
                ViewBag.Error = "You must make a profile to find your suggested restaurants.";
                return View();
            }
            else if (e.Cuisine1 == "Ethiopian")
            {
                HttpWebRequest request = WebRequest.CreateHttp("https://api.foursquare.com/v2/venues/search?client_id=GXLVM2YRQLY4BF5YAY2QN1UVDBDKYU0IL5420SJTGTI4RZ5T&client_secret=12OG4YUDEFBX32OFGYJNUHWOZUPV2JLKZG2RNKU1FPFJNV5H&intent=checkin&near=Grand%20Rapids&categoryId=4bf58dd8d48988d10a941735&radius=8047&v=20171109");
                request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader rd = new StreamReader(response.GetResponseStream());
                String ApiText = rd.ReadToEnd();
                JObject o = JObject.Parse(ApiText);
                string restaurants = string.Empty;

                for (int i = 0; i < o["response"]["venues"].Count(); i++)
                {
                    string input = o["response"]["venues"][i]["name"].ToString();
                    restaurants = restaurants + input;
                }

                ViewBag.SuggestedRestaurants = restaurants;

                return View();
            }
            else if (e.Cuisine1 == "Chinese")
            {
                HttpWebRequest request = WebRequest.CreateHttp("https://api.foursquare.com/v2/venues/search?client_id=GXLVM2YRQLY4BF5YAY2QN1UVDBDKYU0IL5420SJTGTI4RZ5T&client_secret=12OG4YUDEFBX32OFGYJNUHWOZUPV2JLKZG2RNKU1FPFJNV5H&intent=checkin&near=Grand%20Rapids&categoryId=4bf58dd8d48988d145941735&radius=8047&v=20171109");
                request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader rd = new StreamReader(response.GetResponseStream());
                String ApiText = rd.ReadToEnd();
                JObject o = JObject.Parse(ApiText);
                string restaurants = string.Empty;

                for (int i = 0; i < o["response"]["venues"].Count(); i++)
                {
                    string input = o["response"]["venues"][i]["name"].ToString();
                    restaurants = restaurants + input;
                }

                ViewBag.SuggestedRestaurants = restaurants;

                return View();
            }
            else if (e.Cuisine1 == "Mediterranean")
            {
                HttpWebRequest request = WebRequest.CreateHttp("https://api.foursquare.com/v2/venues/search?client_id=GXLVM2YRQLY4BF5YAY2QN1UVDBDKYU0IL5420SJTGTI4RZ5T&client_secret=12OG4YUDEFBX32OFGYJNUHWOZUPV2JLKZG2RNKU1FPFJNV5H&intent=checkin&near=Grand%20Rapids&categoryId=4bf58dd8d48988d1c0941735&radius=8047&v=20171109");
                request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader rd = new StreamReader(response.GetResponseStream());
                String ApiText = rd.ReadToEnd();
                JObject o = JObject.Parse(ApiText);
                string restaurants = string.Empty;

                for (int i = 0; i < o["response"]["venues"].Count(); i++)
                {
                    string input = o["response"]["venues"][i]["name"].ToString();
                    restaurants = restaurants + input;
                }

                ViewBag.SuggestedRestaurants = restaurants;

                return View();
            }

            else if (e.Cuisine1 == "Mexican")
            {
                HttpWebRequest request = WebRequest.CreateHttp("https://api.foursquare.com/v2/venues/search?client_id=GXLVM2YRQLY4BF5YAY2QN1UVDBDKYU0IL5420SJTGTI4RZ5T&client_secret=12OG4YUDEFBX32OFGYJNUHWOZUPV2JLKZG2RNKU1FPFJNV5H&intent=checkin&near=Grand%20Rapids&categoryId=4bf58dd8d48988d1c1941735&radius=8047&v=20171109");
                request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader rd = new StreamReader(response.GetResponseStream());
                String ApiText = rd.ReadToEnd();
                JObject o = JObject.Parse(ApiText);
                string restaurants = string.Empty;

                for (int i = 0; i < o["response"]["venues"].Count(); i++)
                {
                    string input = o["response"]["venues"][i]["name"].ToString();
                    restaurants = restaurants + input;
                }

                ViewBag.SuggestedRestaurants = restaurants;

                return View();
            }

            else if (e.Cuisine1 == "Italian")
            {
                HttpWebRequest request = WebRequest.CreateHttp("https://api.foursquare.com/v2/venues/search?client_id=GXLVM2YRQLY4BF5YAY2QN1UVDBDKYU0IL5420SJTGTI4RZ5T&client_secret=12OG4YUDEFBX32OFGYJNUHWOZUPV2JLKZG2RNKU1FPFJNV5H&intent=checkin&near=Grand%20Rapids&categoryId=4bf58dd8d48988d110941735&radius=8047&v=20171109");
                request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader rd = new StreamReader(response.GetResponseStream());
                String ApiText = rd.ReadToEnd();
                JObject o = JObject.Parse(ApiText);
                string restaurants = string.Empty;

                for (int i = 0; i < o["response"]["venues"].Count(); i++)
                {
                    string input = o["response"]["venues"][i]["name"].ToString();
                    restaurants = restaurants + input;
                }

                ViewBag.SuggestedRestaurants = restaurants;

                return View();
            }

            else
            {
                return View();
            }
        }
    }
}