using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        string imagepath;

        public GlanceView(string name, string details, double starRating, double obscurityRating, double price, string imagePath)
        {
            InitializeComponent();
            this.imagepath = imagePath;
            this.nameText.Text = name;
            this.detailsText.Text = details;
            this.obscValueText.Text = obscurityRating.ToString() + "/100";
            this.priceValueText.Text = "$" + price.ToString();
            this.wishlistImage.MouseDown += WishlistImage_MouseDown;
            this.blacklistImage.MouseDown += BlacklistImage_MouseDown;
            if (imagePath != "")
            {
                List<String> imageFileNames = HelperMethods481.AssemblyManager.GetAllEmbeddedResourceFilesEndingWith(".png", ".jpg");
                Image image = HelperMethods481.AssemblyManager.GetImageFromEmbeddedResources(imagePath);
                this.placeImage.Source = image.Source;
            }
        }

        private void BlacklistImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image image = HelperMethods481.AssemblyManager.GetImageFromEmbeddedResources("heart1.png");
            this.blacklistImage.Source = image.Source;
            System.Threading.Thread.Sleep(600);
            //image = HelperMethods481.AssemblyManager.GetImageFromEmbeddedResources("heart.png");
            //this.blacklistImage.Source = image.Source;
            ((MainWindow)((Canvas)((Canvas)((ScrollViewer)((DockPanel)((StackPanel)this.Parent).Parent).Parent).Parent).Parent).Parent).addToBlacklist(this.nameText.Text);
        }

        private void WishlistImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WishEntry wishEntry = new WishEntry(this.nameText.Text, this.imagepath) { };

            ((MainWindow)((Canvas)((Canvas)((ScrollViewer)((DockPanel)((StackPanel)this.Parent).Parent).Parent).Parent).Parent).Parent).wishStack.Children.Add(wishEntry);

        }

    }
    
}
