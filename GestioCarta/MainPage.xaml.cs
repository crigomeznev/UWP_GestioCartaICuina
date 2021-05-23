using CookomaticDB;
using CookomaticDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization.NumberFormatting;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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

        //private Dictionary<long, PlatDB> llistaPlats;

        private void SetNumberBoxNumberFormatter()
        {
            //IncrementNumberRounder rounder = new IncrementNumberRounder();
            //rounder.Increment = 0.25;
            //rounder.RoundingAlgorithm = RoundingAlgorithm.RoundUp;

            DecimalFormatter formatter = new DecimalFormatter();
            formatter.IntegerDigits = 1;
            formatter.FractionDigits = 2;
            //formatter.NumberRounder = rounder;
            nbbPlatPreu.NumberFormatter = formatter;
        }


        public MainPage()
        {
            this.InitializeComponent();

            SetNumberBoxNumberFormatter();
            try
            {
                llistaCategories = CategoriaDB.GetCategories();
                llistaPlats = PlatDB.GetPlats();
                lsvCategories.ItemsSource = llistaCategories;
                //lsvPlats.ItemsSource = llistaPlats;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void btnMostrarPlats_Click(object sender, RoutedEventArgs e)
        {
            // filtre per nom
            string nomFiltre = txbFiltreNomPlat.Text;
            llistaPlats = PlatDB.GetPlatsPerNom(nomFiltre);
            lsvPlats.ItemsSource = llistaPlats;

            // netegem seleccio
            NetejarSeleccio();
        }

        private void lsvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // filtrar per categoria
            if (lsvCategories.SelectedValue != null)
            {
                CategoriaDB categoria = (CategoriaDB)lsvCategories.SelectedValue;
                llistaPlats = PlatDB.GetPlatsPerCategoria(categoria);
                lsvPlats.ItemsSource = llistaPlats;
            }
        }

        private void btnNetejarSeleccio_Click(object sender, RoutedEventArgs e)
        {
            NetejarSeleccio();
        }
        


        private void NetejarSeleccio()
        {
            lsvCategories.SelectedValue = null;
        }

        //private async System.Threading.Tasks.Task btnEliminarPlat_ClickAsync(object sender, RoutedEventArgs e)
        private async void btnEliminarPlat_Click(object sender, RoutedEventArgs e)
        {
            if (lsvPlats.SelectedItem != null)
            {
                PlatDB p = (PlatDB)lsvPlats.SelectedItem;

                int liniesImplicades = LiniaComandaDB.GetNumLiniesPerPlat(p);

                if (liniesImplicades == 0)
                {
                    // podem eliminar plat
                    p.Delete();
                    // actualitzar taula
                    llistaPlats = PlatDB.GetPlats();
                    lsvPlats.ItemsSource = llistaPlats;

                }
                else
                {
                    var dialog = new MessageDialog("No es pot eliminar plat. Hi ha línies de comanda relacionades", "Error");
                    await dialog.ShowAsync();

                    //Debug.WriteLine("No es pot eliminar plat. Hi ha línies de comanda relacionades");
                }
            }
        }

        private async void btnInserirPlat_Click(object sender, RoutedEventArgs e)
        {
            PlatDB nouPlat;

            if (lsvCategories.SelectedValue == null)
            {
                var dialog = new MessageDialog("Selecciona una categoria", "Avís");
                await dialog.ShowAsync();
                return;
            }


            nouPlat = new PlatDB();
            nouPlat.Nom = txbPlatNom.Text;
            nouPlat.DescripcioMD = txbPlatDescripcio.Text;

            Decimal preu = Convert.ToDecimal(nbbPlatPreu.Value);
            //Decimal.TryParse(nbbPlatPreu.Value, out preu);
            nouPlat.Preu = preu;
            // TODO FOTO
            nouPlat.Foto = (BitmapImage)imgPlatFoto.Source;
            nouPlat.Disponible = chkDisponible.IsEnabled ? true : false;
            nouPlat.Categoria = (CategoriaDB)lsvCategories.SelectedValue;

            nouPlat.Insert();
        }



        /// Obrir un selector d'arxius, triar un arxiu i copiar-lo a la carpeta ApplicationData del
        /// programa. Crear una imatge en memòria a partir de l'arxiu.
        private async void btnPlatFoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker fp = new FileOpenPicker();
                fp.FileTypeFilter.Add(".jpg");
                fp.FileTypeFilter.Add(".png");

                StorageFile sf = await fp.PickSingleFileAsync();

                if (sf != null)
                {
                    // Cerca la carpeta de dades de l'aplicació, dins de ApplicationData
                    var folder = ApplicationData.Current.LocalFolder;
                    // Dins de la carpeta de dades, creem una nova carpeta "icons"
                    var iconsFolder = await folder.CreateFolderAsync("icons", CreationCollisionOption.OpenIfExists);
                    // Creem un nom usant la data i hora, de forma que no poguem repetir noms.
                    string name = (DateTime.Now).ToString("yyyyMMddhhmmss") + "_" + sf.Name;
                    // Copiar l'arxiu triat a la carpeta indicada, usant el nom que hem muntat
                    StorageFile copiedFile = await sf.CopyAsync(iconsFolder, name);
                    // Crear una imatge en memòria (BitmapImage) a partir de l'arxiu copiat a ApplicationData
                    BitmapImage tmpBitmap = new BitmapImage(new Uri(copiedFile.Path));
                    // ..... YOUR CODE HERE ...........

                    imgPlatFoto.Source = tmpBitmap;
                }
            } catch (Exception ex)
            {
            }
        }

        private void lsvPlats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlatDB plat = (PlatDB)lsvPlats.SelectedItem;
            imgPlatFoto.Source = plat.Foto;
        }
    }
}
