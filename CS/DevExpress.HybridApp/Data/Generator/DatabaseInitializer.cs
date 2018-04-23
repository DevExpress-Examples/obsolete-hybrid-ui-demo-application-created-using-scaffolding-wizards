using System;
using System.Data.Entity;

namespace DevExpress.DevAV.Generator {
    public class DatabaseInitializer : CreateDatabaseIfNotExists<DevAVDb> {
        protected override void Seed(DevAVDb context) {
            var generator = new DatabaseGenerator();
            base.Seed(context);
            generator.Seed(context);
        }
    }
}