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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CityAttractionsAndEvents
{
    /// <summary>
    /// Interaction logic for GlanceView.xaml
    /// </summary>
    public partial class GlanceView : UserControl
    {
        public GlanceView()
        {
            InitializeComponent();
        }
        public GlanceView(string name, string details, double starRating, double obscurityRating, double price, string imagePath)
        {
            InitializeComponent();
            this.nameText.Text = name;
            this.detailsText.Text = details;
            this.obscValueText.Text = obscurityRating.ToString() + "/100";
            this.priceValueText.Text = "$" + price.ToString();
            if (imagePath != "")
            { 
                List<String> imageFileNames = HelperMethods481.AssemblyManager.GetAllEmbeddedResourceFilesEndingWith(".png", ".jpg");
                Image image = HelperMethods481.AssemblyManager.GetImageFromEmbeddedResources(imagePath);
                this.placeImage.Source = image.Source;
            }
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
