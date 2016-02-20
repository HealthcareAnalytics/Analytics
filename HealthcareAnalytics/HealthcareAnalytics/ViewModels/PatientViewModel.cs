using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HealthcareAnalytics.Models;
namespace HealthcareAnalytics.ViewModels
{
    public class PatientsViewModel
    {
        public Patient Patient { get; set; }
        public Person Person { get; set; }
        public NameDetails NameDetails { get; set; }
        public ContactInformation HomeContactInfo { get; set; }
        public ContactInformation WorkContactInfo { get; set; }
    }
}