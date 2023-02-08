using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFProject.ViewModel;
using System.Configuration;
using System.DirectoryServices.ActiveDirectory;

namespace WPFProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int imgcount = 0;
        List<BitmapImage> lstbitmapImages = new List<BitmapImage>();
        MainWindowViewModel vm;
        string cultureCode = "en-US";
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel();
            this.DataContext = vm;
            // string currentAssemblyPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("bin"));
            // string currentAssemblyParentPath = System.IO.Path.GetDirectoryName(currentAssemblyPath);
            #region
            List<string> lstAdsImages = new List<string>();
            lstAdsImages = Directory.EnumerateFiles(ConfigurationManager.AppSettings["MainWindowImgAdsPth"], "*").ToList();
            foreach (var img in lstAdsImages)
            {
                lstbitmapImages.Add(new BitmapImage(new Uri(img, UriKind.Absolute)));
            }
            LeftAdImg.Source = lstbitmapImages[0];
            RightAdImg.Source = lstbitmapImages[1];
            BottomAdImg.Source = lstbitmapImages[2];
            #endregion

            List<string> lstimages = new List<string>(); ; //Directory.EnumerateFiles(System.IO.Path.Combine(currentAssemblyPath, System.IO.Path.Combine("Images", "Country Flags")), "*.png").ToList();
            lstimages = Directory.EnumerateFiles(ConfigurationManager.AppSettings["CountryFlagsImgPth"], "*").ToList();
            lstbitmapImages.Clear();
            foreach (var img in lstimages)
            {
                lstbitmapImages.Add(new BitmapImage(new Uri(img)));
            }
            for (int i = 0; i <= 4; i++)
            {
                Button imgbtn = new Button();
                imgbtn.Click += Button_Click;
                Image img = new Image();
                img.Source = lstbitmapImages[i];
                // img.Margin = new Thickness(0, 0, 15, 0);
                imgbtn.Content = img;
                imgbtn.Margin = new Thickness(0, 0, 15, 0);
                spcountyflags.Children.Add(imgbtn);
                imgcount = i;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cultureCode = "de-DE";
            CultureInfo cultureInfo = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            var dictionary = (from d in BaseModel.Instance.ImportCatalog.ResourceDictionaryList
                              where d.Metadata.ContainsKey("Culture")
                              && d.Metadata["Culture"].ToString().Equals(cultureCode)
                              select d).FirstOrDefault();
            if (dictionary != null && dictionary.Value != null)
            {
                this.Resources.MergedDictionaries.Clear();
                this.Resources.MergedDictionaries.Add(dictionary.Value);
            }


            // cultureInfo = new CultureInfo(cultureCode);
            //Thread.CurrentThread.CurrentCulture = cultureInfo;
            //Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Application.Current.MainWindow.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);

        }
        private void btnstart_Click(object sender, RoutedEventArgs e)
        {
            Scan_Your_Passport scan_Your_Passport = new Scan_Your_Passport(cultureCode);
            scan_Your_Passport.Show();
        }
        private void btnloadnext_Click(object sender, RoutedEventArgs e)
        {

            spcountyflags.Children.RemoveAt(0);
            btnloadprevious.IsEnabled = true;
            for (int i = imgcount + 1; spcountyflags.Children.Count <= 4; i++)
            {
                Button imgbtn = new Button();
                imgbtn.Click += Button_Click;
                Image img = new Image();
                img.Source = lstbitmapImages[i];
                //  img.Margin = new Thickness(0, 0, 20, 0);
                imgbtn.Content = img;
                imgbtn.Margin = new Thickness(0, 0, 15, 0);
                img.Focus();

                spcountyflags.Children.Add(imgbtn);
                // spcountyflags.Children.Add(img);
                imgcount = i;
            }
        }
        private void btnloadprevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                spcountyflags.Children.Clear();
                for (int i = imgcount - 5; spcountyflags.Children.Count <= 4; i++)
                {
                    if (i == 0)
                    {
                        btnloadprevious.IsEnabled = false;
                    }
                    Button imgbtn = new Button();
                    imgbtn.Click += Button_Click;
                    Image img = new Image();
                    img.Source = lstbitmapImages[i];
                    //img.Margin = new Thickness(0, 0, 20, 0);
                    imgbtn.Content = img;
                    imgbtn.Margin = new Thickness(0, 0, 15, 0);
                    //  spcountyflags.Children.Add(img);
                    spcountyflags.Children.Add(imgbtn);
                    imgcount = i;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reached to the first image");
                btnloadprevious.IsEnabled = false;
            }

        }
    }
}
