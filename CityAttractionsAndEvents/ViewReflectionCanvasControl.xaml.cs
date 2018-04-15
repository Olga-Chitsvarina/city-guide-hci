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
    /// Interaction logic for ViewReflectionCanvasControl.xaml
    /// </summary>
    public partial class ViewReflectionCanvasControl : UserControl
    {
        public ViewReflectionCanvasControl()
        {
            InitializeComponent();
            this.viewReflectionsCloseButt.Click += ViewReflectionsCloseButt_Click;

            this.Margin = new Thickness(260, 45, 0, 0);


            for (int i = 0; i < 8; i++)
            {
                ProfileExpanded pe = new ProfileExpanded();
                pe.reflectionsTitle.Visibility = Visibility.Hidden;
                pe.viewAllButton.Visibility = Visibility.Hidden;
                pe.reflectRightButton.Visibility = Visibility.Hidden;
                pe.Height = 285;
                pe.Width = 680;
                pe.HorizontalAlignment = HorizontalAlignment.Center;
                Grid tempGrid = new Grid();
                tempGrid.Width = 710;
                tempGrid.Background = new SolidColorBrush(Color.FromRgb(244, 244, 244));
                tempGrid.HorizontalAlignment = HorizontalAlignment.Center;
                tempGrid.VerticalAlignment = VerticalAlignment.Top;
                tempGrid.Children.Add(pe);
                reflectionDock.Children.Add(tempGrid);
            }

            //this.viewReflectionsCloseButt.Click += ViewReflectionsCloseButt_Click;
        }
    

        private void ViewReflectionsCloseButt_Click(object sender, RoutedEventArgs e)
        {
            ((Panel)this.Parent).Children.Remove(this);
        }
    }
}
