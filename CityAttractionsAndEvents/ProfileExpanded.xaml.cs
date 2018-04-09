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
    /// Interaction logic for ProfileExpanded.xaml
    /// </summary>
    public partial class ProfileExpanded : UserControl
    {
        static int reflectIndex;
        static int picIndex;
        public ProfileExpanded()
        {
            InitializeComponent();
            reflectIndex = 0;
            picIndex = 0;
            reflectLightButton.Click += ReflectLightButton_Click;
            reflectRightButton.Click += ReflectRightButton_Click;
            imageDownButton.Click += ImageDownButton_Click;
            imageUpButton.Click += ImageUpButton_Click;
        }

        private void ImageUpButton_Click(object sender, RoutedEventArgs e)
        {
            picIndex--;
            if (picIndex <= 0)
                this.imageUpButton.Visibility = Visibility.Hidden;
        }

        private void ImageDownButton_Click(object sender, RoutedEventArgs e)
        {
            picIndex++;
            if (picIndex > 0)
                this.imageUpButton.Visibility = Visibility.Visible;
        }

        private void ReflectRightButton_Click(object sender, RoutedEventArgs e)
        {
            reflectIndex++;
            if (reflectIndex > 0)
                this.reflectLightButton.Visibility = Visibility.Visible;
        }

        private void ReflectLightButton_Click(object sender, RoutedEventArgs e)
        {
            reflectIndex--;
            if (reflectIndex <= 0)
                this.reflectLightButton.Visibility = Visibility.Hidden;
        }
    }
}
