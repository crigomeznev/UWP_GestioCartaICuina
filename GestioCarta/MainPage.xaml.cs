using CookomaticDB;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace GestioCarta
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<CategoriaDB> llistaCategories;
        private ObservableCollection<PlatDB> llistaPlats;
        public MainPage()
        {
            this.InitializeComponent();

            //try
            //{
            //try
            //{
            llistaCategories = CategoriaDB.GetCategories();
            //    llistaPlats = PlatDB.GetPlats();
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
            lsvCategories.ItemsSource = llistaCategories;
            //dtgPlats.ItemsSource = llistaPlats;
            //}catch(Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
        }

        private void btnMostrarPlats_Click(object sender, RoutedEventArgs e)
        {
            // filtrar per categoria
            if (lsvCategories.SelectedValue != null)
            {
                CategoriaDB categoria = (CategoriaDB)lsvCategories.SelectedValue;
                llistaPlats = PlatDB.GetPlatsPerCategoria(categoria.Codi);
            }
            else
            {
                // filtre per nom
                string nomFiltre = txbNomPlat.Text;
                //if (nomFiltre == null || nomFiltre.Length == 0) nomFiltre = "%";
                //else if (!nomFiltre.Contains('%'))
                //{
                //    // envoltem cadena de %
                //    string nomFiltreAux = "%";
                //    nomFiltreAux += nomFiltre;
                //    nomFiltreAux += "%";
                //}
                llistaPlats = PlatDB.GetPlatsPerNom(nomFiltre);
            }
            dtgPlats.ItemsSource = llistaPlats;
        }

        private void btnNetejarSeleccio_Click(object sender, RoutedEventArgs e)
        {
            lsvCategories.SelectedValue = null;
        }
    }
}
