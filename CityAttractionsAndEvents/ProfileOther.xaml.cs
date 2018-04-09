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

namespace ProfileV2
{
    /// <summary>
    /// Interaction logic for ProfileOther.xaml
    /// </summary>
    public partial class ProfileOther : UserControl
    {
        ScrollViewer sv;
        Point mouseRel;
        double curScroll;
        public ProfileOther()
        {
            InitializeComponent();
            this.sv = this.profileInterests;

            mouseRel = new Point();

            sv.PreviewMouseLeftButtonDown += ScrollInterestsStart;
            sv.PreviewMouseMove += ScrollingInterests;
            sv.PreviewMouseLeftButtonUp += ScrollInterestsDone;

            foreach (Object icon in this.InterestsSP.Children)
            {
                if (icon is Button)
                    ((Button)icon).Cursor = Cursors.Hand;
            }
        }

        private void ScrollingInterests(object sender, MouseEventArgs e)
        {
            if (sv.IsMouseCaptured)
            {
                double visualOffset = (curScroll + (mouseRel.X - e.GetPosition(sv).X));
                double factor = Math.Round(visualOffset / 1);
                double actualOff = factor * 1;
                sv.ScrollToHorizontalOffset(actualOff);
            }


        }

        private void ScrollInterestsDone(object sender, MouseButtonEventArgs e)
        {

            sv.ReleaseMouseCapture();
            //this.prered.Background = Brushes.White;
        }

        private void ScrollInterestsStart(object sender, MouseButtonEventArgs e)
        {
            mouseRel = e.GetPosition(sv);
            sv.CaptureMouse();
            curScroll = sv.HorizontalOffset;

            //this.prered.Background = Brushes.Black;
        }
    }
    
}
