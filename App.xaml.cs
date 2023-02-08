using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WPFProject.ViewModel;

namespace WPFProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SetImportCatalog();
            SetLanguage();
        }

        private void SetImportCatalog()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                DirectoryCatalog catalog = new DirectoryCatalog(path);
                CompositionContainer container = new CompositionContainer(catalog);
                container.ComposeParts(BaseModel.Instance.ImportCatalog);

            }
            catch (Exception ex)
            {

            }
        }

        private void SetLanguage()
        {
            try
            {
                string cultureCode = "en-US";
                CultureInfo cultureInfo = new CultureInfo(cultureCode);
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                var dictionary = (from d in BaseModel.Instance.ImportCatalog.ResourceDictionaryList
                                  where d.Metadata.ContainsKey("Culture")
                                  && d.Metadata["Culture"].ToString().Equals(cultureCode)
                                  select d).FirstOrDefault();
                if (dictionary != null && dictionary.Value != null)
                {
                    this.Resources.MergedDictionaries.Add(dictionary.Value);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
