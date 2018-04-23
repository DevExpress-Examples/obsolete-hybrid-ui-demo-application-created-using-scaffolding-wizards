using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;

namespace DevExpress.DevAV.Common.DataModel {
    /// <summary>
    /// DesignTimeReadOnlyRepository is an IReadOnlyRepository interface implementation that represents the collection of entities of a given type for design-time mode. 
    /// DesignTimeReadOnlyRepository objects are created from a DesignTimeInitOfWork class instance using the GetReadOnlyRepository method. 
    /// DesignTimeReadOnlyRepository provides only read-only operations against entities of a given type.
    /// </summary>
    /// <typeparam name="TEntity">A repository entity type.</typeparam>
    public class DesignTimeReadOnlyRepository<TEntity> : DesignTimeRepositoryQuery<TEntity>, IReadOnlyRepository<TEntity>
        where TEntity : class {

        static IQueryable<TEntity> CreateSampleQueryable() {
            return DesignTimeHelper.CreateDesignTimeObjects<TEntity>(2).AsQueryable();
        }

        readonly DesignTimeUnitOfWork unitOfWork;

        public DesignTimeReadOnlyRepository(DesignTimeUnitOfWork unitOfWork)
            : base(CreateSampleQueryable()) {
            this.unitOfWork = unitOfWork;
        }

        #region IReadOnlyRepository
        IUnitOfWork IReadOnlyRepository<TEntity>.UnitOfWork {
            get { return unitOfWork; }
        }
        #endregion
    }
    public class DesignTimeRepositoryQuery<TEntity> : RepositoryQueryBase<TEntity>, IRepositoryQuery<TEntity> {
        public DesignTimeRepositoryQuery(IQueryable<TEntity> queryable)
            : base(() => queryable) { }
        IRepositoryQuery<TEntity> IRepositoryQuery<TEntity>.Include<TProperty>(Expression<Func<TEntity, TProperty>> path) {
            return this;
        }
        IRepositoryQuery<TEntity> IRepositoryQuery<TEntity>.Where(Expression<Func<TEntity, bool>> predicate) {
            return this;
        }
    }
}