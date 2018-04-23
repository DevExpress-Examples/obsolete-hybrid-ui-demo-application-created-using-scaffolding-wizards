using DevExpress.DevAV.DevAVDbDataModel;
using DevExpress.DevAV;
using System.Collections.Generic;

namespace DevExpress.DevAV.ViewModels {
    partial class CustomerCollectionViewModel {
        protected override void OnEntitiesLoaded(IDevAVDbUnitOfWork unitOfWork, IEnumerable<CustomerInfo> entities) {
            base.OnEntitiesLoaded(unitOfWork, entities);
            QueriesHelper.UpdateCustomerInfoStores(entities, unitOfWork.Customers);
        }
    }
}