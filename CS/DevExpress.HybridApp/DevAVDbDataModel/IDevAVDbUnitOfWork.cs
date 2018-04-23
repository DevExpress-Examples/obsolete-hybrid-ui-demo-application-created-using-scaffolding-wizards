using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Collections.Generic;
using DevExpress.DevAV.Common.Utils;
using DevExpress.DevAV.Common.DataModel;
using DevExpress.DevAV.Common.DataModel.EntityFramework;
using DevExpress.DevAV;

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
