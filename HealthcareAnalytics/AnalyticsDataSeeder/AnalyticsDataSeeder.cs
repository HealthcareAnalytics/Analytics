using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HealthcareAnalytics.Models;

namespace AnalyticsDataSeeder
{
    class AnalyticsDataSeeder
    {
        Branch[] branches;
        private List<IncidentType> types;

        public AnalyticsDataSeeder()
        {
            branches = new Branch[3];
        }

        static void Main(string[] args)
        {
            AnalyticsDataSeeder a = new AnalyticsDataSeeder();

            a.ClearTableData();
            a.RunBase();

            new Thread(a.Run).Start();
            new Thread(a.Run).Start();
            new Thread(a.Run).Start();

            a.Run();
        }

        public void ClearTableData()
        {
            Console.WriteLine("Clearing database data...");
            HospitalDBContext context = new HospitalDBContext();
            context.Database.ExecuteSqlCommand("delete from Incidents");
            context.Database.ExecuteSqlCommand("delete from IncidentTypes");
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('IncidentTypes', RESEED, 1);");

            context.Database.ExecuteSqlCommand("delete from EmploymentDetails");
            context.Database.ExecuteSqlCommand("delete from Employees");

            context.Database.ExecuteSqlCommand("delete from CheckinDetails");
            context.Database.ExecuteSqlCommand("delete from Patients");
        
            context.Database.ExecuteSqlCommand("delete from NameDetails");
            context.Database.ExecuteSqlCommand("delete from ContactInformations");

            context.Database.ExecuteSqlCommand("delete from People");
            context.Database.ExecuteSqlCommand("delete from Branches");
        }

        public void RunBase()
        {
            HospitalDBContext context = new HospitalDBContext();
            Console.WriteLine("Creating Branches...");

            int streetStart = 1;
            string[] cities = new string[] { "Richmond", "Vancouver", "Coquitlam", "Burnaby", "Surrey", "Victoria" };
            string[] streets = new string[] { };
            string provinceStart = "BC";
            string postal = "A1B 2C3";
            string country = "CA";
            string basePhone = "604{0}{1}";
            string email = "bob.jones@gmail.com";

            Random r = new Random();

            ContactInformation b1Contact = new ContactInformation();
            b1Contact.Street = streetStart++ + " Main Street";
            b1Contact.City = cities[r.Next(0, cities.Length)];
            b1Contact.ZipPostalCode = postal;
            b1Contact.ProvinceState = provinceStart;
            b1Contact.Country = country;
            b1Contact.PhoneNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
            b1Contact.FaxNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
            b1Contact.CellPhoneNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
            b1Contact.Email = email;

            Branch b1 = new Branch();
            b1.BranchName = "Vancouver General Hospital";
            b1.ContactInformationId = b1Contact.ID;
            b1.ContactInformation = b1Contact;
            context.ContactInformations.Add(b1Contact);
            context.SaveChanges();

            context.Branches.Add(b1);
            context.SaveChanges();

            ContactInformation b2Contact = new ContactInformation();
            b2Contact.Street = streetStart++ + " Broadway Street";
            b2Contact.City = cities[(new Random()).Next(0, cities.Length - 1)];
            b2Contact.ZipPostalCode = postal;
            b2Contact.ProvinceState = provinceStart;
            b2Contact.Country = country;
            b2Contact.PhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
            b2Contact.FaxNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
            b2Contact.CellPhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
            b2Contact.Email = email;

            Branch b2 = new Branch();
            b2.BranchName = "Richmond Olds People Hospital";
            b2.ContactInformationId = b2Contact.ID;
            b2.ContactInformation = b2.ContactInformation;

            context.ContactInformations.Add(b2Contact);
            context.SaveChanges();

            context.Branches.Add(b2);
            context.SaveChanges();

            ContactInformation b3Contact = new ContactInformation();
            b3Contact.Street = streetStart++ + " Lakewood Street";
            b3Contact.City = cities[(new Random()).Next(0, cities.Length - 1)];
            b3Contact.ZipPostalCode = postal;
            b3Contact.ProvinceState = provinceStart;
            b3Contact.Country = country;
            b3Contact.PhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
            b3Contact.FaxNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
            b3Contact.CellPhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
            b3Contact.Email = email;

            Branch b3 = new Branch();
            b3.BranchName = "Burnaby Childrens Hospital";
            b3.ContactInformationId = b3Contact.ID;
            b3.ContactInformation = b3Contact;

            context.ContactInformations.Add(b3Contact);
            context.SaveChanges();

            context.Branches.Add(b3);
            context.SaveChanges();

            branches[0] = b1;
            branches[1] = b2;
            branches[2] = b3;

            Console.WriteLine("Creating Incidents...");
            //Create Incident Types
            IncidentType general = new IncidentType();
            general.ID = 1;
            general.Name = "General Incident";

            context.IncidentTypes.Add(general);
            context.SaveChanges();

            IncidentType complaint = new IncidentType();
            complaint.ID = 2;
            complaint.Name = "Complaint";

            context.IncidentTypes.Add(complaint);
            context.SaveChanges();

            IncidentType fall = new IncidentType();
            fall.ID = 3;
            fall.Name = "Fall";

            context.IncidentTypes.Add(fall);
            context.SaveChanges();

            types = new List<IncidentType>() { general, complaint, fall };
        }

