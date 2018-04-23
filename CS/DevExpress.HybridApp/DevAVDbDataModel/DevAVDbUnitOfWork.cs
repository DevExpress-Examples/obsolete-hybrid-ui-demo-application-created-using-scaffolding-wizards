using DevExpress.DevAV;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.DataModel.EF6;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevExpress.DevAV.DevAVDbDataModel {

    /// <summary>
    /// A DevAVDbUnitOfWork instance that represents the run-time implementation of the IDevAVDbUnitOfWork interface.
    /// </summary>
    public class DevAVDbUnitOfWork : DbUnitOfWork<DevAVDb>, IDevAVDbUnitOfWork {

        public DevAVDbUnitOfWork(Func<DevAVDb> contextFactory)
            : base(contextFactory) {
        }

        IRepository<Customer, long> IDevAVDbUnitOfWork.Customers
        {
            get { return GetRepository(x => x.Set<Customer>(), (Customer x) => x.Id); }
        }

        IRepository<CustomerStore, long> IDevAVDbUnitOfWork.CustomerStores
        {
            get { return GetRepository(x => x.Set<CustomerStore>(), (CustomerStore x) => x.Id); }
        }

        IRepository<Order, long> IDevAVDbUnitOfWork.Orders
        {
            get { return GetRepository(x => x.Set<Order>(), (Order x) => x.Id); }
        }

        IRepository<Employee, long> IDevAVDbUnitOfWork.Employees
        {
            get { return GetRepository(x => x.Set<Employee>(), (Employee x) => x.Id); }
        }

        IRepository<EmployeeTask, long> IDevAVDbUnitOfWork.Tasks
        {
            get { return GetRepository(x => x.Set<EmployeeTask>(), (EmployeeTask x) => x.Id); }
        }

        IRepository<Quote, long> IDevAVDbUnitOfWork.Quotes
        {
            get { return GetRepository(x => x.Set<Quote>(), (Quote x) => x.Id); }
        }

        IRepository<Product, long> IDevAVDbUnitOfWork.Products
        {
            get { return GetRepository(x => x.Set<Product>(), (Product x) => x.Id); }
        }
    }
}
