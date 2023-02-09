using Desko.FullPage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPFProject.ViewModel;
//using Emgu.CV;
//using Emgu.CV.OCR;
//using Emgu.CV.Structure;
//using OpenCvSharp;
using Window = System.Windows.Window;

namespace WPFProject
{
    /// <summary>
    /// Interaction logic for Scan_Your_Passport.xaml
    /// </summary>
    public partial class Scan_Your_Passport : Window
    {
        private System.Timers.Timer tmr;
        List<BitmapImage> lstbitmapImages = new List<BitmapImage>();
        static int imgcount = 0;
        DispatcherTimer timer = new DispatcherTimer();
        string culturecode = "en-US";
        public Scan_Your_Passport(string CultureCode)
        {
            InitializeComponent();

            List<string> lstAdsImages = new List<string>();
            lstAdsImages = Directory.EnumerateFiles(ConfigurationManager.AppSettings["PassportScanImgAdsPth"], "*").ToList();
            foreach (var img in lstAdsImages)
            {
                lstbitmapImages.Add(new BitmapImage(new Uri(img, UriKind.Absolute)));
            }
            LeftAdImg.Source = lstbitmapImages[0];
            RightAdImg.Source = lstbitmapImages[1];
            BottomAdImg.Source = lstbitmapImages[2];
            lstbitmapImages.Clear();
            imgdisplay.Source = new BitmapImage(new Uri(@"/Images/PassportScanScreen/PassportScan.jpg", UriKind.Relative));
            string currentAssemblyPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("bin"));
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
                imgdisplay.Visibility = Visibility.Hidden;
                mediaelement.Visibility = Visibility.Visible;
                string currentAssemblyPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("bin"));
                mediaelement.Source = (new Uri(@"D:\VS Projects\WPFProject\WPFProject\Images\PassportScanScreen\Ads\Videos\AdVideo.mp4", UriKind.Absolute));
                scan();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Scan_Your_BoardingPass scan_Your_BoardingPass = new Scan_Your_BoardingPass(culturecode);
            scan_Your_BoardingPass.Show();
        }
        private void mediaelement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaelement.Position = TimeSpan.FromSeconds(1);
            // txtsuccessmsg.Visibility = Visibility.Visible;
            btnNext.IsHitTestVisible = true;
        }
        //private void timer_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (imgcount == lstbitmapImages.Count - 1)
        //        {
        //            imgcount = 0;
        //            timer.Stop();
        //            txtsuccessmsg.Visibility = Visibility.Visible;
        //            btnNext.IsHitTestVisible = true;
        //            txtscaninprogress.Visibility = Visibility.Hidden;
        //        }
        //        else
        //        {
        //            imgcount++;
        //            imgdisplay.Source = lstbitmapImages[imgcount];
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //    }
        //}
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

        //void updateDeviceInfo()
        //{

        //    bool connected = Api.IsDeviceConnected();

        //    List<SystemInfoEntry> list = new List<SystemInfoEntry>();
        //    list.Add(new SystemInfoEntry("API", Api.GetProperty(PropertyKey.ApiVersionString, "n/a")));
        //    list.Add(new SystemInfoEntry("DLL", String.Format("{0} ({1} {2})",
        //        Api.GetProperty(PropertyKey.DllVersionString, "?"),
        //        Api.GetProperty(PropertyKey.DllCompileDate, "?"),
        //        Api.GetProperty(PropertyKey.DllCompileTime, "?")
        //        )));


        //    if (connected)
        //    {
        //        list.Add(new SystemInfoEntry("Firmware", String.Format("{0} ({1} {2})",
        //            Api.GetProperty(PropertyKey.DeviceFirmwareVersionString, "n/a"),
        //            Api.GetProperty(PropertyKey.DeviceFirmwareDate, "n/a"),
        //            Api.GetProperty(PropertyKey.DeviceFirmwareTime, "n/a")
        //            )));

        //        list.Add(new SystemInfoEntry("S/N", Api.GetProperty(PropertyKey.DeviceSerialNumber, "n/a")));
        //        list.Add(new SystemInfoEntry("Production", Api.GetProperty(PropertyKey.DeviceProductionId, "n/a")));
        //        list.Add(new SystemInfoEntry("PCB", Api.GetProperty(PropertyKey.DevicePcbRevision, "n/a")));
        //        list.Add(new SystemInfoEntry("USB VID/PID", String.Format("{0}/{1}",
        //            Api.GetProperty(PropertyKey.DeviceVid, (uint)0).ToString("X4"),
        //            Api.GetProperty(PropertyKey.DevicePid, (uint)0).ToString("X4"))));
        //        list.Add(new SystemInfoEntry("Illumination",
        //            String.Format(
        //                "{0}.{1}/{2}",
        //                Api.GetProperty(PropertyKey.DeviceIlluminationGenerationVerbose, "?"),
        //                Api.GetProperty(PropertyKey.DeviceIlluminationRevisionVerbose, "?"),
        //                Api.GetProperty(PropertyKey.DeviceIlluminationVariantVerbose, "?")
        //            )));
        //        list.Add(new SystemInfoEntry("Barcode on PC", Api.GetProperty(PropertyKey.DeviceSupportBarcodeOnPc, (uint)0) == 0 ? "No" : "Yes"));

        //        DeviceInfo info = Api.GetDeviceInfo();
        //        foreach (DeviceInfoFlags flag in Enum.GetValues(typeof(DeviceInfoFlags)))
        //        {
        //            if (flag != DeviceInfoFlags.None)
        //                if (0 != (flag & info.Features))
        //                {
        //                    list.Add(new SystemInfoEntry(flag.ToString(), "YES"));
        //                }
        //                else
        //                {
        //                    list.Add(new SystemInfoEntry(flag.ToString(), "NO"));
        //                }
        //        }
        //    }

        //    buttonQuickBarcode.Enabled = (Api.GetProperty(PropertyKey.DeviceSupportBarcodeOnPc, (uint)0) != 0);

        //    dataGridViewSystemInfo.DataSource = list;
        //    dataGridViewSystemInfo.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    dataGridViewSystemInfo.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //    dataGridViewSystemInfo.ClearSelection();
        //}

        void scan()
        {
            try
            {
                bool plugged = Api.IsDevicePlugged();
                bool connected = Api.IsDeviceConnected();
                if (plugged)
                {
                    if (connected)
                    {
                        Tools.HandleApiExceptions(delegate ()
                        {
                            Api.Scan();
                            txtsuccessmsg.Visibility = Visibility.Visible;
                            onScanOcr();
                        });
                    }
                    else
                    {
                        MessageBox.Show("The scanner device is not connected.Please check .");
                    }
                }
                else
                {
                    MessageBox.Show("The scanner device is not plugged.Please check");
                }
               

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
        void onScanOcr()
        {
            try
            {
                string mrz = Api.GetOcrPc();
                if (mrz != null)
                {
                    txtsuccessmsg.Text = "OCR:\r\n" + mrz.Replace("\r", "\r\n");
                }
                Bitmap Image = Api.GetImage(
                            LightSource.Infrared,
                            ImageClipping.None);
                mediaelement.Source = ConvertBitmap(Image).UriSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
        public BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
    }
}
