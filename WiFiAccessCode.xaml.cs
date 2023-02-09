using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;
using System.Threading;
using WPFProject.ViewModel;
using System.Windows.Markup;
using System.Printing;
using System.Windows.Documents;
using System.Windows.Controls;
using Custom.CuCustomWndAPI;
using System.Windows.Media.Imaging;
using System.Configuration;
using System.IO;
using System.Security.Policy;
using IronBarCode;
using System.Net.Http;
using System.Windows.Media;

namespace WPFProject
{
    /// <summary>
    /// Interaction logic for WiFiAccessCode.xaml
    /// </summary>
    public partial class WiFiAccessCode : Window
    {
        string accesscode = string.Empty;
        //CuCustomWndAPIWrap customWndAPIWrap = null;
        //CuCustomWndDevice dev = null;

        //private static Thread threadManageStatus = null;      //Get Status Thread
        //private static EventWaitHandle waitHandleThreadManageStatusExit = new EventWaitHandle(false, EventResetMode.ManualReset);
        //private static EventWaitHandle waitHandleThreadManageStatusStopped = new EventWaitHandle(false, EventResetMode.ManualReset);
        List<BitmapImage> lstbitmapImages = new List<BitmapImage>();
        public WiFiAccessCode()
        {
            //List<string> lstAdsImages = new List<string>();
            //lstAdsImages = Directory.EnumerateFiles(ConfigurationManager.AppSettings["WiFiAccessCodeImgAdsPth"], "*").ToList();
            //foreach (var img in lstAdsImages)
            //{
            //    lstbitmapImages.Add(new BitmapImage(new Uri(img, UriKind.Absolute)));
            //}
            //LeftAdImg.Source = lstbitmapImages[0];
            //RightAdImg.Source = lstbitmapImages[1];
            //BottomAdImg.Source = lstbitmapImages[2];
            //lstbitmapImages.Clear();
            InitializeComponent();
            SetLanguage("en-US");

            Random random = new Random();
             accesscode = (random.Next(100000, 999999)).ToString();
            //HttpClientHandler clientHandler = new HttpClientHandler();
            //clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, SslPolicyErrors) => { return true; };
            //clientHandler.SslProtocols = SslProtocols.None;
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("https://localhost:44318/api/Home/GetPassCode");
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HttpResponseMessage response = client.GetAsync("https://localhost:44318/api/Home/GetPassCode").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    MessageBox.Show(response.Content.ToString());
            //    randomnumber = response.Content.ToString();
            //}
            //else
            //{
            //    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            //}
            txtWifiCode.Visibility = Visibility.Visible;
            txtWifiCode.Text = txtWifiCode.Text + "\n" + accesscode;
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
        private async void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            ImageSource imgsrc = null;
            try
            {
                #region QRCodegenerator
                //GeneratedBarcode Qrcode = IronBarCode.QRCodeWriter.CreateQrCode(accesscode);
                //Qrcode.SaveAsPng("QrCode.png");

                using (HttpClient client = new HttpClient())
                {
                    string url= "https://api.qrserver.com/v1/create-qr-code/?data=" + accesscode + "&size=100x100";
                    var imagebytearray = await client.GetByteArrayAsync(url);
                    imgsrc = ByteToImage(imagebytearray);
                }
                #endregion

                #region Printingtheaccsscode
                //PrintDialog printdialog = new PrintDialog();
                //StringBuilder sb =
                //    new StringBuilder();
                //sb.AppendLine("Chatrapathi Shivaji Maharaj ");
                //sb.AppendLine("International Airport");
                //sb.AppendLine("Mumbai,India.");
                //StringBuilder stringbuilder = new StringBuilder();
                //string imgurl = @"/Images/Wifiaccesscodescreen/wifi.jpg";
                //stringbuilder.AppendLine("<img src='" + imgurl + "'  style='width: 30px; height: 30px' />");
                //sb.Append(" <b> Free WiFi</b>");
                //sb.AppendLine("              <b>Access Code</b>");
                //sb.AppendLine("Network:WiFi");
                //sb.AppendLine("User:ABCD");
                //sb.Append("<img src='" + imgsrc + "'  style='width: 30px; height: 30px' />");
                //Bold bold = new Bold();
                //bold.Inlines.Add("Password:" + accesscode);
                //StringBuilder sb1 = new StringBuilder();
                //sb1.AppendLine("---------------------------");
                //sb1.AppendLine("Issued On:");
                //sb1.Append("            " + DateTime.Now.ToString());
                //sb1.AppendLine("Valid Till:            24 hrs");
                //sb1.AppendLine("Registered User:            Harika");
                //sb1.AppendLine();
                //sb1.AppendLine("For Terms & Conditions of use ");
                //sb1.AppendLine("please check back side.");
                //FlowDocument flowdocument = new FlowDocument();
                //flowdocument.Name = "flowdoc";
                //Section sec = new Section();
                //Paragraph p1 = new Paragraph();
                //p1.Inlines.Add(sb.ToString());
                //p1.Inlines.Add(bold);
                //Paragraph p2 = new Paragraph();
                //p2.Inlines.Add(sb1.ToString());
                //sec.Blocks.Add(p1);
                //sec.Blocks.Add(p2);
                //flowdocument.Blocks.Add(sec);
                //IDocumentPaginatorSource idpsource = flowdocument;
                //printdialog.PrintDocument(idpsource.DocumentPaginator, string.Empty);
                #endregion
                #region Printing
                string imgurl = @"/Images/Wifiaccesscodescreen/wifi.jpg";
                Image imgwifi = new Image();
                imgwifi.Source = new BitmapImage(new Uri(imgurl,UriKind.Relative));
                imgwifi.Height = 30;
                imgwifi.Width = 30;
                Image imgqrcode = new Image();
                imgqrcode.Width = 100;
                imgqrcode.Height = 100;
                imgqrcode.Margin = new Thickness(95,0,20,0);
                imgqrcode.Source = imgsrc;
                PrintDialog printdialog = new PrintDialog();
                StringBuilder sb =
                    new StringBuilder();
                FlowDocument flowdocument = new FlowDocument();
                flowdocument.Name = "flowdoc";
                Section sec = new Section();
                Paragraph p1 = new Paragraph();
                Paragraph p2 = new Paragraph();
                StringBuilder sb2 = new StringBuilder();
                StringBuilder sb3 = new StringBuilder();

                sb.AppendLine("           Chatrapathi Shivaji Maharaj ");
                sb.AppendLine("              International Airport");
                sb.AppendLine("                   Mumbai,India.");
                p1.Inlines.Add(sb.ToString());
                p1.Inlines.Add(imgwifi);
                sb2.AppendLine();
                sb2.AppendLine("                    Free WiFi");
                sb2.AppendLine("                     Access Code");
                sb2.AppendLine("Network:  WiFi");
                sb2.AppendLine("User:   ABCD");
                sb2.AppendLine();
                p1.Inlines.Add(sb2.ToString());
                p1.Inlines.Add(imgqrcode);
                Bold bold = new Bold();
                bold.Inlines.Add("              Password:" + accesscode);
                sb3.AppendLine();
                p1.Inlines.Add(sb3.ToString());
                p1.Inlines.Add(bold);
                StringBuilder sb1 = new StringBuilder();
                sb1.AppendLine("--------------------------------------------");
                sb1.Append("Issued On:");
                sb1.AppendLine("   " + DateTime.Now.ToString());
                sb1.AppendLine("Valid Till:    24 hrs");
                sb1.AppendLine("Registered User:  Harika");
                sb1.AppendLine();
                sb1.AppendLine("For Terms & Conditions of use ");
                sb1.AppendLine("please check back side.");
                p2.Inlines.Add(sb1.ToString());
                sec.Blocks.Add(p1);
                sec.Blocks.Add(p2);
                flowdocument.Blocks.Add(sec);
                IDocumentPaginatorSource idpsource = flowdocument;
                printdialog.PrintDocument(idpsource.DocumentPaginator, string.Empty);


                //customWndAPIWrap = new CuCustomWndAPIWrap(CuCustomWndAPIWrap.CcwLogVerbosity.CCW_LOG_DEEP_DEBUG, null);

                ////Init the library
                //customWndAPIWrap.InitLibrary();
                //dev = customWndAPIWrap.OpenInstalledPrinter("CUSTOM K80");

                //If no text, exit    
                //if (String.IsNullOrEmpty(strTextToPrint))
                //    return;
                //PrintFontSettings pfs = new PrintFontSettings();
                //pfs.Emphasized = true;
                //dev.PrintText(strTextToPrint, pfs, true);

                #endregion
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

    }
}
