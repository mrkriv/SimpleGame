using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace OneCGame
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);

            SetLang(OneCGame.Properties.Settings.Default.Lang.Name);
        }

        public static void SetLang(string CultureName)
        {
            var newCulture = new CultureInfo("ru-RU");

            if (Thread.CurrentThread.CurrentUICulture == newCulture)
                return;

            Thread.CurrentThread.CurrentUICulture = newCulture;

            var langDict = Current.Resources.MergedDictionaries.First(e => e.Source.OriginalString.StartsWith("Resources/lang"));
            langDict.Source = new Uri($"Resources/lang.{CultureName}.xaml", UriKind.Relative);

            OneCGame.Properties.Settings.Default.Lang = newCulture;
            OneCGame.Properties.Settings.Default.Save();
        }
    }
}