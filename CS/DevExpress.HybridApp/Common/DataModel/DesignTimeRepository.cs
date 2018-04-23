using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using DevExpress.Mvvm;
using DevExpress.DevAV.Common.Utils;

namespace DevExpress.DevAV.Common.DataModel {
    /// <summary>
    /// DesignTimeRepository is an IRepository interface implementation representing the collection of entities of a given type for design-time mode. 
    /// DesignTimeRepository objects are created from a DesignTimeUnitOfWork class instance using the GetRepository method. 
    /// Write operations against entities of a given type are not supported in this implementation and throw InvalidOperationException.
    /// </summary>
    /// <typeparam name="TEntity">A repository entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">An entity primary key type.</typeparam>
    public class DesignTimeRepository<TEntity, TPrimaryKey> : DesignTimeReadOnlyRepository<TEntity>, IRepository<TEntity, TPrimaryKey>
        where TEntity : class {

        readonly Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression;
        readonly EntityTraits<TEntity, TPrimaryKey> entityTraits;

        /// <summary>
        /// Initializes a new instance of the DesignTimeRepository class.
        /// </summary>
        /// <param name="getPrimaryKeyExpression">A lambda-expression that returns the entity primary key.</param>
        public DesignTimeRepository(DesignTimeUnitOfWork unitOfWork, Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression)
            : base(unitOfWork) {
            this.getPrimaryKeyExpression = getPrimaryKeyExpression;
            this.entityTraits = ExpressionHelper.GetEntityTraits(this, getPrimaryKeyExpression);
        }

        protected virtual TEntity CreateCore() {
            return DesignTimeHelper.CreateDesignTimeObject<TEntity>();
        }

        protected virtual void AttachCore(TEntity entity) {
        }

        protected virtual void DetachCore(TEntity entity) {
        }

        protected virtual void UpdateCore(TEntity entity) {
        }

        protected virtual EntityState GetStateCore(TEntity entity) {
            return EntityState.Detached;
        }

        protected virtual TEntity FindCore(TPrimaryKey key) {
            throw new InvalidOperationException();
        }

        protected virtual void RemoveCore(TEntity entity) {
            throw new InvalidOperationException();
        }

        protected virtual TEntity ReloadCore(TEntity entity) {
            throw new InvalidOperationException();
        }

        protected virtual TPrimaryKey GetPrimaryKeyCore(TEntity entity) {
            return entityTraits.GetPrimaryKey(entity);
        }

        protected virtual void SetPrimaryKeyCore(TEntity entity, TPrimaryKey key) {
            var setPrimaryKeyaction = entityTraits.SetPrimaryKey;
            setPrimaryKeyaction(entity, key);
        }

        #region IRepository
        TEntity IRepository<TEntity, TPrimaryKey>.Find(TPrimaryKey key) {
            return FindCore(key);
        }

        void IRepository<TEntity, TPrimaryKey>.Remove(TEntity entity) {
            RemoveCore(entity);
        }

        TEntity IRepository<TEntity, TPrimaryKey>.Create() {
            return CreateCore();
        }

        void IRepository<TEntity, TPrimaryKey>.Attach(TEntity entity) {
            AttachCore(entity);
        }

        void IRepository<TEntity, TPrimaryKey>.Detach(TEntity entity) {
            DetachCore(entity);
        }

        void IRepository<TEntity, TPrimaryKey>.Update(TEntity entity) {
            UpdateCore(entity);
        }

        EntityState IRepository<TEntity, TPrimaryKey>.GetState(TEntity entity) {
            return GetStateCore(entity);
        }

        TEntity IRepository<TEntity, TPrimaryKey>.Reload(TEntity entity) {
            return ReloadCore(entity);
        }
        Expression<Func<TEntity, TPrimaryKey>> IRepository<TEntity, TPrimaryKey>.GetPrimaryKeyExpression {
            get { return getPrimaryKeyExpression; }
        }

        TPrimaryKey IRepository<TEntity, TPrimaryKey>.GetPrimaryKey(TEntity entity) {
            return GetPrimaryKeyCore(entity);
        }

        bool IRepository<TEntity, TPrimaryKey>.HasPrimaryKey(TEntity entity) {
            return entityTraits.HasPrimaryKey(entity);
        }

        void IRepository<TEntity, TPrimaryKey>.SetPrimaryKey(TEntity entity, TPrimaryKey key) {
            SetPrimaryKeyCore(entity, key);
        }
        #endregion
    }
}