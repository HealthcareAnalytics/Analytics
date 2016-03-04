using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HealthcareAnalytics.Models;
using System.Data.Entity;



namespace HealthcareAnalytics.Controllers
{
    public class AnalyticsController : Controller
    {
        private HospitalDBContext db = new HospitalDBContext();

        [System.Web.Mvc.HttpGet]
        public JsonResult NumberOfIncidentsByType()
        {
            List<Incident> incidents = db.Incidents
                                    .Include(i => i.IncidentType)
                                    .ToList();

            List<Dictionary<string, object>> jsonObject = new List<Dictionary<string, object>>();

            jsonObject.Add(new Dictionary<string, object>());
            jsonObject[0].Add("type", "General Incident");
            jsonObject[0].Add("count", new int[12]);

            jsonObject.Add(new Dictionary<string, object>());
            jsonObject[1].Add("type", "Complaint");
            jsonObject[1].Add("count", new int[12]);

            jsonObject.Add(new Dictionary<string, object>());
            jsonObject[2].Add("type", "Fall");
            jsonObject[2].Add("count", new int[12]);

            foreach (Incident incident in incidents)
            {
                int month = incident.DateAndTime.Month - 1;

                if (incident.IncidentType.Name.Equals(jsonObject[0]["type"]))
                {
                    (jsonObject[0]["count"] as int[])[month]++;
                }
                else if (incident.IncidentType.Name.Equals(jsonObject[1]["type"]))
                {
                    (jsonObject[1]["count"] as int[])[month]++;
                }
                else if (incident.IncidentType.Name.Equals(jsonObject[2]["type"]))
                {
                    (jsonObject[2]["count"] as int[])[month]++;
                }
            }

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }
    }
}
