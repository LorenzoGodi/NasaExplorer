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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Media.Imaging;
using NasaExplorer.Classi;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace NasaExplorer
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        
        public HomePage()
        {
            this.InitializeComponent();
            
            CaricaBottone("apod", "Astronomy Picture Of the Day", "https://api.nasa.gov/planetary/apod?api_key=eyzZKG6yo1by0opmheMRLsOtUZnp0yUpKSSI5vzD");

            int ultimoconosciuto = Int32.Parse(LocalStorageUtility.RitornaStringa("ultimoconosciuto"));
            CaricaBottone("rover", "Mars Rover Photos", $"https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?api_key={ApiUtility.APIKEY}&sol={ultimoconosciuto}");

            CaricaBottone("epic", "Earth images from space", "https://epic.gsfc.nasa.gov/epic-archive/jpg/epic_1b_20151031003633.jpg");
            CaricaBottone("neo", "Asteroids and Near Earth Objects", "https://space-facts.com/wp-content/uploads/asteroid-vesta.png");

            string h = LocalStorageUtility.RitornaStringa("ultimoconosciuto");
            ApiUtility.TrovaUltimoSolMarte();
        }

        public async void CaricaBottone(string nome, string info, string urlimg)
        {
            System.Diagnostics.Debug.WriteLine(nome);
            Button btn = new Button()
            {
                Name = nome,
                Tag = nome,
                Margin = new Thickness(0, 0, 0, 15),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 250,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(0),
                Style = (Windows.UI.Xaml.Style)this.Resources["btn_style_1"]
            };

            // Principale
            Grid grd = new Grid()
            {
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 20, 20, 20))
            };

            // Immagini
            Image imgback = new Image()
            {
                Source = new BitmapImage(new Uri(this.BaseUri, "/Immagini/empty.png")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Stretch = Stretch.UniformToFill
            };
            Image img = new Image()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Stretch = Stretch.UniformToFill
            };

            // Testo info
            Grid grdinfo = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 40,
                Style = (Windows.UI.Xaml.Style)this.Resources["grd_style_1"]
            };
            TextBlock textinfo = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(15, 0, 0, 0),
                Text = info
            };

            // Caricamento
            Grid grdcar = new Grid()
            {
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 66, 66, 66))
            };
            StackPanel stackcar = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            ProgressRing ringcar = new ProgressRing()
            {
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                Height = 30,
                Width = 30,
                IsActive = true
            };
            TextBlock textcar = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(15, 0, 0, 0),
                Text = "Caricamento",
                FontSize = 16
            };

            // Creazione XAML
            stackcar.Children.Add(ringcar);
            stackcar.Children.Add(textcar);

            grdcar.Children.Add(stackcar);

            grdinfo.Children.Add(textinfo);

            grd.Children.Add(imgback);
            grd.Children.Add(img);
            grd.Children.Add(grdcar);
            grd.Children.Add(grdinfo);

            btn.Content = grd;

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MainPanel.Children.Add(btn);
            });

            switch (nome)
            {
                case "apod":
                    CaricaImmagine_apod(urlimg, img, grdcar, nome);
                    break;
                case "rover":
                    CaricaImmagine_rover(urlimg, img, grdcar, nome);
                    break;
                case "epic":
                    CaricaImmagine_epic(urlimg, img, grdcar, nome);
                    break;
                case "neo":
                    CaricaImmagine_epic(urlimg, img, grdcar, nome);
                    break;
            }
        }

        public async void CaricaImmagine_apod(string url, Image img, Grid gr, string @as)
        {
            var winrtClient = new Windows.Web.Http.HttpClient();
            string k = await winrtClient.GetStringAsync(new Uri(url));

            JObject cose = JObject.Parse(k);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                img.Source = new BitmapImage(new Uri(cose["url"].ToString(), UriKind.Absolute));
            });
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                gr.Visibility = Visibility.Collapsed;
            });
            System.Diagnostics.Debug.WriteLine("Completato caricamento immagine: " + @as);
        }

        public async void CaricaImmagine_rover(string url, Image img, Grid gr, string @as)
        {
            var winrtClient = new Windows.Web.Http.HttpClient();
            string k = await winrtClient.GetStringAsync(new Uri(url));

            JMars1 jm = JMars1.FromJson(k);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                img.Source = new BitmapImage(new Uri(jm.Photos[0].ImgSrc, UriKind.Absolute));
            });
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                gr.Visibility = Visibility.Collapsed;
            });
            System.Diagnostics.Debug.WriteLine("Completato caricamento immagine: " + @as);
            System.Diagnostics.Debug.WriteLine(jm.Photos[0].ImgSrc);
        }

        public async void CaricaImmagine_epic(string url, Image img, Grid gr, string @as)
        {
            //var winrtClient = new Windows.Web.Http.HttpClient();
            //string k = await winrtClient.GetStringAsync(new Uri(url));

            //JObject cose = JObject.Parse(k);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                img.Source = new BitmapImage(new Uri(url, UriKind.Absolute));
            });
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                gr.Visibility = Visibility.Collapsed;
            });
            System.Diagnostics.Debug.WriteLine("Completato caricamento immagine: " + @as);
        }

        public async void CaricaImmagine_neo(string url, Image img, Grid gr, string @as)
        {
            //var winrtClient = new Windows.Web.Http.HttpClient();
            //string k = await winrtClient.GetStringAsync(new Uri(url));

            //JObject cose = JObject.Parse(k);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                img.Source = new BitmapImage(new Uri(url, UriKind.Absolute));
            });
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                gr.Visibility = Visibility.Collapsed;
            });
            System.Diagnostics.Debug.WriteLine("Completato caricamento immagine: " + @as);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string type = ((Button)sender).Tag.ToString();

            switch (type)
            {
                case "apod":
                    break;
            }
        }

    }
}
