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
    /// A DevAVDbUnitOfWork instance that represents the run-time implementation of the IDevAVDbUnitOfWork interface.
    /// </summary>
    public class DevAVDbUnitOfWork : DbUnitOfWork<DevAVDb>, IDevAVDbUnitOfWork {

        public DevAVDbUnitOfWork(Func<DevAVDb> contextFactory)
            : base(contextFactory) {
        }

        IRepository<Customer, long> IDevAVDbUnitOfWork.Customers {
            get { return GetRepository(x => x.Set<Customer>(), x => x.Id); }
        }

        IRepository<Order, long> IDevAVDbUnitOfWork.Orders {
            get { return GetRepository(x => x.Set<Order>(), x => x.Id); }
        }

        IRepository<Employee, long> IDevAVDbUnitOfWork.Employees {
            get { return GetRepository(x => x.Set<Employee>(), x => x.Id); }
        }

        IRepository<EmployeeTask, long> IDevAVDbUnitOfWork.Tasks {
            get { return GetRepository(x => x.Set<EmployeeTask>(), x => x.Id); }
        }

        IRepository<Quote, long> IDevAVDbUnitOfWork.Quotes {
            get { return GetRepository(x => x.Set<Quote>(), x => x.Id); }
        }

        IRepository<Product, long> IDevAVDbUnitOfWork.Products {
            get { return GetRepository(x => x.Set<Product>(), x => x.Id); }
        }
    }
}
