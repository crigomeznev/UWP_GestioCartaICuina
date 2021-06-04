using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace GestioCarta.View
{
    public sealed partial class ImprimirCartaPage : Page
    {
        AppWindow window;
        private string urlJR = "http://51.68.224.27:8080/jasperserver/login.html";
        // ...
        public ImprimirCartaPage()
        {
            this.InitializeComponent();

            Loaded += AppWindowPage_Loaded;
        }

        private void AppWindowPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the reference to this AppWindow that was stored when it was created.
            window = CartaPage.AppWindows[this.UIContext];

            // Set up event handlers for the window.
            //window.Changed += Window_Changed;
            txbURL.Text = urlJR;
            Navegar(urlJR);
        }

        private void Navegar(string url)
        {
            try
            {
                Uri webURL = new Uri(url);
                wbvJasperReports.Navigate(webURL);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void wbvJasperReports_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            ctlProgress.IsActive = true;
        }

        private void wbvJasperReports_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            ctlProgress.IsActive = false;
        }

        private void btnVes_Click(object sender, RoutedEventArgs e)
        {
            String sURL = txbURL.Text;
            if (sURL.IndexOf("http://") == -1) { sURL = "http://" + sURL; }
            Navegar(sURL);
        }
    }
}
