using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HealthcareAnalytics.Models
{
    public class TitleValidationAttribute : ValidationAttribute
    {
        public static ValidationResult IsValidTitle(string title)
        {
            if (title.Equals("Mr.") || title.Equals("Mrs.") || title.Equals("Ms.") || title.Equals("Dr.") ||
                title.Equals("Sir"))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Valid titles are: Mr., Mrs., Ms., Dr., and Sir");
            }
        }
    }

    public class NameInformation
    {
        public NameInformation()
        {
            ID = Guid.NewGuid();
        }

        public NameInformation(string title, string firstName, string lastName, string middleName, string nickName,
            string maidenName)
        {
            ID = Guid.NewGuid();
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            NickName = nickName;
            MaidenName = maidenName;
        }

        [Key]
        [Required]
        [ScaffoldColumn(false)]
        public Guid ID { get; private set; }

        [CustomValidation(typeof(TitleValidationAttribute), "IsValidTitle")]
        [Required(ErrorMessage = "A title is required")]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A first name is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "A last name is required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [DisplayName("Nickname")]
        public string NickName { get; set; }

        [DisplayName("Maiden Name")]
        public string MaidenName { get; set; }
    }

    public class ContactInformation
    {
        public ContactInformation()
        {
            ID = Guid.NewGuid();
        }

        public ContactInformation(string street, string city, string provinceState, string country, string postalZip,
            string phone, string cell, string fax, string email)
        {
            ID = Guid.NewGuid();
            Street = street;
            City = city;
            ProvinceState = provinceState;
            Country = country;
            ZipPostalCode = postalZip;
            PhoneNumber = phone;
            CellPhoneNumber = cell;
            FaxNumber = fax;
            Email = email;
        }


        [Key]
        [Required]
        [ScaffoldColumn(false)]
        public Guid ID { get; private set; }

        [Required(ErrorMessage = "A street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "A city is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "A state or province is required")]
        [DisplayName("Province/State")]
        public string ProvinceState { get; set; }

        [Required(ErrorMessage = "A country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "A zip or postal code is required")]
        [DisplayName("Postal/Zip Code")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Provided postal/zip code not valid")]
        public string ZipPostalCode { get; set; }

        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Provided phone number not valid")]
        public string PhoneNumber { get; set; }

        [DisplayName("Cell")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Provided cell phone number not valid")]
        public string CellPhoneNumber { get; set; }

        [DisplayName("Fax")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Provided fax number not valid")]
        public string FaxNumber { get; set; }

        [Required(ErrorMessage = "An email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Provided email number not valid")]
        public string Email { get; set; }
    }

    public class Person
    {
        public Person()
        {
            ID = Guid.NewGuid();
        }

        public Person(NameInformation nameInfo, DateTime dob, ContactInformation homeContact, ContactInformation workContact)
        {
            ID = Guid.NewGuid();
            Name = nameInfo;
            DateOfBirth = dob;
            HomeContactInfo = homeContact;
            WorkContactInfo = workContact;
            HomeContactInfoId = homeContact.ID;
            WorkContactInfoId = workContact.ID;
        }

        [Key]
        [Required]
        public Guid ID { get; private set; }

        [Required]
        public  DateTime DateOfBirth { get; set; }

        [Required]
        [ForeignKey("NameId")]
        public NameInformation Name { get; set; }
        public Guid NameId { get; set; }

        [Required]
        [ForeignKey("HomeContactInfoId")]
        public ContactInformation HomeContactInfo { get; set; }
        public Guid HomeContactInfoId { get; set; }

        [Required]
        [ForeignKey("WorkContactInfoId")]
        public ContactInformation WorkContactInfo { get; set; }
        public Guid WorkContactInfoId { get; set; }

    }
}