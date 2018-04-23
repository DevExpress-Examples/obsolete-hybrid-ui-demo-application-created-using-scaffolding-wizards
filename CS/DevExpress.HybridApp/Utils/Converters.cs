

using DevExpress.Utils;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DevExpress.DevAV {
    public static class Converters {
        public static BitmapImage LocalUriToImage(string value) {
            return value != null ? new BitmapImage(AssemblyHelper.GetResourceUri(typeof(Converters).Assembly, value)) : null;
        }
    }
}