using CookomaticDB.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace GestioComandes.View
{
    public class LiniaComandaViewModel : INotifyPropertyChanged
    {
        // INotifyPropertyChanged
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion

        private EstatLinia estat;

        private LiniaComandaDB liniaComandaOriginal;


        public LiniaComandaViewModel()
        {
        }

        public LiniaComandaViewModel(LiniaComandaDB liniaComandaOriginal)
        {
            LiniaComandaOriginal = liniaComandaOriginal;

            // IMPORTANT: SENSE SETTER!
            estat = liniaComandaOriginal.Estat;
        }

        public EstatLinia Estat
        {
            get => estat;
            set 
            { 
                estat = value;
                // en principi, des del viewmodel només podem modificar l'estat de la línia


                RaisePropertyChange(); // notifiquem canvi
                //RaisePropertyChange("Estat");

                // Update BD
                if (LiniaComandaOriginal != null)
                {
                    LiniaComandaOriginal.Estat = value;
                    LiniaComandaOriginal.Update();
                }
            }
        }

        public LiniaComandaDB LiniaComandaOriginal
        {
            get => liniaComandaOriginal;
            set 
            { 
                liniaComandaOriginal = value;
                // (Apliquem altres camps)?
            }
        }

        public int getImport()
        {
            return 0;
        }

        public override string ToString()
        {
            return LiniaComandaOriginal.Num + " " + LiniaComandaOriginal.Quantitat;
        }

    }
}
