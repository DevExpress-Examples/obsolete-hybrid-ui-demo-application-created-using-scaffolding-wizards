using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DevExpress.DevAV {
    public class DevAVDb : DbContext {
        public DevAVDb() {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<EmployeeTask> Tasks { get; set; }
        public DbSet<CustomerStore> CustomerStores { get; set; }
    }
}
