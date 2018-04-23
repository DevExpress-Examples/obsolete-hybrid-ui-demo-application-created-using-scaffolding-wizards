#region #Lesson8
using DevExpress.DevAV.Common.ViewModel;
using DevExpress.DevAV.DevAVDbDataModel;
using DevExpress.HybridApp.Properties;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System;

namespace DevExpress.DevAV.ViewModels {
    internal static class FiltersSettings {
        public static FilterTreeViewModel<EmployeeTask, long> GetTasksFilterTree(object parentViewModel) {
            return FilterTreeViewModel<EmployeeTask, long>.Create(
                new FilterTreeModelPageSpecificSettings<Settings>(Settings.Default, null, x => x.TasksStaticFilters, null, null),
                CreateUnitOfWork().Tasks, (recipient, handler) => RegisterEntityChangedMessageHandler<EmployeeTask, long>(recipient, handler)
            ).SetParentViewModel(parentViewModel);
        }
        public static FilterTreeViewModel<Employee, long> GetEmployeesFilterTree(object parentViewModel) {
            return FilterTreeViewModel<Employee, long>.Create(
                new FilterTreeModelPageSpecificSettings<Settings>(Settings.Default, "Status", x => x.EmployeesStaticFilters, null, null),
                CreateUnitOfWork().Employees, (recipient, handler) => RegisterEntityChangedMessageHandler<Employee, long>(recipient, handler)
            ).SetParentViewModel(parentViewModel);
        }
        public static FilterTreeViewModel<Product, long> GetProductsFilterTree(object parentViewModel) {
            return FilterTreeViewModel<Product, long>.Create(
                new FilterTreeModelPageSpecificSettings<Settings>(Settings.Default, "Category", x => x.ProductsStaticFilters, x => x.ProductsCustomFilters, null,
                    new[] {
                        BindableBase.GetPropertyName(() => new Product().Id),
                        BindableBase.GetPropertyName(() => new Product().EngineerId),
                        BindableBase.GetPropertyName(() => new Product().SupportId),
                        BindableBase.GetPropertyName(() => new Product().Support),
                    }),
                CreateUnitOfWork().Products, (recipient, handler) => RegisterEntityChangedMessageHandler<Product, long>(recipient, handler)
            ).SetParentViewModel(parentViewModel);
        }
        public static FilterTreeViewModel<Customer, long> GetCustomersFilterTree(object parentViewModel) {
            return FilterTreeViewModel<Customer, long>.Create(
                new FilterTreeModelPageSpecificSettings<Settings>(Settings.Default, "Favorites", null, x => x.CustomersCustomFilters, null,
                    new[] {
                        BindableBase.GetPropertyName(() => new Customer().Id),
                    }),
                CreateUnitOfWork().Customers, (recipient, handler) => RegisterEntityChangedMessageHandler<Customer, long>(recipient, handler)
            ).SetParentViewModel(parentViewModel);
        }
        public static FilterTreeViewModel<Order, long> GetSalesFilterTree(object parentViewModel) {
            return FilterTreeViewModel<Order, long>.Create(
                new FilterTreeModelPageSpecificSettings<Settings>(Settings.Default, null, null, null, null),
                CreateUnitOfWork().Orders, (recipient, handler) => RegisterEntityChangedMessageHandler<Order, long>(recipient, handler)
            ).SetParentViewModel(parentViewModel);
        }
        public static FilterTreeViewModel<Quote, long> GetOpportunitiesFilterTree(object parentViewModel) {
            return FilterTreeViewModel<Quote, long>.Create(
                new FilterTreeModelPageSpecificSettings<Settings>(Settings.Default, null, null, null, null),
                CreateUnitOfWork().Quotes, (recipient, handler) => RegisterEntityChangedMessageHandler<Quote, long>(recipient, handler)
            ).SetParentViewModel(parentViewModel);
        }

        static IDevAVDbUnitOfWork CreateUnitOfWork() {
            return UnitOfWorkSource.GetUnitOfWorkFactory().CreateUnitOfWork();
        }

        static void RegisterEntityChangedMessageHandler<TEntity, TPrimaryKey>(object recipient, Action handler) {
            Messenger.Default.Register<EntityMessage<TEntity, TPrimaryKey>>(recipient, message => handler());
        }
    }
}
#endregion #Lesson8