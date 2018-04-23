using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpress.DevAV {
    public static class QueriesHelper {
        public static IQueryable<CustomerInfo> GetCustomerInfo(IQueryable<Customer> customers) {
            return customers.Select(x => new CustomerInfo {
                Id = x.Id,
                Name = x.Name,
                AddressLine = x.AddressLine,
                AddressCity = x.AddressCity,
                AddressState = x.AddressState,
                AddressZipCode = x.AddressZipCode,
                Phone = x.Phone,
                Fax = x.Fax,
                TotalSales = x.Orders.Sum(orderItem => orderItem.TotalAmount),
                MonthlySales = x.Orders.GroupBy(o => o.OrderDate.Month).Select(g => g.Sum(i => i.TotalAmount)),
            });
        }
        public static void UpdateCustomerInfoStores(IEnumerable<CustomerInfo> entities, IQueryable<Customer> customers) {
            foreach(var item in entities) {
                item.SetDeferredStores(() => customers.First(x => x.Id == item.Id).CustomerStores.ToArray());
            }
        }

    }
}
