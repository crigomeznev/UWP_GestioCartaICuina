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
        private ComandaDB comandaOriginal;
        private ObservableCollection<LiniaComandaViewModel> linies = new ObservableCollection<LiniaComandaViewModel>();
        private bool finalitzada;
        public event PropertyChangedEventHandler PropertyChanged;

        public ComandaViewModel(ComandaDB comandaOriginal)
        {
            ComandaOriginal = comandaOriginal;
        }

        public ComandaViewModel()
        {
        }

        public long Codi { get => codi; set => codi = value; }
        public DateTime Data { get => data; set => data = value; }
        public int Taula { get => taula; set => taula = value; }
        public ObservableCollection<LiniaComandaViewModel> Linies { get => linies; set => linies = value; }
        public ComandaDB ComandaOriginal
        {
            get => comandaOriginal;
            set 
            { 
                comandaOriginal = value;

                Codi = comandaOriginal.Codi;
                Data = comandaOriginal.Data;
                Taula = comandaOriginal.Taula;

                // Finalitzada: camp calculat -> aprofitem aquest bucle per calcular-lo
                finalitzada = true;

                // reassignem linies
                Linies.Clear();
                foreach(LiniaComandaDB liniaDB in comandaOriginal.Linies)
                {
                    LiniaComandaViewModel lcvm = new LiniaComandaViewModel(liniaDB);
                    //lcvm.PropertyChanged += LiniaComandaVM_PropertyChanged;

                    Linies.Add(new LiniaComandaViewModel(liniaDB));

                    // a la que trobem una línia EN_PREPARACIO, comanda.FINALITZADA passa a fals
                    if (lcvm.Estat.Equals(EstatLinia.EN_PREPARACIO))
                        finalitzada = false; // NO usem setter
                }
            }
        }

        // Mètode MOLT important
        public void ActualitzarComandaDB()
        {
            try
            {
                ComandaOriginal.Update();
            } catch (Exception ex)
            {
                // TODO: messagebox on es vegi error
                Debug.WriteLine(ex);
            }
        }
        //private void LiniaComandaVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    // qui ha llançat l'esdeveniment
        //    LiniaComandaViewModel lcvm = (LiniaComandaViewModel)sender;
        //    Debug.WriteLine("Linia comanda property changed");
        //    Debug.WriteLine(lcvm.LiniaComandaOriginal);
        //}


        // Camp calculat
        public bool Finalitzada
        {
            get => finalitzada;
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
