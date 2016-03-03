using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using HealthcareAnalytics.Models;

namespace HealthcareAnalytics.ViewModels
{
    public class IncidentsViewModel
    {
        public Incident Incident { get; set; }

        [DisplayName("Patient")]
        public string PatientFullName
        {
            get
            {
                return string.Format("{0}, {1} {2}", Incident.Patient.NameDetails.LastName,
                    Incident.Patient.NameDetails.FirstName, Incident.Patient.NameDetails.MiddleName);
            }
        }

        [DisplayName("Reported By")]
        public string EmployeeFullName
        {
            get
            {
                return string.Format("{0}, {1} {2}", Incident.Employee.NameDetails.LastName,
                    Incident.Employee.NameDetails.FirstName, Incident.Employee.NameDetails.MiddleName);
            }
        }

    }
}