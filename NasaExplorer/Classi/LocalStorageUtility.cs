using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaExplorer.Classi
{
    static class LocalStorageUtility
    {
        public static void SetStringa(string data, string nome)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values[nome] = data;
        }

        public static string RitornaStringa(string nome)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            return localSettings.Values[nome]?.ToString() ?? "";
        }
    }
}