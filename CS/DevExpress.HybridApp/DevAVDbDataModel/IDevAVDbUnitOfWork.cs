using DevExpress.DevAV;
using DevExpress.Mvvm.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevExpress.DevAV.DevAVDbDataModel {

    /// <summary>
    /// IDevAVDbUnitOfWork extends the IUnitOfWork interface with repositories representing specific entities.
    /// </summary>
    public interface IDevAVDbUnitOfWork : IUnitOfWork {

        /// <summary>
        /// The Customer entities repository.
        /// </summary>
        IRepository<Customer, long> Customers { get; }

        /// <summary>
        /// The CustomerStore entities repository.
        /// </summary>
        IRepository<CustomerStore, long> CustomerStores { get; }

        /// <summary>
        /// The Order entities repository.
        /// </summary>
        IRepository<Order, long> Orders { get; }

        /// <summary>
        /// The Employee entities repository.
        /// </summary>
        IRepository<Employee, long> Employees { get; }

        /// <summary>
        /// The EmployeeTask entities repository.
        /// </summary>
        IRepository<EmployeeTask, long> Tasks { get; }

        /// <summary>
        /// The Quote entities repository.
        /// </summary>
        IRepository<Quote, long> Quotes { get; }

        /// <summary>
        /// The Product entities repository.
        /// </summary>
        IRepository<Product, long> Products { get; }
    }
}
