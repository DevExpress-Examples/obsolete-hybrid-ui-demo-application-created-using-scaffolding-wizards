using DevExpress.DevAV;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.DataModel.DesignTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevExpress.DevAV.DevAVDbDataModel {

    /// <summary>
    /// A DevAVDbDesignTimeUnitOfWork instance that represents the design-time implementation of the IDevAVDbUnitOfWork interface.
    /// </summary>
    public class DevAVDbDesignTimeUnitOfWork : DesignTimeUnitOfWork, IDevAVDbUnitOfWork {

        /// <summary>
        /// Initializes a new instance of the DevAVDbDesignTimeUnitOfWork class.
        /// </summary>
        public DevAVDbDesignTimeUnitOfWork() {
        }

        IRepository<Customer, long> IDevAVDbUnitOfWork.Customers
        {
            get { return GetRepository((Customer x) => x.Id); }
        }

        IRepository<CustomerStore, long> IDevAVDbUnitOfWork.CustomerStores
        {
            get { return GetRepository((CustomerStore x) => x.Id); }
        }

        IRepository<Order, long> IDevAVDbUnitOfWork.Orders
        {
            get { return GetRepository((Order x) => x.Id); }
        }

        IRepository<Employee, long> IDevAVDbUnitOfWork.Employees
        {
            get { return GetRepository((Employee x) => x.Id); }
        }

        IRepository<EmployeeTask, long> IDevAVDbUnitOfWork.Tasks
        {
            get { return GetRepository((EmployeeTask x) => x.Id); }
        }

        IRepository<Quote, long> IDevAVDbUnitOfWork.Quotes
        {
            get { return GetRepository((Quote x) => x.Id); }
        }

        IRepository<Product, long> IDevAVDbUnitOfWork.Products
        {
            get { return GetRepository((Product x) => x.Id); }
        }
    }
}
