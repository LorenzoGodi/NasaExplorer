using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NasaExplorer.Classi
{
    static class ApiUtility
    {
        public const string APIKEY = "eyzZKG6yo1by0opmheMRLsOtUZnp0yUpKSSI5vzD";
        
        public static async void TrovaUltimoSolMarte()
        {
            int ultimoconosciuto = Int32.Parse(LocalStorageUtility.RitornaStringa("ultimoconosciuto"));
            bool ripeti = true;

            while (ripeti)
            {
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    ultimoconosciuto++;
                    string url = $"https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?api_key={APIKEY}&sol={ultimoconosciuto}";

                    var stream = await httpClient.GetStreamAsync(url);
                    StreamReader reader = new StreamReader(stream);
                    string jsonString = reader.ReadToEnd();

                    JMars1 jm = JMars1.FromJson(jsonString);
                    if (ripeti = jm.Photos.Count > 1)
                        LocalStorageUtility.SetStringa(ultimoconosciuto.ToString(), "ultimoconosciuto");
                }
            }            
        }


    }
}
