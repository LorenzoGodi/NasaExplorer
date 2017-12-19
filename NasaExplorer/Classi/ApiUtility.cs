using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NasaExplorer.Classi;

namespace NasaExplorer.Classi
{
    static class ApiUtility
    {
        public const string APIKEY = "eyzZKG6yo1by0opmheMRLsOtUZnp0yUpKSSI5vzD";
        
        public static int TrovaUltimoSolMarte()
        {
            Task.Factory.StartNew(async () => {
                int ultimoconosciuto = Int32.Parse(LocalStorageUtility.RitornaStringa("ultimoconosciuto"));
                string url = $"https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?api_key={APIKEY}&sol={ultimoconosciuto}";
                var winrtClient = new Windows.Web.Http.HttpClient();
                string k = await winrtClient.GetStringAsync(new Uri(url));
                JObject cose = JObject.Parse(k);
            }).Wait();

            return 1;
        }

    }
}
