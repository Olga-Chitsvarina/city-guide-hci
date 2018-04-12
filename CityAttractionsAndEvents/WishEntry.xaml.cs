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
    /// Interaction logic for WishEntry.xaml
    /// </summary>
    public partial class WishEntry : UserControl
    {

        public string Name = "";
        public string ImagePath = "";
        bool isSelected = false;

        public WishEntry()
        {
            InitializeComponent();
        }
        public WishEntry(string name, string imagePath)
        {
            InitializeComponent();
            this.text.Text = name;
            this.Name = name;
            if (imagePath != "")
            {
                ImagePath = imagePath;
                List<String> imageFileNames = HelperMethods481.AssemblyManager.GetAllEmbeddedResourceFilesEndingWith(".png", ".jpg");
                Image image = HelperMethods481.AssemblyManager.GetImageFromEmbeddedResources(imagePath);
                this.image.Source = image.Source;
            }
        }
    }
}
