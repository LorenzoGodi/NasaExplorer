﻿using Newtonsoft.Json.Linq;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace NasaExplorer
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class SubHome_neo_Page : Page
    {
        public SubHome_neo_Page()
        {
            this.InitializeComponent();
            //JJJ();
            Download("https://space-facts.com/wp-content/uploads/asteroid-vesta.png");
        }

        private async void JJJ()
        {
            const string starting_url = "";
            var winrtClient = new Windows.Web.Http.HttpClient();
            string k = await winrtClient.GetStringAsync(new Uri(starting_url));
            JObject cose = JObject.Parse(k);

            //

            string url_img = cose["url"].ToString();

            //

            Download(url_img);
        }

        private async void Download(string dwn_url)
        {
            Uri uri = new Uri(BaseUri, dwn_url);
            BitmapImage imgSource = new BitmapImage(uri);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Img.Source = imgSource;
            });
        }

        private void Img_ImageOpened(object sender, RoutedEventArgs e)
        {
            grid_caricamento.Visibility = Visibility.Collapsed;
        }
    }
}
