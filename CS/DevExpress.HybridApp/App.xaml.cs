using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Core;
using DevExpress.DevAV;
using DevExpress.DevAV.Data.Generator;
using DevExpress.DevAV.Generator;
using System.Data.Entity;

namespace DevExpress.HybridApp {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            //var deleteContext = new DevAVDb();
            //if(deleteContext.Database.Exists())
            //    deleteContext.Database.Delete();

            var context = new DevAVDb();
            if(!context.Database.Exists()) {
                DXSplashScreen.Show<GenerateDBSplashScreen>();
                DXSplashScreen.SetState("Generating database...");
                try {

                    Database.SetInitializer<DevAVDb>(new DatabaseInitializer());
                    context.Customers.Count();
                } finally {
                    DXSplashScreen.Close();
                }
            }
        }

        private void OnAppStartup_UpdateThemeName(object sender, StartupEventArgs e) {
            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
        }
    }
}
