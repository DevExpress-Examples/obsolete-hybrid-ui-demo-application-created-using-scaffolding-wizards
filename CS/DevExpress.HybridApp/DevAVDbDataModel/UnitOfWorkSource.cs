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
using DevExpress.Mvvm;

namespace DevExpress.DevAV.DevAVDbDataModel {
    /// <summary>
    /// Provides methods to obtain the relevant IUnitOfWorkFactory.
    /// </summary>
    public static class UnitOfWorkSource {

        #region inner classes
        class DbUnitOfWorkFactory : IUnitOfWorkFactory<IDevAVDbUnitOfWork> {
            public static readonly IUnitOfWorkFactory<IDevAVDbUnitOfWork> Instance = new DbUnitOfWorkFactory();
            DbUnitOfWorkFactory() { }
            IDevAVDbUnitOfWork IUnitOfWorkFactory<IDevAVDbUnitOfWork>.CreateUnitOfWork() {
                return new DevAVDbUnitOfWork(() => new DevAVDb());
            }
        }

        class DesignUnitOfWorkFactory : IUnitOfWorkFactory<IDevAVDbUnitOfWork> {
            public static readonly IUnitOfWorkFactory<IDevAVDbUnitOfWork> Instance = new DesignUnitOfWorkFactory();
            DesignUnitOfWorkFactory() { }
            IDevAVDbUnitOfWork IUnitOfWorkFactory<IDevAVDbUnitOfWork>.CreateUnitOfWork() {
                return new DevAVDbDesignTimeUnitOfWork();
            }
        }
        #endregion

        /// <summary>
        /// Returns the IUnitOfWorkFactory implementation based on the current mode (run-time or design-time).
        /// </summary>
        public static IUnitOfWorkFactory<IDevAVDbUnitOfWork> GetUnitOfWorkFactory() {
            return GetUnitOfWorkFactory(ViewModelBase.IsInDesignMode);
        }

        /// <summary>
        /// Returns the IUnitOfWorkFactory implementation based on the given mode (run-time or design-time).
        /// </summary>
        /// <param name="isInDesignTime">Used to determine which implementation of IUnitOfWorkFactory should be returned.</param>
        public static IUnitOfWorkFactory<IDevAVDbUnitOfWork> GetUnitOfWorkFactory(bool isInDesignTime) {
            return isInDesignTime ? DesignUnitOfWorkFactory.Instance : DbUnitOfWorkFactory.Instance;
        }
    }
}