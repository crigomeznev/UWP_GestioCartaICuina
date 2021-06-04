using CookomaticDB.Model;
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

namespace GestioComandes.View
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ComandesPage : Page
    {
        // Refrescar la pàgina cada 2 segons
        private Timer _timer;


        public ObservableCollection<ComandaDB> comandes = new ObservableCollection<ComandaDB>();
        public ObservableCollection<ComandaViewModel> comandesVM = new ObservableCollection<ComandaViewModel>();


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
            comandes = ComandaDB.GetComandesPerFinalitzada(false);
            iniComandesVM();
            //PComandes = comandes;

            //foreach (ComandaDB com in comandes)
            //{
            //    UIComanda uic = new UIComanda(com);
            //    //uic.PComandaDB = com;
            //    comandesUI.Add(uic);
            //}

            //and update the UI afterwards:

            // Actualitzar UI
            //lsvComandes.ItemsSource = comandesUI;
            //lsvComandes.ItemsSource = comandesVM;


            //ComandaDB comanda = ComandaDB.GetComandes()[0];
            //provaUIComanda.PComandaDB = comanda;

            //foreach(ComandaDB com1 in comandes)
            //{
            //    UIComanda uic = new UIComanda(com1);
            //    uic.Width = 50;
            //    stkComandesUI.Children.Add(uic);
            //}


            // task que refrescarà la pàgina cada 2 segons
            _timer = new Timer(new TimerCallback((obj) => RefrescarPagina()), null, 0, 2000);
        }

        private void iniComandesVM()
        {
            comandesVM.Clear();
            foreach(ComandaDB comandaDB in comandes)
            {
                ComandaViewModel comandaVM = new ComandaViewModel(comandaDB);

                comandesVM.Add(comandaVM);
                //foreach(LiniaComandaViewModel lcvm in coman)
            }
        }

        private async void RefrescarPagina()
        {
            // Crida a la BD
            comandes = ComandaDB.GetComandesPerFinalitzada(false);
            //PComandes = comandes;

            //foreach (ComandaDB com in comandes)
            //{
            //    comandesUI.Add(new UIComanda());
            //}

            //and update the UI afterwards:

            // Actualitzar UI
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            () =>
            {
                iniComandesVM();
                // Your UI update code goes here!
                //txt.Text = dd++.ToString();
                lsvComandes.ItemsSource = comandesVM;
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
