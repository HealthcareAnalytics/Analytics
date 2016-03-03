using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using HealthcareAnalytics.Models;

namespace HealthcareAnalytics.ViewModels
{
    public class PatientsDetailsViewModel
    {
        public Patient Patient { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0}, {1} {2}", Patient.NameDetails.LastName,
                    Patient.NameDetails.FirstName, Patient.NameDetails.MiddleName);
            }
        }

        [DisplayName("Address")]
        public string FullAddress { get; set; }
    }
}