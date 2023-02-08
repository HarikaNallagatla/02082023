using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFProject
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Window
    {
        public HomeScreen()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = -1;
            DoubleAnimation dbanimation = new DoubleAnimation();
            dbanimation.To = 1;
            dbanimation.Duration = new Duration(TimeSpan.FromMilliseconds(5000));                        
            Storyboard.SetTargetName(dbanimation, ImgAd1.Name);
            Storyboard.SetTargetProperty(dbanimation, new PropertyPath(ScaleTransform.ScaleXProperty));
            Storyboard sboard = new Storyboard();
            sboard.Children.Add(dbanimation);
            sboard.Begin(ImgAd1);
            

        }
    }
}
