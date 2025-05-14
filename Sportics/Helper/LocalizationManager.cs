using System;
using System.Linq;
using System.Windows;

namespace Sportics.Helper
{
    public static class LocalizationManager
    {
        public static void ChangeCulture(string cultureCode)
        {
            ResourceDictionary dict = new ResourceDictionary();

            switch (cultureCode)
            {
                case "RU":
                {
                    dict.Source = new Uri("Resources/Strings.ru-RU.xaml", UriKind.Relative);
                    break;
                }
                default:
                {
                    dict.Source = new Uri("Resources/Strings.en-US.xaml", UriKind.Relative);
                    break;
                }
            }

            // Удаление старого словаря
            ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                           where d.Source != null && d.Source.OriginalString.StartsWith("Resources/Strings.")
                           select d).FirstOrDefault();

            if (oldDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);
            }

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }

}
