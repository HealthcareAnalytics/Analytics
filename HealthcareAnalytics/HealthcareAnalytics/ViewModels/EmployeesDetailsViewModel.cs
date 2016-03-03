using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using HealthcareAnalytics.Models;

namespace HealthcareAnalytics.ViewModels
{
    public class EmployeesDetailsViewModel
    {
        public Employee Employee { get; set; }

        [DisplayName("Address")]
        public string FullAddress { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0}, {1} {2}", Employee.NameDetails.LastName,
                    Employee.NameDetails.FirstName, Employee.NameDetails.MiddleName);
            }
        }

    }
}