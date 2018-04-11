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
    /// Interaction logic for priceBorderRectControl.xaml
    /// </summary>
    public partial class priceBorderRectControl : UserControl
    {
        public priceBorderRectControl(double linkSize, string name)
        {
            InitializeComponent();
            this.priceLinkRect.Height = linkSize;
            Canvas.SetTop(priceBorderRect, linkSize+this.priceEllipse.Height/2);
            Canvas.SetTop(priceBoxText, linkSize+5 + this.priceEllipse.Height / 2);
            this.priceBoxText.Text = name;
        }
        public priceBorderRectControl()
        {
            InitializeComponent();
            
        }
    }
}
