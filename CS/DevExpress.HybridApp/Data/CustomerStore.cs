using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevExpress.DevAV {
    [ScaffoldTable(false)]
    public class CustomerStore : DatabaseObject {
        public virtual Customer Customer { get; set; }

        public long? CustomerId { get; set; }

        public StateEnum AddressState { get; set; }
        public string AddressLine { get; set; }
        public string AddressCity { get; set; }
        public string AddressZipCode { get; set; }

        public StateEnum State { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public int TotalEmployees { get; set; }

        public int SquereFootage { get; set; }

        [DataType(DataType.Currency)]
        public decimal AnnualSales { get; set; }

        public byte[] CrestLarge { get; set; }
        public byte[] CrestSmall { get; set; }

        public string Location { get; set; }

        public string AddressCityLine {
            get { return Employee.GetCityLine(AddressCity, AddressState, AddressZipCode); }
        }
    }
}
