using DevExpress.Data.Filtering;
using DevExpress.DevAV;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.DevAV.ViewModels;
using DevExpress.Xpf.Editors.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DevExpress.DevAV.Views {
    public partial class CustomFilterView : UserControl {
        CustomFilterViewModel ViewModel { get { return (CustomFilterViewModel)DataContext; } }
        public CustomFilterView() {
            InitializeComponent();
        }
    }
}
