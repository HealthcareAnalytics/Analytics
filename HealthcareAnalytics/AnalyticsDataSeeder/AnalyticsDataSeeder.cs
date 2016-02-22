using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthcareAnalytics.Models;

namespace AnalyticsDataSeeder
{
    class AnalyticsDataSeeder
    {
        static void Main(string[] args)
        {
            new AnalyticsDataSeeder().Run();
        }

        public void Run()
        {
            Console.WriteLine("Data seeding process started...");

            HospitalDBContext context = new HospitalDBContext();

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
            b1Contact.City = cities[(new Random()).Next(0, cities.Length - 1)];
            b1Contact.ZipPostalCode = postal;
            b1Contact.ProvinceState = provinceStart;
            b1Contact.Country = country;
            b1Contact.PhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
            b1Contact.FaxNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
            b1Contact.CellPhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
            b1Contact.Email = email;

            Branch b1 = new Branch();
            b1.BranchName = "Vancouver General Hospital";
            b1.ContactInformationId = b1Contact.ID;
            b1.ContactInformation = b1Contact;
            Console.WriteLine("test");

            context.ContactInformations.Add(b1Contact);
            context.SaveChanges();
            Console.WriteLine("test");

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


            string[] titles = new string[] { "Dr.", "Mr.", "Mrs.", "Ms.", "Sir" };
            string[] firstNames = new string[] { "Peter", "Lois", "Stuey", "Bob", "Nancy", "Sarah", "John", "David", "Amy", "Michelle", "Sean", "Brandon", "Renan" };
            string[] lastNames = new string[] { "Griffin", "Love", "Hope", "Lavergne", "Tran", "Aguiar", "Luo", "Smith", "Le", "Nguyen", "Cho", "Lenni" };
            string[] genders = new string[] { "M", "F" };
            Branch[] branches = { b1, b2, b3 };
            string[] positions = { "Doctor", "Nurse", "Janitor", "Receptionist", "Intern", "EMT" };
            string[] departments = { "Pediatrics", "Emergency", "Cardiology", "Radiology", "ICU", "Marternity" };
            string[] streetBase = { " Broadway Street", " Lakewood Street", " Royal Oak Street", " Main Street", " 1st Ave", " 13th Ave" };
            int medicareCardNumber = 1000000000;

            // Create Employees
            for (int i = 0; i < 300; i++)
            {
                Console.WriteLine(i);

                ContactInformation homeContact = new ContactInformation();
                homeContact.Street = streetStart++ + streetBase[r.Next(0, streetBase.Length - 1)];
                homeContact.City = cities[(new Random()).Next(0, cities.Length - 1)];
                homeContact.ZipPostalCode = postal;
                homeContact.ProvinceState = provinceStart;
                homeContact.Country = country;
                homeContact.PhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
                homeContact.FaxNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
                homeContact.CellPhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
                homeContact.Email = email;

                context.ContactInformations.Add(homeContact);
                context.SaveChanges();

                NameDetails nameDetails = new NameDetails();
                nameDetails.Title = titles[r.Next(0, titles.Length)];
                nameDetails.FirstName = firstNames[r.Next(0, firstNames.Length - 1)];
                nameDetails.LastName = lastNames[r.Next(0, lastNames.Length - 1)];
                nameDetails.MiddleName = firstNames[r.Next(0, firstNames.Length - 1)];
                nameDetails.MaidenName = lastNames[r.Next(0, lastNames.Length - 1)];
                nameDetails.NickName = firstNames[r.Next(0, firstNames.Length - 1)];

                context.NameDetails.Add(nameDetails);
                context.SaveChanges();

                Employee employee = new Employee();
                employee.DateOfBirth = DateTime.Today;
                employee.Gender = genders[r.Next(0, 1)];
                employee.NameDetailsId = nameDetails.ID;
                employee.HomeContactInfoId = homeContact.ID;
                employee.WorkContactInfoId = branches[i % 3].ContactInformationId;
                employee.BranchId = branches[i % 3].ID;

                context.Employees.Add(employee);
                context.SaveChanges();

                EmploymentDetails details = new EmploymentDetails();
                details.EmployeeId = employee.ID;
                details.StartDate = DateTime.Today;
                details.TerminationDate = DateTime.Today;
                details.Position = positions[r.Next(0, positions.Length - 1)];
                details.Department = departments[r.Next(0, departments.Length - 1)];

                context.EmployementDetails.Add(details);
                context.SaveChanges();
            }

            // Create Patients
            for (int i = 0; i < 10000; i++)
            {
                ContactInformation homeContact = new ContactInformation();
                homeContact.Street = streetStart++ + streetBase[r.Next(0, streetBase.Length - 1)];
                homeContact.City = cities[(new Random()).Next(0, cities.Length - 1)];
                homeContact.ZipPostalCode = postal;
                homeContact.ProvinceState = provinceStart;
                homeContact.Country = country;
                homeContact.PhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
                homeContact.FaxNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
                homeContact.CellPhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
                homeContact.Email = email;

                context.ContactInformations.Add(homeContact);
                context.SaveChanges();

                ContactInformation workContact = new ContactInformation();
                workContact.Street = streetStart++ + streetBase[r.Next(0, streetBase.Length - 1)];
                workContact.City = cities[(new Random()).Next(0, cities.Length - 1)];
                workContact.ZipPostalCode = postal;
                workContact.ProvinceState = provinceStart;
                workContact.Country = country;
                workContact.PhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
                workContact.FaxNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
                workContact.CellPhoneNumber = string.Format(basePhone, r.Next(100, 999), r.Next(1000, 9999));
                workContact.Email = email;

                context.ContactInformations.Add(workContact);
                context.SaveChanges();

                NameDetails nameDetails = new NameDetails();
                nameDetails.Title = titles[r.Next(0, titles.Length)];
                nameDetails.FirstName = firstNames[r.Next(0, firstNames.Length - 1)];
                nameDetails.LastName = lastNames[r.Next(0, lastNames.Length - 1)];
                nameDetails.MiddleName = firstNames[r.Next(0, firstNames.Length - 1)];
                nameDetails.MaidenName = lastNames[r.Next(0, lastNames.Length - 1)];
                nameDetails.NickName = firstNames[r.Next(0, firstNames.Length - 1)];

                context.NameDetails.Add(nameDetails);
                context.SaveChanges();

                Patient patient = new Patient();
                patient.DateOfBirth = DateTime.Today;
                patient.Gender = genders[r.Next(0, 1)];
                patient.NameDetailsId = nameDetails.ID;
                patient.HomeContactInfoId = homeContact.ID;
                patient.WorkContactInfoId = branches[i % 3].ContactInformationId;
                patient.BranchId = branches[i % 3].ID;
                patient.MedicareCardNumber = medicareCardNumber++;

                context.Patients.Add(patient);
                context.SaveChanges();

                CheckinDetails details = new CheckinDetails();
                details.PatientId = patient.ID;
                details.CheckinDate = DateTime.Today;
                details.DischargeDate = DateTime.Today;
                details.Diagnosis = "Dieing";
                details.IllnessDetails = "Cancer";

                context.CheckinDetails.Add(details);
                context.SaveChanges();
            }

            Console.WriteLine("Data seeding process completed.");
        }
    }
}
