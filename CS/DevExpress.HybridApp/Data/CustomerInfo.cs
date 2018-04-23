using System;
using System.Collections.Generic;
using System.Linq;

namespace DevExpress.DevAV {
    public class CustomerInfo {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string AddressCity { get; set; }
        public StateEnum AddressState { get; set; }
        public string AddressZipCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public decimal? TotalSales { get; set; }
        public IEnumerable<decimal> MonthlySales { get; set; }

        Lazy<IEnumerable<CustomerStore>> customerStores;
        public IEnumerable<CustomerStore> CustomerStores { get { return customerStores.Value; } }
        public void SetDeferredStores(Func<IEnumerable<CustomerStore>> getStores) {
            this.customerStores = new Lazy<IEnumerable<CustomerStore>>(getStores);
        }
    }
}
