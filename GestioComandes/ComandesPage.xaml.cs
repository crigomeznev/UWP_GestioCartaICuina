﻿using CookomaticDB.Model;
using GestioComandes.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace GestioComandes
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ComandesPage : Page
    {
        // Refrescar la pàgina cada 2 segons
        private Timer _timer;


        public ObservableCollection<ComandaDB> comandes = new ObservableCollection<ComandaDB>();


        public ObservableCollection<UIComanda> comandesUI = new ObservableCollection<UIComanda>();



        //public ObservableCollection<ComandaDB> PComandes
        //{
        //    get { return (ObservableCollection<ComandaDB>)GetValue(PComandesProperty); }
        //    set { SetValue(PComandesProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PComandes.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PComandesProperty =
        //    DependencyProperty.Register("PComandes", typeof(ObservableCollection<ComandaDB>), typeof(ComandesPage), new PropertyMetadata(new ObservableCollection<ComandaDB>()));




        public ComandesPage()
        {
            this.InitializeComponent();

            // Crida a la BD
            comandes = ComandaDB.GetComandes();
            //PComandes = comandes;

            foreach (ComandaDB com in comandes)
            {
                UIComanda uic = new UIComanda();
                uic.PComandaDB = com;
                comandesUI.Add(uic);
            }

            //and update the UI afterwards:

            // Actualitzar UI
            lsvComandes.ItemsSource = comandesUI;

            // task que refrescarà la pàgina cada 2 segons
            //_timer = new Timer(new TimerCallback((obj) => RefrescarPagina()), null, 0, 2000);
        }

        private async void RefrescarPagina()
        {
            // Crida a la BD
            comandes = ComandaDB.GetComandes();
            //PComandes = comandes;

            foreach (ComandaDB com in comandes)
            {
                comandesUI.Add(new UIComanda());
            }

            //and update the UI afterwards:

            // Actualitzar UI
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            () =>
            {
                // Your UI update code goes here!
                //txt.Text = dd++.ToString();
                lsvComandes.ItemsSource = comandes;
            });
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            Debug.WriteLine(chk.Parent);

            //Debug.WriteLine("CHECKED");
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView curLsv = (ListView)sender;

            if (curLsv.SelectedValue != null)
            {
                LiniaComandaDB lc = (LiniaComandaDB)curLsv.SelectedValue;

                Debug.WriteLine(lc);
            }
        }



        /*
else if (e.PropertyName == "Acabat")
                    {
                        Binding b = new Binding();
        b.Mode = BindingMode.TwoWay;
                        b.Source = lllcomanda[0].Acabat;

                        // Attach the binding to the target.
                        TextBlock MyText = new TextBlock();
        dg.SetBinding(DataGrid.DataContextProperty, b);
                    }
    */



    }
}
