using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DevExpress.DataAnnotations;

namespace DevExpress.DevAV {
    public partial class Customer : DatabaseObject {
        public Customer() {
            Orders = new List<Order>();
        }
        [Required]
        public string Name { get; set; }

        public StateEnum AddressState { get; set; }
        public string AddressLine { get; set; }
        public string AddressCity { get; set; }
        public string AddressZipCode { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Phone]
        public string Fax { get; set; }

        [Url]
        public string Website { get; set; }

        [DataType(DataType.Currency)]
        public decimal AnnualRevenue { get; set; }

        public int TotalStores { get; set; }

        public int TotalEmployees { get; set; }

        public CustomerStatus Status { get; set; }

        public virtual List<Order> Orders { get; set; }

        public virtual List<Quote> Quotes { get; set; }

        public virtual List<CustomerStore> CustomerStores { get; set; }

        public byte[] Logo { get; set; }
    }

    public enum CustomerStatus {
        Active,
        Suspended
    }
}
