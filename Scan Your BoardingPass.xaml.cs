using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFProject.ViewModel;

namespace WPFProject
{
    /// <summary>
    /// Interaction logic for Scan_Your_BoardingPass.xaml
    /// </summary>
    public partial class Scan_Your_BoardingPass : Window
    {
        private System.Timers.Timer tmr;
        List<BitmapImage> lstbitmapImages = new List<BitmapImage>();
        static int imgcount = 0;
        DispatcherTimer timer = new DispatcherTimer();
        string culturecode = "en-US";
        public Scan_Your_BoardingPass(string CultureCode)
        {
            InitializeComponent();
            List<string> lstAdsImages = new List<string>();
            lstAdsImages = Directory.EnumerateFiles(ConfigurationManager.AppSettings["BoardingPassScanImgAdsPth"], "*").ToList();
            foreach (var img in lstAdsImages)
            {
                lstbitmapImages.Add(new BitmapImage(new Uri(img,UriKind.Absolute)));
            }
            LeftAdImg.Source = lstbitmapImages[0];
            RightAdImg.Source = lstbitmapImages[1];
            BottomAdImg.Source = lstbitmapImages[2];
            lstbitmapImages.Clear();
            imgdisplay.Source = new BitmapImage(new Uri(@"/Images/BoardingPassScanScreen/BoardingPass.jpg", UriKind.Relative));
            // imgdisplay.Source = new BitmapImage(new Uri(@"/Images/PassportScan.jpg", UriKind.Relative));
            string currentAssemblyPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("bin"));
            // string currentAssemblyParentPath = System.IO.Path.GetDirectoryName(currentAssemblyPath);
            List<string> lstimages = Directory.EnumerateFiles(ConfigurationManager.AppSettings["CarouselImagesPth"], "*").ToList();


            foreach (var img in lstimages)
            {
                lstbitmapImages.Add(new BitmapImage(new Uri(img)));
            }
            SetLanguage(CultureCode);
            culturecode = CultureCode;
        }
        private void btnClickMe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtsuccessmsg.Visibility = Visibility.Hidden;
                btnClickMe.Visibility = Visibility.Hidden;
                txtscaninprogress.Visibility = Visibility.Visible;
                timer.Interval = TimeSpan.FromSeconds(2);
                timer.Tick += timer_Tick;
                timer.Start();
                imgdisplay.Source = lstbitmapImages[imgcount];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }
        //private void tmr_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    Application.Current.Dispatcher.Invoke(GetValue);
        //}
        //private void GetValue()
        //{
        //    if (tmr.Interval == 0.5 * 1000)
        //    {
        //        txtscaninprogress.Visibility = Visibility.Visible;
        //        tmr.Interval = 5 * 1000;

        //    }
        //    else
        //    {
        //        txtscaninprogress.Visibility = Visibility.Hidden;
        //        // tmr.Interval = 3 * 1000;
        //        txtsuccessmsg.Visibility = Visibility.Visible;
        //        btnNext.IsHitTestVisible = true;

        //        // btnhide_Click;
        //    }
        //}

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            WiFiAccessCode wiFiAccessCode = new WiFiAccessCode();
            wiFiAccessCode.Show();

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (imgcount == lstbitmapImages.Count - 1)
            {
                imgcount = 0;
                timer.Stop();
                txtsuccessmsg.Visibility = Visibility.Visible;
                btnNext.IsHitTestVisible = true;
                txtscaninprogress.Visibility = Visibility.Hidden;
            }
            else
            {
                imgcount++;
                imgdisplay.Source = lstbitmapImages[imgcount];
            }

        }

        private void SetLanguage(string CultureCode)
        {
            try
            {
                string cultureCode = CultureCode;
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
                Application.Current.MainWindow.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);

            }
            catch (Exception ex)
            {
            }
        }
    }
}
