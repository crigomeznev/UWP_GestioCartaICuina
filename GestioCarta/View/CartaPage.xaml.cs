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
        private ObservableCollection<CategoriaDB> llistaCategories = new ObservableCollection<CategoriaDB>();
        private ObservableCollection<PlatDB> llistaPlats = new ObservableCollection<PlatDB>();
        private ObservableCollection<PlatViewModel> llistaPlatsVM = new ObservableCollection<PlatViewModel>();
        private ObservableCollection<PlatViewModel> llistaPlatsVMFiltrats = new ObservableCollection<PlatViewModel>();

        private const long CATTOTES = 0;

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
                {
                    Estat = EstatView.NOU;
                    apbDelete.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Estat = EstatView.CONSULTA;
                    apbDelete.Visibility = Visibility.Visible;

                    tbkNou.Text = selectedPlat == null ? "" : selectedPlat.Nom;
                    txbPlatNom.Text = selectedPlat == null ? "" : selectedPlat.Nom;
                    txbPlatDescripcio.Text = selectedPlat == null ? "" : selectedPlat.DescripcioMD;
                    nbbPlatPreu.Value = selectedPlat == null ? 0.00 : (double)selectedPlat.Preu;
                    chkDisponible.IsChecked = selectedPlat == null ? false : selectedPlat.Disponible;

                    if (SelectedPlat.Foto != null)
                    {
                        imgPlatFoto.Source = SelectedPlat.Foto;
                        selectedPhotoBa = SelectedPlat.FotoBa;
                    }
                    else
                    {
                        imgPlatFoto.Source = null;
                        selectedPhotoBa = null;
                    }
                }
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
                        btnInserirPlat.Visibility = Visibility.Collapsed;
                        break;
                    case EstatView.NOU:
                        netejarForm();
                        btnNetejarSeleccio.Visibility = Visibility.Collapsed;
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


        // UI
        private void netejarForm()
        {
            //tbkNou.Text = plat.Nom;
            txbPlatNom.Text = "";
            txbPlatDescripcio.Text = "";
            nbbPlatPreu.Value = 10.00;
            chkDisponible.IsChecked = true;
            imgPlatFoto.Source = null;
            selectedPhotoPath = null;
            selectedPhotoBa = null;
        }
        private void SetNumberBoxNumberFormatter()
        {
            DecimalFormatter formatter = new DecimalFormatter();
            formatter.IntegerDigits = 1;
            formatter.FractionDigits = 2;
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
                // Inicialment carreguem tots els plats
                llistaCategories = CategoriaDB.GetCategories();
                llistaPlats = PlatDB.GetPlats();
                CarregarPlatsVM();

                lsvCategories.ItemsSource = llistaCategories;
                // afegim botó per veure totes les categories
                CategoriaDB cat = new CategoriaDB(CATTOTES, "Totes", System.Drawing.Color.White);
                llistaCategories.Add(cat);

                lsvPlats.ItemsSource = llistaPlatsVMFiltrats;

                Estat = EstatView.NOU;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        #region DB
        private void CarregarPlatsDB()
        {
            llistaPlats.Clear();
            llistaPlats = PlatDB.GetPlats();
        }
        #endregion



        #region ViewModel
        private async void CarregarPlatsVM()
        {
            llistaPlatsVM.Clear();
            llistaPlatsVMFiltrats.Clear();
            if (llistaPlats != null)
            {
                foreach (PlatDB platDB in llistaPlats)
                {
                    PlatViewModel platVM = new PlatViewModel(platDB);
                    try
                    {
                        await platVM.iniFotoAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    llistaPlatsVM.Add(platVM);
                    llistaPlatsVMFiltrats.Add(platVM);
                }
            }
        }

        private void FiltrarPerCategoria(CategoriaDB categoria)
        {
            llistaPlatsVMFiltrats.Clear();
            foreach (PlatViewModel plat in llistaPlatsVM)
            {
                if (plat.PlatOriginal.Categoria.Equals(categoria) || categoria.Codi== CATTOTES)
                {
                    llistaPlatsVMFiltrats.Add(plat);
                }
            }
            lsvPlats.ItemsSource = llistaPlatsVMFiltrats;
        }

        private void FiltrarPerNom(string nom)
        {
            llistaPlatsVMFiltrats.Clear();

            // Categoria seleccionada pot ser null
            CategoriaDB categoriaSeleccionada = (CategoriaDB)lsvCategories.SelectedValue;

            foreach (PlatViewModel plat in llistaPlatsVM)
            {
                if (plat.PlatOriginal.Nom.ToUpper().Contains(nom.ToUpper()) &&
                    ((categoriaSeleccionada == null || categoriaSeleccionada.Codi==CATTOTES)
                    || plat.PlatOriginal.Categoria.Equals(categoriaSeleccionada)))
                {
                    llistaPlatsVMFiltrats.Add(plat);
                }
            }
        }

        #endregion

        private void btnMostrarPlats_Click(object sender, RoutedEventArgs e)
        {
            // filtre per nom
            string nomFiltre = txbFiltreNomPlat.Text;

            FiltrarPerNom(nomFiltre);
        }

        private void lsvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // filtrar per categoria
            if (lsvCategories.SelectedValue != null)
            {
                CategoriaDB categoria = (CategoriaDB)lsvCategories.SelectedValue;

                //llistaPlatsVM.Clear();
                FiltrarPerCategoria(categoria);


                //llistaPlats = PlatDB.GetPlatsPerCategoria(categoria);
                //CarregarPlatsVM();
                //lsvPlats.ItemsSource = llistaPlatsVM;
            }
        }




        //private async void lsvPlats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        private void lsvPlats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedPlat = (PlatViewModel)lsvPlats.SelectedItem;
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
            ctlProgress.IsEnabled = true;
            SelectedPlat.Nom = txbPlatNom.Text;
            SelectedPlat.DescripcioMD = txbPlatDescripcio.Text;
            SelectedPlat.Preu = Convert.ToDecimal(nbbPlatPreu.Value);
            SelectedPlat.Disponible = chkDisponible.IsChecked.Value;
            SelectedPlat.FotoBa = selectedPhotoBa;

            SelectedPlat.PlatOriginal.Update();

            CarregarPlatsDB();
            CarregarPlatsVM();
            //lsvPlats.ItemsSource = llistaPlatsVM;
            SelectedPlat = null;
            lsvCategories.SelectedValue = null;
            ctlProgress.IsEnabled = false;
        }

        // INSERT
        private async void btnInserirPlat_Click(object sender, RoutedEventArgs e)
        {
            //PlatDB nouPlat;
            PlatViewModel nouPlat;
            CategoriaDB catSeleccionada = (CategoriaDB)lsvCategories.SelectedValue;

            // Comprovacions
            if (catSeleccionada == null || catSeleccionada.Codi == CATTOTES)
            {
                var dialog = new MessageDialog("Selecciona una categoria", "Avís");
                await dialog.ShowAsync();
                return;
            }
            if (txbPlatNom.Text == null || txbPlatNom.Text.Length==0)
            {
                var dialog = new MessageDialog("El nom del plat és obligatori", "Avís");
                await dialog.ShowAsync();
                return;
            }

            // Inserir nou plat
            ctlProgress.IsEnabled = true;
            try
            {
                nouPlat = new PlatViewModel();
                nouPlat.PlatOriginal = new PlatDB();
                nouPlat.Nom = txbPlatNom.Text;
                nouPlat.DescripcioMD = txbPlatDescripcio.Text;
                Decimal preu = Convert.ToDecimal(nbbPlatPreu.Value);
                nouPlat.Preu = preu;
                nouPlat.FotoBa = selectedPhotoBa;
                nouPlat.Disponible = chkDisponible.IsChecked.Value ? true : false;
                nouPlat.PlatOriginal.Categoria = (CategoriaDB)lsvCategories.SelectedValue;

                nouPlat.PlatOriginal.Insert();

                CarregarPlatsDB();
                CarregarPlatsVM();

                // Tornar a estat modificació:
                Estat = EstatView.CONSULTA;
                SelectedPlat = null;
                lsvCategories.SelectedValue = null;
                netejarForm();
            } catch (Exception ex)
            {
                var dialog = new MessageDialog("Error: "+ex.Message, "Error");
                await dialog.ShowAsync();
                return;
            }
            finally {
                ctlProgress.IsEnabled = false;
            }
        }

        // Formulari per nou plat
        private void apbAdd_Click(object sender, RoutedEventArgs e)
        {
            Estat = EstatView.NOU;
        }

        // Eliminar plat seleccionat
        private async void apbDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lsvPlats.SelectedItem != null)
            {
                PlatDB p = SelectedPlat.PlatOriginal;

                ctlProgress.IsEnabled = true;
                int liniesImplicades = LiniaComandaDB.GetNumLiniesPerPlat(p);

                if (liniesImplicades == 0)
                {
                    try
                    {
                        // podem eliminar plat
                        p.Delete();
                        // actualitzar taula
                        CarregarPlatsDB();
                        CarregarPlatsVM();
                        NetejarSeleccio();
                    } catch(Exception ex)
                    {
                        var dialog = new MessageDialog("Error: "+ex.Message, "Error");
                        await dialog.ShowAsync();
                    }
                }
                else
                {
                    var dialog = new MessageDialog("No es pot eliminar plat. Hi ha línies de comanda relacionades", "Error");
                    await dialog.ShowAsync();
                }
                ctlProgress.IsEnabled = false;
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
            AppWindow appWindow = null;

            // Create a new window.
            if (AppWindows.Count==0)
            {
                appWindow = await AppWindow.TryCreateAsync();

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
            }
            else
            {
                appWindow = AppWindows.Values.First();
            }
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

    }
}
