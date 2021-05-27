using CookomaticDB.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace GestioComandes.View
{
    public class BoolToVisibility : IValueConverter
    {
        // bool serà el booleà de comanda: finalitzada
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                bool finalitzada = (bool)value;
                if (finalitzada)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility val = (Visibility)value;

            if (val.Equals(Visibility.Visible)) return true;
            return false;
        }
    }
}
