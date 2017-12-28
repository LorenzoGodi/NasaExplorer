using Newtonsoft.Json.Linq;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace NasaExplorer
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class SpecApodPage : Page
    {
        public SpecApodPage()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();

            var info = new ContinuumNavigationTransitionInfo();

            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;

            //

            this.InitializeComponent();

            //

            Download();
        }

        private async void Download()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                grid_errore.Visibility = Visibility.Collapsed;
                grid_caricamento.Visibility = Visibility.Visible;
            });

            //

            string date = $"&date={mydate.Date.Year}-{mydate.Date.Month}-{mydate.Date.Day}";
            string url = "https://api.nasa.gov/planetary/apod?api_key=eyzZKG6yo1by0opmheMRLsOtUZnp0yUpKSSI5vzD" + date;

            //

            try
            {
                var winrtClient = new Windows.Web.Http.HttpClient();
                string k = await winrtClient.GetStringAsync(new Uri(url));

                JObject cose = JObject.Parse(k);
                string url_img = cose["url"].ToString();

                //

                if(cose["media_type"].ToString() == "image")
                {
                    Uri uri = new Uri(BaseUri, url_img);
                    BitmapImage imgSource = new BitmapImage(uri);

                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        myimage.Source = imgSource;
                        descrizione.Text = cose["explanation"].ToString();
                    });
                }
                else
                {
                    throw new Exception("Impossibile caricare immagine");
                }                
            }
            catch
            {
                grid_errore.Visibility = Visibility.Visible;
            }
            
        }

        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            Download();
        }

        private void myimage_ImageOpened(object sender, RoutedEventArgs e)
        {
            grid_caricamento.Visibility = Visibility.Collapsed;
        }
    }
}
