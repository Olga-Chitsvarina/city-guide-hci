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

        private double HALFRECT = 95 / 2;

        private double maxLeft;
        private double minLeft;


        public impFactorSlider()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            this.Loaded += OnLoaded;

            maxLeft = this.sliderRect.Width + Canvas.GetLeft(this.sliderRect) - HALFRECT;
            minLeft = Canvas.GetLeft(this.sliderRect) - HALFRECT;

        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.priceRectControl = new priceBorderRectControl(35, "Price");
            Canvas.SetTop(this.priceRectControl, 6);
            Canvas.SetLeft(this.priceRectControl, 30);
            Canvas.SetZIndex(this.priceRectControl, 2);
            this.priceRectControl.mapEllipseSize(30, minLeft, maxLeft);
            this.impCanvas.Children.Add(this.priceRectControl);
            this.obscRectControl = new priceBorderRectControl(70, "Obscurity Rating");
            Canvas.SetTop(this.obscRectControl, 6);
            Canvas.SetLeft(this.obscRectControl, 25);
            Canvas.SetZIndex(this.obscRectControl, 1);
            this.obscRectControl.mapEllipseSize(25, minLeft, maxLeft);
            this.impCanvas.Children.Add(obscRectControl);
            this.starRectControl = new priceBorderRectControl(105, "Star Rating");
            Canvas.SetTop(this.starRectControl, 6);
            Canvas.SetLeft(this.starRectControl, 40);
            Canvas.SetZIndex(this.starRectControl, 0);
            this.starRectControl.mapEllipseSize(40, minLeft, maxLeft);
            this.impCanvas.Children.Add(this.starRectControl);
            this.priceRectControl.MouseDown += onClick;
            this.obscRectControl.MouseDown += onClick;
            this.starRectControl.MouseDown += onClick;
            Window.GetWindow(this).MouseMove += OnWindowMouseMove;
            Window.GetWindow(this).MouseLeftButtonUp += onBoundClickReleased;
            Window.GetWindow(this).MouseEnter += ImpFactorSlider_MouseEnter;
        }

        private void ImpFactorSlider_MouseEnter(object sender, MouseEventArgs e)
        {
            doneSelection();
        }

        private void onBoundClickReleased(object sender, MouseButtonEventArgs e)
        {
            doneSelection();
        }

        private void doneSelection() {
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

        private double ClampLeft(double val) {
            double retVal = val;
            if (retVal < minLeft)
                retVal = minLeft;
            if (retVal > maxLeft)
                retVal = maxLeft;
            return retVal;
        }

        private void OnWindowMouseMove(object sender, MouseEventArgs e)
        {
            if (selection != null)
            {
                Canvas.SetLeft(selection, ClampLeft(e.GetPosition(this.impCanvas).X - HALFRECT));
                selection.mapEllipseSize(ClampLeft(e.GetPosition(this.impCanvas).X - HALFRECT), minLeft, maxLeft);

                double pricePosition = Canvas.GetLeft(this.priceRectControl);
                this.currentPrice = Utils.Map(pricePosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
                double starPosition = Canvas.GetLeft(this.starRectControl);
                this.currentStar = Utils.Map(starPosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
                double obscPosition = Canvas.GetLeft(this.obscRectControl);
                this.currentObsc = Utils.Map(obscPosition, 0, this.sliderRect.Width, this.minimum, this.maximum);

                this.currentObsc += HALFRECT;
                this.currentPrice += HALFRECT;
                this.currentStar += HALFRECT;
            }
        }
        private void onClick(object sender, MouseButtonEventArgs e)
        {
            this.selection = sender as priceBorderRectControl;
        }

        private void updateWindowWithPositions()
        {
            (Application.Current.MainWindow as MainWindow).refreshPriorities(this.currentObsc, this.currentPrice, this.currentStar);
        }

        private void updatePosition()
        {
            double pricePosition = Canvas.GetLeft(this.priceRectControl);
            this.currentPrice = Utils.Map(pricePosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
            double starPosition = Canvas.GetLeft(this.starRectControl);
            this.currentStar = Utils.Map(starPosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
            double obscPosition = Canvas.GetLeft(this.obscRectControl);
            this.currentObsc = Utils.Map(obscPosition, 0, this.sliderRect.Width, this.minimum, this.maximum);
            if (this.currentPrice > 100)
            {
                this.currentPrice = 100.00;
                pricePosition = Utils.Map(this.currentPrice, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(this.priceRectControl, ClampLeft(pricePosition - HALFRECT));
                priceRectControl.mapEllipseSize(ClampLeft(pricePosition - HALFRECT), minLeft, maxLeft);

            }
            if (this.currentPrice < 0)
            {
                this.currentPrice = 0.0;
                pricePosition = Utils.Map(this.currentPrice, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(this.priceRectControl, ClampLeft(pricePosition - HALFRECT));
                priceRectControl.mapEllipseSize(ClampLeft(pricePosition - HALFRECT), minLeft, maxLeft);

            }
            if (this.currentObsc > 100)
            {
                this.currentObsc = 100.00;
                obscPosition = Utils.Map(this.currentObsc, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(this.obscRectControl, ClampLeft(obscPosition-HALFRECT));
                obscRectControl.mapEllipseSize(ClampLeft(obscPosition - HALFRECT), minLeft, maxLeft);
            }
            if (this.currentObsc < 0)
            {
                this.currentObsc = 0.0;
                obscPosition = Utils.Map(this.currentObsc, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(this.obscRectControl, ClampLeft(obscPosition - HALFRECT));
                obscRectControl.mapEllipseSize(ClampLeft(obscPosition - HALFRECT), minLeft, maxLeft);

            }
            if (this.currentStar > 100)
            {
                this.currentStar = 100.00;
                starPosition = Utils.Map(this.currentStar, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(this.starRectControl, ClampLeft(starPosition - HALFRECT));
                starRectControl.mapEllipseSize(ClampLeft(starPosition - HALFRECT), minLeft, maxLeft);

            }
            if (this.currentStar < 0)
            {
                this.currentStar = 0.0;
                starPosition = Utils.Map(this.currentStar, this.minimum, this.maximum, 0, this.sliderRect.Width);
                Canvas.SetLeft(this.starRectControl, ClampLeft(starPosition - HALFRECT));
                starRectControl.mapEllipseSize(ClampLeft(starPosition - HALFRECT), minLeft, maxLeft);

            }
            updateWindowWithPositions();
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
