using CookomaticDB.Model;
using System;
using System.Collections.Generic;
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
    public sealed partial class UILiniaComanda : UserControl
    {
        public UILiniaComanda()
        {
            this.InitializeComponent();
        }

        public LiniaComandaDB PLiniaComandaDB
        {
            get { return (LiniaComandaDB)GetValue(PLiniaComandaDBProperty); }
            set { SetValue(PLiniaComandaDBProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PLiniaComandaDB.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PLiniaComandaDBProperty =
            DependencyProperty.Register("PLiniaComandaDB", typeof(LiniaComandaDB), typeof(UILiniaComanda), new PropertyMetadata(new LiniaComandaDB(), LiniaComandaDBChangedCallbackStatic));

        private static void LiniaComandaDBChangedCallbackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UILiniaComanda ui = (UILiniaComanda)d;
            ui.LiniaComandaDBChangedCallback();
        }
        private void LiniaComandaDBChangedCallback()
        {
            PNum = PLiniaComandaDB.Num;
            PPlat = PLiniaComandaDB.Item;
            PQuantitat = PLiniaComandaDB.Quantitat;
            PEstat = PLiniaComandaDB.Estat;
        }




        #region PNum
        public int PNum
        {
            get { return (int)GetValue(PNumProperty); }
            set { SetValue(PNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PNumProperty =
            DependencyProperty.Register("PNum", typeof(int), typeof(UILiniaComanda), new PropertyMetadata(0));
        #endregion



        #region PPlat
        public PlatDB PPlat
        {
            get { return (PlatDB)GetValue(PPlatProperty); }
            set { SetValue(PPlatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PPlat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PPlatProperty =
            DependencyProperty.Register("PPlat", typeof(PlatDB), typeof(UILiniaComanda), new PropertyMetadata(new PlatDB()));
        #endregion



        #region PQuantitat
        public int PQuantitat
        {
            get { return (int)GetValue(PQuantitatProperty); }
            set { SetValue(PQuantitatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PQuantitat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PQuantitatProperty =
            DependencyProperty.Register("PQuantitat", typeof(int), typeof(UILiniaComanda), new PropertyMetadata(0));
        #endregion



        #region PEstat
        public EstatLinia PEstat
        {
            get { return (EstatLinia)GetValue(PEstatProperty); }
            set { SetValue(PEstatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PEstat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PEstatProperty =
            DependencyProperty.Register("PEstat", typeof(EstatLinia), typeof(UILiniaComanda), new PropertyMetadata(EstatLinia.EN_PREPARACIO));
        #endregion




        #region PLiniaComandaVM
        public LiniaComandaViewModel PLiniaComandaVM
        {
            get { return (LiniaComandaViewModel)GetValue(PLiniaComandaVMProperty); }
            set { SetValue(PLiniaComandaVMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PLiniaComandaVM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PLiniaComandaVMProperty =
            DependencyProperty.Register("PLiniaComandaVM", typeof(LiniaComandaViewModel), typeof(UILiniaComanda), new PropertyMetadata(new LiniaComandaViewModel()));
        #endregion



    }
}
