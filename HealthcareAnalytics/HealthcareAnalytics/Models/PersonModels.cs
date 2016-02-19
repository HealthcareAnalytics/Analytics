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

    public class GenderValidation : ValidationAttribute
    {
        public static ValidationResult IsValidGender(string gender)
        {
            if (gender.Equals("M") || gender.Equals("F"))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Valid gender values are M or F");
            }
        }
    }

    public class NameDetails
    {
        public NameDetails()
        {
            ID = Guid.NewGuid();
        }

        [Key]
        [Required]
        [ScaffoldColumn(false)]
        public Guid ID { get; set; }

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

        [Key]
        [Required]
        [ScaffoldColumn(false)]
        public Guid ID { get; set; }

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

    public abstract class Person
    {
        public Person()
        {
            ID = Guid.NewGuid();
        }

        [Key]
        [Required]
        public Guid ID { get; set; }

        [Required]
        public  DateTime DateOfBirth { get; set; }

        [Required]
        [CustomValidation(typeof(TitleValidationAttribute), "IsValidTitle")]
        [StringLength(1)]
        public string Gender { get; set; }

        [Required]
        [ForeignKey("NameDetailsId")]
        public NameDetails NameDetails { get; set; }
        public Guid NameDetailsId { get; set; }

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