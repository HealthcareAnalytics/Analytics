using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HealthcareAnalytics.Models;

namespace HealthcareAnalytics.Models
{
    public class EmploymentDateSet
    {
        public EmploymentDateSet()
        {
        }

        public EmploymentDateSet(Employee emp, DateTime start, DateTime termination)
        {
            Employee = emp;
            EmployeeId = emp.ID;
            StartDate = start;
            TerminationDate = termination;
        }

        [Key]
        public Guid EmployeeId { get; private set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; private set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime TerminationDate { get; set; }
    }

    public class PatientCheckinDischargeDateSet
    {
        public PatientCheckinDischargeDateSet()
        {
        }

        public PatientCheckinDischargeDateSet(Patient patient, DateTime checkin, DateTime discharge)
        {
            Patient = patient;
            PatientId = patient.ID;
            CheckinDate = checkin;
            DischargeDate = discharge;
        }

        [Key]
        [Required]
        public Guid PatientId { get; private set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; private set; }

        [Required]
        public DateTime CheckinDate { get; set; }
        public DateTime DischargeDate { get; set; }
    }


    public class Patient : Person
    {
        public Patient() : base()
        {
        }

        public Patient(int medicareCardNumber, PatientCheckinDischargeDateSet dateSet, NameInformation nameInfo, DateTime dob, 
            ContactInformation homeContact, ContactInformation workContact, Branch branch) 
            : base(nameInfo, dob, homeContact, workContact)
        {
            Branch = branch;
            BranchId = branch.ID;
            MedicareCardNumber = medicareCardNumber;
            CheckinDischargeDates = new PatientCheckinDischargeDateSet[1];
            CheckinDischargeDates.Add(dateSet);
        }

        public int MedicareCardNumber { get; set; }

        public ICollection<PatientCheckinDischargeDateSet> CheckinDischargeDates { get; set; }

        public Guid BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }
    }

    public class Employee : Person
    {
        public Employee() : base()
        {
        }

        public Employee(string department, string position, EmploymentDateSet dateSet, 
            NameInformation nameInfo, DateTime dob, ContactInformation homeContact, ContactInformation workContact,
            Branch branch) : base(nameInfo, dob, homeContact, workContact)
        {
            Position = position;
            Department = department;
            Branch = branch;
            BranchId = branch.ID;
            EmploymentDates = new EmploymentDateSet[1];
            EmploymentDates.Add(dateSet);
        }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Department { get; set; }

        public ICollection<EmploymentDateSet> EmploymentDates { get; set; } 

        public Guid BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }
    }

    // TODO: Incident model is incomplete
    public class Incident
    {
        public Incident(Branch branch)
        {
            ID = Guid.NewGuid();
            Branch = branch;
            BranchId = branch.ID;
        }

        [Key]
        [Required]
        public Guid ID { get; private set; }

        [Required]
        public Guid BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

    }

    public class Branch
    {
        [Key]
        [Required]
        public Guid ID { get; private set; }
        public string BranchName { get; set; }

        public ICollection<Employee> Employees { get; set; }
        public ICollection<Patient> Patients { get; set; }
        public ICollection<Incident> Incidents { get; set; }

    }


    public class HosptialDBContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public HosptialDBContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Person>().ToTable("Patients");

            modelBuilder.Entity<Person>().HasRequired(m => m.HomeContactInfo).WithMany().HasForeignKey(m => m.HomeContactInfoId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Person>().HasRequired(m => m.WorkContactInfo).WithMany().HasForeignKey(m => m.WorkContactInfoId)
                .WillCascadeOnDelete(false);
        }
    }
}