using CookomaticDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace GestioComandes.View
{
    public sealed partial class UIComanda : UserControl
    {
        public UIComanda()
        {
            this.InitializeComponent();
        }


        #region PComandaVM
        public ComandaViewModel PComandaVM
        {
            get { return (ComandaViewModel)GetValue(PComandaVMProperty); }
            set { SetValue(PComandaVMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PComandaVM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PComandaVMProperty =
            DependencyProperty.Register("PComandaVM", typeof(ComandaViewModel), typeof(UIComanda), new PropertyMetadata(new ComandaViewModel(), PComandaVMChangedCallbackStatic));
        private static void PComandaVMChangedCallbackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIComanda ui = (UIComanda)d;
            ui.PComandaVMChangedCallback();
        }
        private void PComandaVMChangedCallback()
        {
            PCodi = PComandaVM.Codi;
            PData = PComandaVM.Data;
            PTaula = PComandaVM.Taula;

            if (PComandaVM.ComandaOriginal != null)
            {
                PCambrer = PComandaVM.ComandaOriginal.Cambrer;
                PLinies = PComandaVM.ComandaOriginal.Linies;
            }
        }
        #endregion



        #region PCodi
        public long PCodi
        {
            get { return (long)GetValue(PCodiProperty); }
            set { SetValue(PCodiProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PCodi.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PCodiProperty =
            DependencyProperty.Register("PCodi", typeof(long), typeof(UIComanda), new PropertyMetadata(0));
        #endregion



        #region PData
        public DateTime PData
        {
            get { return (DateTime)GetValue(PDataProperty); }
            set { SetValue(PDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PDataProperty =
            DependencyProperty.Register("PData", typeof(DateTime), typeof(UIComanda), new PropertyMetadata(new DateTime()));
        #endregion



        #region PTaula
        public int PTaula
        {
            get { return (int)GetValue(PTaulaProperty); }
            set { SetValue(PTaulaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PTaula.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PTaulaProperty =
            DependencyProperty.Register("PTaula", typeof(int), typeof(UIComanda), new PropertyMetadata(0));
        #endregion


        //TODO BORRAR
        #region PCambrer
        public CambrerDB PCambrer
        {
            get { return (CambrerDB)GetValue(PCambrerProperty); }
            set { SetValue(PCambrerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PCambrer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PCambrerProperty =
            DependencyProperty.Register("PCambrer", typeof(CambrerDB), typeof(UIComanda), new PropertyMetadata(new CambrerDB()));
        #endregion


        // TODO BORRAR
        #region PLinies
        public ObservableCollection<LiniaComandaDB> PLinies
        {
            get { return (ObservableCollection<LiniaComandaDB>)GetValue(PLiniesProperty); }
            set { SetValue(PLiniesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PLinies.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PLiniesProperty =
            DependencyProperty.Register("PLinies", typeof(ObservableCollection<LiniaComandaDB>),
                typeof(UIComanda), new PropertyMetadata(new ObservableCollection<LiniaComandaDB>()));
        #endregion



        #region PLiniesVM
        public ObservableCollection<LiniaComandaViewModel> PLiniesVM
        {
            get { return (ObservableCollection<LiniaComandaViewModel>)GetValue(PLiniesVMProperty); }
            set { SetValue(PLiniesVMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PLiniesVM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PLiniesVMProperty =
            DependencyProperty.Register("PLiniesVM", typeof(ObservableCollection<LiniaComandaViewModel>), typeof(UIComanda),
                new PropertyMetadata(new ObservableCollection<LiniaComandaViewModel>()));
        #endregion


        private void btnFinalitzarComanda_Click(object sender, RoutedEventArgs e)
        {
            // Millorar:
            if (PComandaVM != null)
            {
                PComandaVM.ComandaOriginal.Finalitzada = true;

                PComandaVM.ActualitzarComandaDB();
            }
        }
    }
}
