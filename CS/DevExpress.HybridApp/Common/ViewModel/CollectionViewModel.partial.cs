using DevExpress.DevAV.Common.DataModel;
using DevExpress.DevAV.ViewModels;
using DevExpress.Mvvm;

namespace DevExpress.DevAV.Common.ViewModel {
    partial class CollectionViewModel<TEntity, TProjection, TPrimaryKey, TUnitOfWork> : ISupportFiltering<TEntity>, IFilterTreeViewModelContainer<TEntity, TPrimaryKey>
        where TEntity : class
        where TProjection : class
        where TUnitOfWork : IUnitOfWork {
        public virtual FilterTreeViewModel<TEntity, TPrimaryKey> FilterTreeViewModel { get; set; }
        public void CreateCustomFilter() {
            Messenger.Default.Send(new CreateCustomFilterMessage<TEntity>());
        }
    }
}