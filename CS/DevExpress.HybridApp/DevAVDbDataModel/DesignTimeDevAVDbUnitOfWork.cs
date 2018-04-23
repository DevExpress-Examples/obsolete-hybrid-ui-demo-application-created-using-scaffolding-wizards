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
    /// A DevAVDbDesignTimeUnitOfWork instance that represents the design-time implementation of the IDevAVDbUnitOfWork interface.
    /// </summary>
    public class DevAVDbDesignTimeUnitOfWork : DesignTimeUnitOfWork, IDevAVDbUnitOfWork {

        /// <summary>
        /// Initializes a new instance of the DevAVDbDesignTimeUnitOfWork class.
        /// </summary>
        public DevAVDbDesignTimeUnitOfWork() {
        }

        IRepository<Customer, long> IDevAVDbUnitOfWork.Customers {
            get { return GetRepository((Customer x) => x.Id); }
        }

        IRepository<Order, long> IDevAVDbUnitOfWork.Orders {
            get { return GetRepository((Order x) => x.Id); }
        }

        IRepository<Employee, long> IDevAVDbUnitOfWork.Employees {
            get { return GetRepository((Employee x) => x.Id); }
        }

        IRepository<EmployeeTask, long> IDevAVDbUnitOfWork.Tasks {
            get { return GetRepository((EmployeeTask x) => x.Id); }
        }

        IRepository<Quote, long> IDevAVDbUnitOfWork.Quotes {
            get { return GetRepository((Quote x) => x.Id); }
        }

        IRepository<Product, long> IDevAVDbUnitOfWork.Products {
            get { return GetRepository((Product x) => x.Id); }
        }
    }
}
