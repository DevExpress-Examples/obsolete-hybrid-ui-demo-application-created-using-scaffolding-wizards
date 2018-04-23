using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DevExpress.DataAnnotations;

namespace DevExpress.DevAV {
    public class Employee : DatabaseObject {
        public Employee() {
            AssignedTasks = new List<EmployeeTask>();
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public PersonPrefix Prefix { get; set; }

        [Phone]
        public string HomePhone { get; set; }

        [Required, Phone]
        public string MobilePhone { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Skype { get; set; }

        public DateTime? BirthDate { get; set; }

        public byte[] Picture { get; set; }

        public StateEnum AddressState { get; set; }
        public string AddressLine { get; set; }
        public string AddressCity { get; set; }
        public string AddressZipCode { get; set; }

        public EmployeeDepartment Department { get; set; }

        [Required]
        public string Title { get; set; }

        public EmployeeStatus Status { get; set; }

        public DateTime? HireDate { get; set; }

        public virtual List<EmployeeTask> AssignedTasks { get; set; }

        public string PersonalProfile { get; set; }

        public override string ToString() {
            return FirstName + " " + LastName;
        }
        public virtual int Order { get; set; }

        public string FullName {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public string AddressCityLine {
            get { return GetCityLine(AddressCity, AddressState, AddressZipCode); }
        }
        internal static string GetCityLine(string city, StateEnum state, string zipCode) {
            return string.Format("{0}, {1} {2}", city, state, zipCode);
        }
    }

    public enum PersonPrefix {
        Dr,
        Mr,
        Ms,
        Miss,
        Mrs,
    }

    public enum EmployeeStatus {
        Salaried,
        Commission,
        Contract,
        Terminated,
        OnLeave
    }

    public enum EmployeeDepartment {
        [Display(Name = "Sales")]
        Sales = 1,
        [Display(Name = "Support")]
        Support,
        [Display(Name = "Shipping")]
        Shipping,
        [Display(Name = "Engineering")]
        Engineering,
        [Display(Name = "Human Resources")]
        HumanResources,
        [Display(Name = "Management")]
        Management,
        [Display(Name = "IT")]
        IT
    }
}
