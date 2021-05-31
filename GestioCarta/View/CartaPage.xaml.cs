using CookomaticDB;
using CookomaticDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization.NumberFormatting;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace GestioCarta.View
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CartaPage : Page
    {
        private ObservableCollection<CategoriaDB> llistaCategories;
        private ObservableCollection<PlatDB> llistaPlats;
        private ObservableCollection<PlatViewModel> llistaPlatsVM;

        //private PlatDB selectedPlat;
        private PlatViewModel selectedPlat;
        public PlatViewModel SelectedPlat
        {
            get => selectedPlat;
            set
            {
                selectedPlat = value;

                // UI
                if (selectedPlat == null)
                    btnEliminarPlat.Visibility = Visibility.Collapsed;
                else
                    btnEliminarPlat.Visibility = Visibility.Visible;

                tbkNou.Text = selectedPlat==null? "" : selectedPlat.Nom;
                txbPlatNom.Text = selectedPlat==null? "" : selectedPlat.Nom;
                txbPlatDescripcio.Text = selectedPlat==null? "" : selectedPlat.DescripcioMD;
                nbbPlatPreu.Value = selectedPlat==null? 0.00 : (double)selectedPlat.Preu;
                chkDisponible.IsChecked = selectedPlat==null? false : selectedPlat.Disponible;
            }
        }


        private EstatView estat;
        internal EstatView Estat
        {
            get => estat;
            set 
            {
                estat = value;

                if (estat.Equals(EstatView.MODIFICACIO))
                    btnActualitzarPlat.Visibility = Visibility.Visible;
                else
                    btnActualitzarPlat.Visibility = Visibility.Collapsed;

                switch (estat)
                {
                    case EstatView.CONSULTA:
                    case EstatView.MODIFICACIO:
                        btnNetejarSeleccio.Visibility = Visibility.Visible;
                        btnEliminarPlat.Visibility = Visibility.Visible;
                        btnNouPlat.Visibility = Visibility.Visible;
                        btnInserirPlat.Visibility = Visibility.Collapsed;
                        break;
                    case EstatView.NOU:
                        netejarForm();
                        btnNetejarSeleccio.Visibility = Visibility.Collapsed;
                        btnEliminarPlat.Visibility = Visibility.Collapsed;
                        btnNouPlat.Visibility = Visibility.Collapsed;
                        btnInserirPlat.Visibility = Visibility.Visible;
                        tbkNou.Text = "Nou Plat";
                        //btnActualitzarPlat.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }


        // Gestió de l'altra pantalla
        // Track open app windows in a Dictionary.
        public static Dictionary<UIContext, AppWindow> AppWindows { get; set; }
            = new Dictionary<UIContext, AppWindow>();


        // Ruta de foto seleccionada
        private string selectedPhotoPath;
        private byte[] selectedPhotoBa;

        private void netejarForm()
        {
            //tbkNou.Text = plat.Nom;
            txbPlatNom.Text = "";
            txbPlatDescripcio.Text = "";
            nbbPlatPreu.Value = 10.00;
            chkDisponible.IsChecked = true;
            imgPlatFoto.Source = null;
        }

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


        public CartaPage()
        {
            this.InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetNumberBoxNumberFormatter();
            try
            {
                // Viewmodel
                llistaPlatsVM = new ObservableCollection<PlatViewModel>();

                llistaCategories = CategoriaDB.GetCategories();
                llistaPlats = PlatDB.GetPlats();
                CarregarPlatsVM();

                lsvCategories.ItemsSource = llistaCategories;
                lsvPlats.ItemsSource = llistaPlatsVM;

                Estat = EstatView.CONSULTA;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }



        #region ViewModel
        private async void CarregarPlatsVM()
        {
            llistaPlatsVM.Clear();
            if (llistaPlats != null)
            {
                foreach(PlatDB platDB in llistaPlats)
                {
                    PlatViewModel platVM = new PlatViewModel(platDB);
                    try
                    {
                        await platVM.iniFotoAsync();
                    } catch(Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    llistaPlatsVM.Add(platVM);
                }
            }
        }
        #endregion

        private void btnMostrarPlats_Click(object sender, RoutedEventArgs e)
        {
            // filtre per nom
            string nomFiltre = txbFiltreNomPlat.Text;
            llistaPlats = PlatDB.GetPlatsPerNom(nomFiltre);
            CarregarPlatsVM();
            lsvPlats.ItemsSource = llistaPlatsVM;

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
                CarregarPlatsVM();
                lsvPlats.ItemsSource = llistaPlatsVM;
            }
        }

        //private async void lsvPlats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        private void lsvPlats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvPlats.SelectedValue != null)
            {
                // UI
                Estat = EstatView.MODIFICACIO;

                SelectedPlat = (PlatViewModel)lsvPlats.SelectedItem;
                if (SelectedPlat.Foto != null)
                {
                    //imgPlatFoto.Source = await ByteArrayToImage(selectedPlat.Foto);
                    imgPlatFoto.Source = SelectedPlat.Foto;
                }
            }
            else
            {
                // UI

                // Com que listview de plats no tindrà cap plat seleccionat, estat del view: NOU
                Estat = EstatView.NOU;

                //btnInserirPlat.Visibility = Visibility.Visible;
            }

        }


        private void btnNetejarSeleccio_Click(object sender, RoutedEventArgs e)
        {
            NetejarSeleccio();
        }



        private void NetejarSeleccio()
        {
            Estat = EstatView.NOU;
            lsvCategories.SelectedValue = null;
            lsvPlats.SelectedValue = null;
        }

        #region DML UI
        // UPDATE
        private void btnActualitzarPlat_Click(object sender, RoutedEventArgs e)
        {
            SelectedPlat.Nom = txbPlatNom.Text;
            SelectedPlat.DescripcioMD = txbPlatDescripcio.Text;
            SelectedPlat.Preu = Convert.ToDecimal(nbbPlatPreu.Value);
            SelectedPlat.Disponible = chkDisponible.IsChecked.Value;
            //SelectedPlat.Foto = (BitmapImage)imgPlatFoto.Source;
            SelectedPlat.FotoBa = selectedPhotoBa;

            SelectedPlat.PlatOriginal.Update();

            llistaPlats = PlatDB.GetPlats();
            CarregarPlatsVM();
            lsvPlats.ItemsSource = llistaPlatsVM;
            SelectedPlat = null;
        }

        // INSERT
        private async void btnInserirPlat_Click(object sender, RoutedEventArgs e)
        {
            //PlatDB nouPlat;
            PlatViewModel nouPlat;

            if (lsvCategories.SelectedValue == null)
            {
                var dialog = new MessageDialog("Selecciona una categoria", "Avís");
                await dialog.ShowAsync();
                return;
            }

            // Inserir nou plat
            nouPlat = new PlatViewModel();
            nouPlat.PlatOriginal = new PlatDB();
            nouPlat.Nom = txbPlatNom.Text;
            nouPlat.DescripcioMD = txbPlatDescripcio.Text;

            Decimal preu = Convert.ToDecimal(nbbPlatPreu.Value);
            //Decimal.TryParse(nbbPlatPreu.Value, out preu);
            nouPlat.Preu = preu;
            // TODO FOTO
            //nouPlat.Foto = ImageToByteArray((BitmapImage)imgPlatFoto.Source);
            //nouPlat.Foto = (BitmapImage)imgPlatFoto.Source;
            nouPlat.FotoBa = selectedPhotoBa;
            nouPlat.Disponible = chkDisponible.IsEnabled ? true : false;
            nouPlat.PlatOriginal.Categoria = (CategoriaDB)lsvCategories.SelectedValue;

            nouPlat.PlatOriginal.Insert();

            // Tornar a estat modificació:
            Estat = EstatView.MODIFICACIO;
            SelectedPlat = null;
        }

        // DELETE
        //private async System.Threading.Tasks.Task btnEliminarPlat_ClickAsync(object sender, RoutedEventArgs e)
        private async void btnEliminarPlat_Click(object sender, RoutedEventArgs e)
        {
            if (lsvPlats.SelectedItem != null)
            {
                //PlatDB p = (PlatDB)lsvPlats.SelectedItem;
                PlatDB p = SelectedPlat.PlatOriginal;

                int liniesImplicades = LiniaComandaDB.GetNumLiniesPerPlat(p);

                if (liniesImplicades == 0)
                {
                    // podem eliminar plat
                    p.Delete();
                    // actualitzar taula
                    llistaPlats = PlatDB.GetPlats();
                    CarregarPlatsVM();
                    lsvPlats.ItemsSource = llistaPlatsVM;

                }
                else
                {
                    var dialog = new MessageDialog("No es pot eliminar plat. Hi ha línies de comanda relacionades", "Error");
                    await dialog.ShowAsync();

                    //Debug.WriteLine("No es pot eliminar plat. Hi ha línies de comanda relacionades");
                }
            }
        }
        #endregion

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
                    selectedPhotoPath = copiedFile.Path;
                    // guardem foto en bitmapimage i en byte array
                    imgPlatFoto.Source = tmpBitmap;


                    // Guardar foto en byte array
                    FileStream fs = new FileStream(selectedPhotoPath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    //Array.Clear(selectedPhotoBa, 0, selectedPhotoBa.Length);
                    selectedPhotoBa = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        //public static async Task<BitmapImage> ByteArrayToImage(Byte[] bytes)
        //{
        //    BitmapImage image = new BitmapImage();
        //    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
        //    {
        //        await stream.WriteAsync(bytes.AsBuffer());
        //        stream.Seek(0);
        //        await image.SetSourceAsync(stream);
        //    }
        //    return image;
        //}


        private void btnNouPlat_Click(object sender, RoutedEventArgs e)
        {
            // UI
            //btnInserirPlat.Visibility = Visibility.Visible;

            // TODO: Estats
            Estat = EstatView.NOU;
        }


        #region UI Update
        //private void PlatModificat(object sender, TextChangedEventArgs e)
        //{
        //    if (Estat.Equals(EstatView.MODIFICACIO)) formModificat = true;
        //}
        #endregion


        //private void PlatModificat(object sender, TextChangedEventArgs e)
        //{
        //}

        private void PlatModificat(object sender, RoutedEventArgs e)
        {
            if (Estat.Equals(EstatView.CONSULTA)) Estat = EstatView.MODIFICACIO;
        }


        // Obrir finestra amb webview a jasperreports
        private async void btnObrirReport_Click(object sender, RoutedEventArgs e)
        {
            // Create a new window.
            AppWindow appWindow = await AppWindow.TryCreateAsync();

            // Create a Frame and navigate to the Page you want to show in the new window.
            Frame appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(typeof(ImprimirCartaPage));

            // Attach the XAML content to the window.
            ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);

            // Add the new page to the Dictionary using the UIContext as the Key.
            AppWindows.Add(appWindowContentFrame.UIContext, appWindow);
            appWindow.Title = "App Window " + AppWindows.Count.ToString();

            // When the window is closed, be sure to release
            // XAML resources and the reference to the window.
            appWindow.Closed += delegate
            {
                Debug.WriteLine("TANCANT FINESTRA REPORTS");
                CartaPage.AppWindows.Remove(appWindowContentFrame.UIContext);
                appWindowContentFrame.Content = null;
                appWindow = null;
            };

            // Show the window.
            await appWindow.TryShowAsync();
        }

        // Tancar finestra amb webview a jasperreports
        private async void btnTancarReport_Click(object sender, RoutedEventArgs e)
        {
            while (AppWindows.Count > 0)
            {
                await AppWindows.Values.First().CloseAsync();
            }
        }


        //private void PlatModificat(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
        //{
        //    PlatModificat(sender, args);
        //}
    }
}
