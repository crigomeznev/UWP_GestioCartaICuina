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
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace GestioCarta.View
{
    public class BoolToDisponibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool disponible = (bool)value;

            return disponible ? "Disponible" : "No disponible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string disponibleS = (string)value;

            // Qualsevol cosa que no sigui "Disponible", voldrà dir disponible = false
            return disponibleS.ToUpper().Equals("Disponible");
        }




    }
}