        public void Run()
        {
            HospitalDBContext context = new HospitalDBContext();
            Random r = new Random();
            int streetStart = 1;
            string[] cities = new string[] { "Richmond", "Vancouver", "Coquitlam", "Burnaby", "Surrey", "Victoria" };
            string provinceStart = "BC";
            string postal = "A1B 2C3";
            string country = "CA";
            string basePhone = "604{0}{1}";
            string email = "bob.jones@gmail.com";

            Console.WriteLine("Data seeding process started...");

            string[] titles = new string[] { "Dr.", "Mr.", "Mrs.", "Ms.", "Sir" };
            string[] firstNames = new string[] { "Peter", "Lois", "Stuey", "Bob", "Nancy", "Sarah", "John", "David", "Amy", "Michelle", "Sean", "Brandon", "Renan" };
            string[] lastNames = new string[] { "Griffin", "Love", "Hope", "Lavergne", "Tran", "Aguiar", "Luo", "Smith", "Le", "Nguyen", "Cho", "Lenni" };
            string[] genders = new string[] { "M", "F" };
            string[] positions = { "Doctor", "Nurse", "Janitor", "Receptionist", "Intern", "EMT" };
            string[] departments = { "Pediatrics", "Emergency", "Cardiology", "Radiology", "ICU", "Marternity" };
            string[] streetBase = { " Broadway Street", " Lakewood Street", " Royal Oak Street", " Main Street", " 1st Ave", " 13th Ave" };
            int medicareCardNumber = 1000000000;
            List<Employee> employees = new List<Employee>();

            // Create Employees
            for (int i = 0; i < 20; i++)
            {
                ContactInformation homeContact = new ContactInformation();
                homeContact.Street = streetStart++ + streetBase[r.Next(0, streetBase.Length)];
                homeContact.City = cities[r.Next(0, cities.Length)];
                homeContact.ZipPostalCode = postal;
                homeContact.ProvinceState = provinceStart;
                homeContact.Country = country;
                homeContact.PhoneNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
                homeContact.FaxNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
                homeContact.CellPhoneNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
                homeContact.Email = email;

                context.ContactInformations.Add(homeContact);
                context.SaveChanges();

                NameDetails nameDetails = new NameDetails();
                nameDetails.Title = titles[r.Next(0, titles.Length)];
                nameDetails.FirstName = firstNames[r.Next(0, firstNames.Length)];
                nameDetails.LastName = lastNames[r.Next(0, lastNames.Length)];
                nameDetails.MiddleName = firstNames[r.Next(0, firstNames.Length)];
                nameDetails.MaidenName = lastNames[r.Next(0, lastNames.Length)];
                nameDetails.NickName = firstNames[r.Next(0, firstNames.Length)];

                context.NameDetails.Add(nameDetails);
                context.SaveChanges();

                Employee employee = new Employee();
                employee.DateOfBirth = randomDate(r);
                employee.Gender = genders[r.Next(0, 1)];
                employee.NameDetailsId = nameDetails.ID;
                employee.HomeContactInfoId = homeContact.ID;
                employee.WorkContactInfoId = branches[i % 3].ContactInformationId;
                employee.BranchId = branches[i % 3].ID;

                context.Employees.Add(employee);
                context.SaveChanges();

                EmploymentDetails details = new EmploymentDetails();
                details.EmployeeId = employee.ID;
                details.StartDate = randomDate(r);
                if (r.Next(0, 2) == 1)
                {
                    details.TerminationDate = DateTime.Today;
                }
                details.Position = positions[r.Next(0, positions.Length)];
                details.Department = departments[r.Next(0, departments.Length)];

                context.EmploymentDetails.Add(details);
                context.SaveChanges();

                employees.Add(employee);
            }

            string[] locations = {"Hallway", "Washroom", "Bedroom", "Front Desk", "Love Chambers"};
            
            // Create Patients
            for (int i = 0; i < 100; i++)
            {
                ContactInformation homeContact = new ContactInformation();
                homeContact.Street = streetStart++ + streetBase[r.Next(0, streetBase.Length)];
                homeContact.City = cities[r.Next(0, cities.Length)];
                homeContact.ZipPostalCode = postal;
                homeContact.ProvinceState = provinceStart;
                homeContact.Country = country;
                homeContact.PhoneNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
                homeContact.FaxNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
                homeContact.CellPhoneNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
                homeContact.Email = email;

                context.ContactInformations.Add(homeContact);
                context.SaveChanges();

                ContactInformation workContact = new ContactInformation();
                workContact.Street = streetStart++ + streetBase[r.Next(0, streetBase.Length)];
                workContact.City = cities[r.Next(0, cities.Length)];
                workContact.ZipPostalCode = postal;
                workContact.ProvinceState = provinceStart;
                workContact.Country = country;
                workContact.PhoneNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
                workContact.FaxNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
                workContact.CellPhoneNumber = string.Format(basePhone, r.Next(100, 1000), r.Next(1000, 10000));
                workContact.Email = email;

                context.ContactInformations.Add(workContact);
                context.SaveChanges();

                NameDetails nameDetails = new NameDetails();
                nameDetails.Title = titles[r.Next(0, titles.Length)];
                nameDetails.FirstName = firstNames[r.Next(0, firstNames.Length)];
                nameDetails.LastName = lastNames[r.Next(0, lastNames.Length)];
                nameDetails.MiddleName = firstNames[r.Next(0, firstNames.Length)];
                nameDetails.MaidenName = lastNames[r.Next(0, lastNames.Length)];
                nameDetails.NickName = firstNames[r.Next(0, firstNames.Length)];

                context.NameDetails.Add(nameDetails);
                context.SaveChanges();

                Patient patient = new Patient();
                patient.DateOfBirth = randomDate(r);
                patient.Gender = genders[r.Next(0, 2)];
                patient.NameDetailsId = nameDetails.ID;
                patient.HomeContactInfoId = homeContact.ID;
                patient.WorkContactInfoId = branches[i % 3].ContactInformationId;
                patient.BranchId = branches[i % 3].ID;
                patient.MedicareCardNumber = medicareCardNumber++;

                context.Patients.Add(patient);
                context.SaveChanges();

                CheckinDetails details = new CheckinDetails();
                details.PatientId = patient.ID;
                details.CheckinDate = randomDate(r);
                if (r.Next(0, 2) == 1)
                {
                    details.DischargeDate = DateTime.Today;
                }
                details.Diagnosis = "Dieing";
                details.IllnessDetails = "Cancer";

                context.CheckinDetails.Add(details);
                context.SaveChanges();

                List<Employee> myEmployees = employees.Where(e => e.BranchId == patient.BranchId).ToList();
                //Create incidents for Patient
                int noOfIncidents = r.Next(1, 6);
                int minIncId = context.IncidentTypes.First().ID;
                int maxIncId = minIncId + 3;
                for (int x = 0; x < noOfIncidents; x++)
                {
                    Incident incident = new Incident();
                    incident.BranchId = patient.BranchId;
                    incident.EmployeeId = myEmployees[r.Next(0, myEmployees.Count())].ID;
                    incident.Location = locations[r.Next(0, locations.Length)];
                    incident.PatientId = patient.ID;
                    incident.DateAndTime = randomDate(r);
                    incident.IncidentTypeId = r.Next(minIncId, maxIncId);
                    incident.Details = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                                       "sed do eiusmod tempor incididunt ut labore et dolore magna " +
                                       "aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco " +
                                       "laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor" +
                                       " in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla" +
                                       " pariatur. Excepteur sint occaecat cupidatat non proident, sunt in " +
                                       "culpa qui officia deserunt mollit anim id est laborum.";
                    incident.FollowUpActions = "Lorem ipsum dolor sit amet, consectetur adipiscing " +
                                               "elit, sed do eiusmod tempor incididunt ut labore et dolor" +
                                               "e magna aliqua. Ut enim ad minim veniam, quis nostrud exer" +
                                               "citation ullamco laboris nisi ut aliquip ex ea commodo co" +
                                               "nsequat. Duis aute irure dolor in reprehenderit in volup" +
                                               "tate velit esse cillum dolore eu fugiat nulla pariatur. Exce" +
                                               "pteur sint occaecat cupidatat non proident, sunt in culpa q" +
                                               "ui officia deserunt mollit anim id est laborum.";

                    context.Incidents.Add(incident);
                    context.SaveChanges();
                }
            }

            Console.WriteLine("Data seeding process completed.");
        }

        private DateTime randomDate(Random r)
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(r.Next(range));
        }
    }


}
