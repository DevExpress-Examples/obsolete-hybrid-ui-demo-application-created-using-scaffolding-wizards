using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DevExpress.DevAV.Common.DataModel {
    /// <summary>
    /// A DbUnitOfWork class instance represents the implementation of the Unit Of Work pattern for design-time mode. 
    /// </summary>
    public class DesignTimeUnitOfWork : UnitOfWorkBase, IUnitOfWork {

        void IUnitOfWork.SaveChanges() { }

        bool IUnitOfWork.HasChanges() {
            return false;
        }

        protected IRepository<TEntity, TPrimaryKey>
            GetRepository<TEntity, TPrimaryKey>(Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression)
            where TEntity : class {
            return GetRepositoryCore<IRepository<TEntity, TPrimaryKey>, TEntity>(() => new DesignTimeRepository<TEntity, TPrimaryKey>(this, getPrimaryKeyExpression));
        }

        protected IReadOnlyRepository<TEntity>
            GetReadOnlyRepository<TEntity>()
            where TEntity : class {
            return GetRepositoryCore<IReadOnlyRepository<TEntity>, TEntity>(() => new DesignTimeReadOnlyRepository<TEntity>(this));
        }
    }
}