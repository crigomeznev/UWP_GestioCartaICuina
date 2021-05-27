using CookomaticDB.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace GestioComandes.View
{
    public class ComandaViewModel : INotifyPropertyChanged
    {
        private long codi;
        private DateTime data;
        private int taula;
        //private CambrerDB cambrer;
        private ComandaDB comandaOriginal;
        //private ObservableCollection<LiniaComandaDB> linies;

        public event PropertyChangedEventHandler PropertyChanged;

        // HAURIA D'ANAR EN UN VIEWMODEL!!!
        // Ens registrem  a l'event property changed 
        // Si l'objecte Actor és modificat (via binding de la interfície
        // gràfica ), ens avisarà.
        //    Actor.PropertyChanged += Actor_PropertyChanged;
        //    }

        //private void Actor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (EstatForm == Estat.SENSE_CANVIS)
        //    {
        //        EstatForm = Estat.MODIFICAT;
        //    }
        //}

        public ComandaViewModel(ComandaDB comandaOriginal)
        {
            ComandaOriginal = comandaOriginal;
        }

        public ComandaViewModel()
        {
        }

        //public ComandaViewModel(long codi, DateTime data, int taula, CambrerDB cambrer, ObservableCollection<LiniaComandaDB> linies)
        //{
        //    Codi = codi;
        //    Data = data;
        //    Taula = taula;
        //    Cambrer = cambrer;
        //    //Linies = linies;
        //    this.linies = linies;

        //    //this.linies.property
        //}

        public long Codi { get => codi; set => codi = value; }
        public DateTime Data { get => data; set => data = value; }
        public int Taula { get => taula; set => taula = value; }
        public ComandaDB ComandaOriginal
        {
            get => comandaOriginal;
            set 
            { 
                comandaOriginal = value;

                Codi = comandaOriginal.Codi;
                Data = comandaOriginal.Data;
                Taula = comandaOriginal.Taula;
            }
        }
        public decimal getBaseImposable()
        {
            return 0;
        }
        public decimal getIVA()
        {
            return 0;
        }







    }
}
