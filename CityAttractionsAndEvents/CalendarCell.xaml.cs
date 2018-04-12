using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace CityAttractionsAndEvents
{
    /// <summary>
    /// Interaction logic for CalendarCell.xaml
    /// </summary>
    public partial class CalendarCell : UserControl
    {
        public int IndexInArrayOfDays { get; set; } = 0;
        public event EventHandler<CalendarEventArgs> RaiseCalendarEvent;


        public CalendarCell(string day, int indexInArrayOfDays)
        {
            InitializeComponent();
            
            TextDay.Text = day;

            this.IndexInArrayOfDays = indexInArrayOfDays;

            CalendarButton.Click += CalendarButton_Click;
           
    }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseCalendarEvent(this, new CalendarEventArgs() {ArgIndexInTheArrayOfDays = IndexInArrayOfDays });
        }

        public void SetImage(string imagePath)
        {
            List<String> imageFileNames = HelperMethods481.AssemblyManager.GetAllEmbeddedResourceFilesEndingWith(".png", ".jpg");
            Image image = HelperMethods481.AssemblyManager.GetImageFromEmbeddedResources(imagePath);
            PlacePicture.Source = image.Source;
        }
    }
}
