using DevExpress.Mvvm;
using DevExpress.Mvvm.DataModel;
using DevExpress.DevAV.ViewModels;

namespace DevExpress.DevAV.Common {
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