using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFProject
{
    /// <summary>
    /// Interaction logic for GermanLanguage.xaml
    /// </summary>
    /// 
    [ExportMetadata("Culture", "de-DE")]
	[Export(typeof(ResourceDictionary))]
    public partial class GermanLanguage : ResourceDictionary
    {
        public GermanLanguage()
        {
            InitializeComponent();
        }
    }
}
