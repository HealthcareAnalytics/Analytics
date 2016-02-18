using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HealthcareAnalytics.Models
{
    public class Patient : Person
    {
        public Patient() : base()
        {
        }

        public Patient(NameInformation nameInfo, ContactInformation homeContact, ContactInformation workContact, Branch branch) 
            : base(nameInfo, homeContact, workContact)
        {
            Branch = branch;
            BranchId = branch.ID;
        }

        public Guid? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }
    }

    public class Employee : Person
    {
        public Employee() : base()
        {
        }

        public Employee(NameInformation nameInfo, ContactInformation homeContact, ContactInformation workContact,
            string department, Branch branch) : base(nameInfo, homeContact, workContact)
        {
            Department = department;
            Branch = branch;
            BranchId = branch.ID;
        }

        [Required]
        public string Department { get; set; }
        [Key]
        [Required]
        public Guid ID { get; private set; }
        
        public Guid? BranchId { get; set; }
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

        public Guid? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

    }

    public class Branch
    {
        [Key]
        [Required]
        public Guid ID { get; private set; }

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