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
using System.Globalization;

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
            ViewBag.Message = "About those who brought you Midday Mingle";

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

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ViewMyProfile(String userEmail)
        {

            MiddayMingleAzureEntities db = new MiddayMingleAzureEntities();
            Employee me = db.Employees.Find(Convert.ToInt32(Session["EmployeeID"]));
            if (me == null)
            {
                List<Employee> allEmployees = db.Employees.ToList();
                Employee currentUser = new Employee();

                foreach (Employee e in allEmployees)
                {
                    if (userEmail == e.EmployeeEmail)
                    {
                        currentUser = e;
                    }
                }

                Session["EmployeeID"] = currentUser.EmployeeID;
                me = db.Employees.Find(Convert.ToInt32(Session["EmployeeID"]));
                Session["MatchIndex"] = 1;

            }

            if (me == null)
            {
                ViewBag.MyName = "Please log in or create a profile first.";
                Session["EmployeeID"] = null;
            }
            else
            {
                ViewBag.MyName = me.EmployeeName;
                ViewBag.MyEmail = me.EmployeeEmail;
                ViewBag.MyPosition = me.Position;
                ViewBag.MyInterest = me.Interest1;
                ViewBag.MyCuisine = me.Cuisine1;
                ViewBag.MyCompany = me.Company.CompanyName;
            }

            return View();
        }

        public ActionResult MyProfile(Employee e)
        {
            EmployeesController ec = new EmployeesController();
            ec.Create(e);

            MiddayMingleAzureEntities db = new MiddayMingleAzureEntities();

            Employee me = db.Employees.ToList().Last();
            Session["EmployeeID"] = me.EmployeeID;
            Session["MatchIndex"] = 1;

            ViewBag.MyName = me.EmployeeName;
            ViewBag.MyEmail = me.EmployeeEmail;
            ViewBag.MyPosition = me.Position;
            ViewBag.MyInterest = me.Interest1;
            ViewBag.MyCuisine = me.Cuisine1;
            ViewBag.MyCompany = me.Company.CompanyName;

            return View("ViewMyProfile");
        }

        public Employee GetEmployeeMatch()
        {
            MiddayMingleAzureEntities db = new MiddayMingleAzureEntities();
            Employee e = db.Employees.Find(Convert.ToInt32(Session["EmployeeID"]));

            //Finding employee match
            List<Employee> allEmployees = db.Employees.ToList();
            Employee myMatch = null;

            for (int i = Convert.ToInt32(Session["MatchIndex"]); i < allEmployees.Count(); i++)
            {
                Employee emp = allEmployees.ElementAt(i);
                if ((emp.CompanyID == e.CompanyID) && (emp.Interest1 == e.Interest1) && (emp.Cuisine1 == e.Cuisine1) && (emp.EmployeeID != e.EmployeeID))
                {
                    Session["MatchIndex"] = i + 1;
                    myMatch = emp;
                    return myMatch;
                }
            }

            return null;

        }

        public ActionResult Logoff()
        {
            Session["EmployeeID"] = null;
            Session["MatchIndex"] = null;
            return View("Login");
        }

        public ActionResult Suggestions()
        {
            MiddayMingleAzureEntities db = new MiddayMingleAzureEntities();
            Employee e = db.Employees.Find(Convert.ToInt32(Session["EmployeeID"]));
            ViewBag.SuggestedRestaurants = null;

            //Finding employee match
            Employee myMatch = null;
            if (e == null)
            {
                ViewBag.MatchName = "Sorry, You must make a profile first or log in to find your suggestions.";
                return View();
            }

            myMatch = GetEmployeeMatch();

            if (myMatch == null)
            {
                ViewBag.MatchName = "I'm sorry, no one has signed up yet who is a match for you. Please try again at a later date.";
            }
            else
            {
                ViewBag.MatchName = "Your Match is " + myMatch.EmployeeName;
                ViewBag.MatchPosition = "Position: " + myMatch.Position;
                ViewBag.MatchInterest = "Like you, they are also interested in " + myMatch.Interest1;
                ViewBag.MatchCuisine = "You both enjoy eating " + myMatch.Cuisine1;
                ViewBag.MatchEmail = "You can contact them at " + myMatch.EmployeeEmail;
            }

            //Suggesting Restaurants
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
                string[] restaurants = new string[o["response"]["venues"].Count()];

                for (int i = 0; i < o["response"]["venues"].Count(); i++)
                {
                    string input = o["response"]["venues"][i]["name"].ToString();
                    restaurants[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
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
                string[] restaurants = new string[o["response"]["venues"].Count()];

                for (int i = 0; i < o["response"]["venues"].Count(); i++)
                {
                    string input = o["response"]["venues"][i]["name"].ToString();
                    restaurants[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
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
                string[] restaurants = new string[o["response"]["venues"].Count()];

                for (int i = 0; i < o["response"]["venues"].Count(); i++)
                {
                    string input = o["response"]["venues"][i]["name"].ToString();
                    restaurants[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
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
                string[] restaurants = new string[o["response"]["venues"].Count()];

                for (int i = 0; i < o["response"]["venues"].Count(); i++)
                {
                    string input = o["response"]["venues"][i]["name"].ToString();
                    restaurants[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
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
                string[] restaurants = new string[o["response"]["venues"].Count()];

                for (int i = 0; i < o["response"]["venues"].Count(); i++)
                {
                    string input = o["response"]["venues"][i]["name"].ToString();
                    restaurants[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
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