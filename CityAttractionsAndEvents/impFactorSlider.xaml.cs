using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for impFactorSlider.xaml
    /// </summary>
    public partial class impFactorSlider : UserControl
    {
        priceBorderRectControl selection = null;
        private double minimum = 0;
        private double maximum = 100.00;

        private double currentPrice;
        private double currentObsc;
        private double currentStar;
        priceBorderRectControl priceRectControl;
        priceBorderRectControl obscRectControl;
        priceBorderRectControl starRectControl;


        public impFactorSlider()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            this.Loaded += OnLoaded;

        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.priceRectControl = new priceBorderRectControl(35, "Price");
            Canvas.SetTop(priceRectControl, 6);
            Canvas.SetLeft(priceRectControl, 30);
            Canvas.SetZIndex(priceRectControl, 2);
            this.impCanvas.Children.Add(priceRectControl);
            this.obscRectControl = new priceBorderRectControl(70, "Obscurity Rating");
            Canvas.SetTop(obscRectControl, 6);
            Canvas.SetLeft(obscRectControl, 25);
            Canvas.SetZIndex(obscRectControl, 1);
            this.impCanvas.Children.Add(obscRectControl);
            this.starRectControl = new priceBorderRectControl(105, "Star Rating");
            Canvas.SetTop(starRectControl, 6);
            Canvas.SetLeft(starRectControl, 40);
            Canvas.SetZIndex(starRectControl, 0);
            this.impCanvas.Children.Add(starRectControl);
            priceRectControl.MouseDown += onClick;
            obscRectControl.MouseDown += onClick;
            starRectControl.MouseDown += onClick;
            Window.GetWindow(this).MouseMove += OnWindowMouseMove;
            Window.GetWindow(this).MouseLeftButtonUp += onBoundClickReleased;
        }

        private void onBoundClickReleased(object sender, MouseButtonEventArgs e)
        {
            this.selection = null;
            if (this.currentPrice > 100)
            {
                this.currentPrice = 100.00;
                updatePosition();
            }
            if (this.currentPrice < 0)
            {
                this.currentPrice = 0.0;
                updatePosition();
            }
            if (this.currentObsc > 100)
            {
                this.currentObsc = 100.00;
                updatePosition();
            }
            if (this.currentObsc < 0)
            {
                this.currentObsc = 0.0;
                updatePosition();
            }
            if (this.currentStar > 100)
            {
                this.currentStar = 100.00;
                updatePosition();
            }
            if (this.currentStar < 0)
            {
                this.currentStar = 0.0;
                updatePosition();
            }
        }

        private void OnWindowMouseMove(object sender, MouseEventArgs e)
        {
            if (selection != null)
            {
                double pricePosition = Canvas.GetLeft(priceRectControl);
                this.currentPrice = Utils.Map(pricePosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
                double starPosition = Canvas.GetLeft(starRectControl);
                this.currentStar = Utils.Map(starPosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
                double obscPosition = Canvas.GetLeft(obscRectControl);
                this.currentObsc = Utils.Map(obscPosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
            }
        }
        private void onClick(object sender, MouseButtonEventArgs e)
        {
            this.selection = sender as priceBorderRectControl;
        }

        private void updatePosition()
        {
            double pricePosition = Canvas.GetLeft(priceRectControl);
            this.currentPrice = Utils.Map(pricePosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
            double starPosition = Canvas.GetLeft(starRectControl);
            this.currentStar = Utils.Map(starPosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
            double obscPosition = Canvas.GetLeft(obscRectControl);
            this.currentObsc = Utils.Map(obscPosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
            if (this.currentPrice > 100)
            {
                this.currentPrice = 100.00;
                pricePosition = Utils.Map(this.currentPrice, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(priceRectControl, pricePosition);
            }
            if (this.currentPrice < 0)
            {
                this.currentPrice = 0.0;
                pricePosition = Utils.Map(this.currentPrice, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(priceRectControl, pricePosition);
            }
            if (this.currentObsc > 100)
            {
                this.currentObsc = 100.00;
                obscPosition = Utils.Map(this.currentObsc, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(obscRectControl, obscPosition);
            }
            if (this.currentObsc < 0)
            {
                this.currentObsc = 0.0;
                obscPosition = Utils.Map(this.currentObsc, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(obscRectControl, obscPosition);
            }
            if (this.currentStar > 100)
            {
                this.currentStar = 100.00;
                starPosition = Utils.Map(this.currentStar, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(starRectControl, starPosition);
            }
            if (this.currentStar < 0)
            {
                this.currentStar = 0.0;
                starPosition = Utils.Map(this.currentStar, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(starRectControl, starPosition);
            }
        }

        public event EventHandler<RangeSliderChangedEventArgs> Changed;

        private void RaiseChanged(RangeSliderChangedEventArgs e)
        {
            if (this.Changed != null)
            {
                Changed(this, e);
            }
        }
    }
}
