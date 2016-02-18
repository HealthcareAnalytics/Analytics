using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthcareAnalytics.Models;

namespace HealthcareAnalytics.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HosptialDBContext db = new HosptialDBContext();

            Employee e = new Employee(
                new NameInformation("brandon","tran","test","test", "", ""), 
                new ContactInformation("123 add st", "Vancouver", "BC", "CA", "123123", null, null, null, null),
                new ContactInformation("123 add st", "Vancouver", "BC", "CA", "123123", null, null, null, null),
                "None",
                new Branch()
            );
            
            db.Employees.Add(e);

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
    }
}