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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CityAttractionsAndEvents
{
    /// <summary>
    /// Interaction logic for PlacePopout.xaml
    /// </summary>
    public partial class PlacePopout : UserControl
    {

        public PlacePopout()
        {
            InitializeComponent();
            Storyboard sb = this.FindResource("disappearBoard") as Storyboard;
            sb.Completed += sbCompleted;
        }

        private void sbCompleted(object sender, EventArgs e)
        {
            ((Canvas)this.Parent).Children.Remove(this);
        }

        private void onCloseButton(object sender, RoutedEventArgs e)
        {

        }

        public PlacePopout(string name, double progress, string details, Boolean upView)
        {
            InitializeComponent();
            this.PlaceName.Text = name;
            this.progressBar.Value = progress;
            this.Details.Text = details;
            if (upView)
                this.DownCanvas.Visibility = Visibility.Hidden;
            else
                this.UpCanvas.Visibility = Visibility.Hidden;
        }
    }
}
