using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.ViewModel;
using DevExpress.DevAV.DevAVDbDataModel;
using DevExpress.DevAV.Common;
using DevExpress.DevAV;

namespace DevExpress.DevAV.ViewModels {

    /// <summary>
    /// Represents the single Employee object view model.
    /// </summary>
    public partial class EmployeeViewModel : SingleObjectViewModel<Employee, long, IDevAVDbUnitOfWork> {

        /// <summary>
        /// Creates a new instance of EmployeeViewModel as a POCO view model.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        public static EmployeeViewModel Create(IUnitOfWorkFactory<IDevAVDbUnitOfWork> unitOfWorkFactory = null) {
            return ViewModelSource.Create(() => new EmployeeViewModel(unitOfWorkFactory));
        }

        /// <summary>
        /// Initializes a new instance of the EmployeeViewModel class.
        /// This constructor is declared protected to avoid undesired instantiation of the EmployeeViewModel type without the POCO proxy factory.
        /// </summary>
        /// <param name="unitOfWorkFactory">A factory used to create a unit of work instance.</param>
        protected EmployeeViewModel(IUnitOfWorkFactory<IDevAVDbUnitOfWork> unitOfWorkFactory = null)
            : base(unitOfWorkFactory ?? UnitOfWorkSource.GetUnitOfWorkFactory(), x => x.Employees, x => x.FullName) {
        }


        /// <summary>
        /// The view model that contains a look-up collection of Tasks for the corresponding navigation property in the view.
        /// </summary>
        public IEntitiesViewModel<EmployeeTask> LookUpTasks
        {
            get
            {
                return GetLookUpEntitiesViewModel(
                    propertyExpression: (EmployeeViewModel x) => x.LookUpTasks,
                    getRepositoryFunc: x => x.Tasks);
            }
        }


        /// <summary>
        /// The view model for the EmployeeAssignedTasks detail collection.
        /// </summary>
        public CollectionViewModelBase<EmployeeTask, EmployeeTask, long, IDevAVDbUnitOfWork> EmployeeAssignedTasksDetails
        {
            get
            {
                return GetDetailsCollectionViewModel(
                    propertyExpression: (EmployeeViewModel x) => x.EmployeeAssignedTasksDetails,
                    getRepositoryFunc: x => x.Tasks,
                    foreignKeyExpression: x => x.AssignedEmployeeId,
                    navigationExpression: x => x.AssignedEmployee);
            }
        }
    }
}
