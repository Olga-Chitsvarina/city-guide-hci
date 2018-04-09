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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {   private Canvas previousWindow;
        private Canvas currentWindow;
        private bool userAuthorized = false;
        private bool notificationsOn = false;
        static List<Ellipse> currentPlacesShown = new List<Ellipse>();
        static PlacePopout currentPopout = new PlacePopout();
        Boolean searchClicked = false;
        public MainWindow()
        {  
            InitializeComponent();

            SetHomePage();
            SetLoginPage();
            SetTopPanel();
            SetMyProfilePage();
            SetContactUsPage();
            SetInformationPage();
            SetCalendarPage();
            SetNotificationsPage();
            setCompassCanvas();
            setSearchSortPage();
        }

        //=============================================================================================
        // RELATED TO TOP PANEL:

        public void SetTopPanel()
        {
            TopPanel.Visibility = Visibility.Visible;
            NotificationsButtonWithRedEllipse.Visibility = Visibility.Hidden;

            HomeButton.Click += HomeButton_Click;
            InformationButton.Click += InformationButton_Click;
            MapButton.Click += MapButton_Click;
            SearchButton.Click += SearchButton_Click;
            CalendarButton.Click += CalendarButton_Click;
            NotificationsButton.Click += NotificationsButton_Click;
            NotificationsButtonWithRedEllipse.Click += NotificationsButtonWithRedEllipse_Click;
            MyProfileButton.Click += MyProfileButton_Click;
           
        }

       
        private void setSearchSortPage() {
            renderGlanceViews(generatePlaces());
            this.searchBar.GotFocus += onSearchFocus;
            this.searchBar.KeyDown += searchEnter;
            this.searchButton.Click += searchClick;
            this.attCheck.Checked += Check_Checked;
            this.attCheck.Unchecked += Check_Unchecked;
            this.eveCheck.Checked += Check_Checked;
            this.eveCheck.Unchecked += Check_Unchecked;
            this.restCheck.Checked += Check_Checked;
            this.restCheck.Unchecked += Check_Unchecked;
            this.sportCheck.Checked += Check_Checked;
            this.sportCheck.Unchecked += Check_Unchecked;
            this.shopCheck.Checked += Check_Checked;
            this.shopCheck.Unchecked += Check_Unchecked;
        }

        private void setCompassCanvas()
        {
            PageIsNotVisible(NotificationsPage);
            sliderChange();
            this.TimeSlider.ValueChanged += onTimeSlider;
            List<Place> places = generatePlaces();
            renderPlaces(places);
            this.TypeCombo.SelectionChanged += onTypeCombo;
            this.SortBy.SelectionChanged += onSortBy;
            this.NightShader.MouseDown += clickScreen;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(HomePage);
        }

        private void MyProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (userAuthorized)
            {
                UpdateCurrentAndPreviousPages(NickProfileCanvas);
            }
            else
            {
                UpdateCurrentAndPreviousPages(LoginPage);
            }
           
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(NotificationsPage);
        }

        private void NotificationsButtonWithRedEllipse_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(NotificationsPage);
        }


        private void NotificationsEllipseButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationsButton_Click(sender, e);
        }


        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(CalendarPage);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(CompassCanvas);
        }

      
        private void InformationButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(InformationPage);
        }


        
        public void CheckLogoOrHomeIsVisible()
        {
            if (currentWindow == HomePage)
            {
                LogoTextBlock.Visibility = Visibility.Visible;
                HomeButton.Visibility = Visibility.Hidden;
            }
            else
            {
                LogoTextBlock.Visibility = Visibility.Hidden;
                HomeButton.Visibility = Visibility.Visible;
            }
        }

        public void CheckNotifications()
        {
            if (notificationsOn)
            {
                NotificationsButtonWithRedEllipse.Visibility = Visibility.Visible;
            }

            else
            {
                NotificationsButtonWithRedEllipse.Visibility = Visibility.Hidden;
            }
        }

        //==========================================================================================
        // RELATED TO HOME PAGE
        public void SetHomePage()
        {
            currentWindow = HomePage;

            CheckLogoOrHomeIsVisible();

            PageIsVisible(HomePage);

            AttractionsButton.Click += AttractionsButton_Click;
            EventsButton.Click += EventsButton_Click;
            RestaurantsButton.Click += RestaurantsButton_Click;
            ShoppingButton.Click += ShoppingButton_Click;
            SportButton.Click += SportButton_Click;
            ContactUsButton.Click += ContactUsButton_Click;
        }

        private void ContactUsButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(ContactUsPage);
        }

        private void SportButton_Click(object sender, RoutedEventArgs e)
        {
            this.attCheck.IsChecked = false;
            this.eveCheck.IsChecked = false;
            this.restCheck.IsChecked = false;
            this.sportCheck.IsChecked = true;
            this.shopCheck.IsChecked = false;
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
        }

        private void ShoppingButton_Click(object sender, RoutedEventArgs e)
        {
            this.attCheck.IsChecked = false;
            this.eveCheck.IsChecked = false;
            this.restCheck.IsChecked = false;
            this.sportCheck.IsChecked = false;
            this.shopCheck.IsChecked = true;
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
        }

        private void RestaurantsButton_Click(object sender, RoutedEventArgs e)
        {
            this.attCheck.IsChecked = false;
            this.eveCheck.IsChecked = false;
            this.restCheck.IsChecked = true;
            this.sportCheck.IsChecked = false;
            this.shopCheck.IsChecked = false;
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
        }

        private void EventsButton_Click(object sender, RoutedEventArgs e)
        {
            this.attCheck.IsChecked = false;
            this.eveCheck.IsChecked = true;
            this.restCheck.IsChecked = false;
            this.sportCheck.IsChecked = false;
            this.shopCheck.IsChecked = false;
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
        }

        private void AttractionsButton_Click(object sender, RoutedEventArgs e)
        {
            this.attCheck.IsChecked = true;
            this.eveCheck.IsChecked = false;
            this.restCheck.IsChecked = false;
            this.sportCheck.IsChecked = false;
            this.shopCheck.IsChecked = false;
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
        }
       
        //==========================================================================================
        // RELATED TO LOGIN PAGE:
        public void SetLoginPage()
        {
            PageIsNotVisible(LoginPage);
            InvalidLoginMessage.Visibility = Visibility.Hidden;

            LoginButton.Click += LoginButton_Click;
            FacebookButton.Click += FacebookButton_Click;
            LinkedInButton.Click += LinkedInButton_Click;
        }

        public void placeProfileExpand(String name)
        {
            throw new NotImplementedException();
        }

        private void LinkedInButton_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void FacebookButton_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }


        public bool UserNameIsValid()
        {
            string userName = UserNameInput.Text;
            if (userName == "BadUserName" || userName.Length < 1 )
            {
                return false;
            }
            return true;
        }

        public bool UserPasswordIsValid()
        {

            if (PasswordInput.Password.ToString() == "")
            {
                return false;
            }
            return true;

        }

        private bool UserNameAndPasswordAreValid()
        {
            return UserNameIsValid() && UserPasswordIsValid();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (! UserNameAndPasswordAreValid())
            {
                InvalidLoginMessage.Visibility = Visibility.Visible;
                UserNameInput.Text = "";
                PasswordInput.Password = "";

            }
            else
            {
                UpdateCurrentAndPreviousPages(NickProfileCanvas);
                userAuthorized = true;
                InvalidLoginMessage.Visibility = Visibility.Hidden;
            }
                
        }
        
        //==========================================================================
        // RELATED TO MY PROFILE

        public void SetMyProfilePage()
        {
            PageIsNotVisible(NickProfileCanvas);
        }

        //==========================================================================
        // RELATED TO CONTACT US

        public void SetContactUsPage()
        {
            PageIsNotVisible(ContactUsPage);

            SubmitMessageButton.Click += SubmitMessageButton_Click;
        }

        private void SubmitMessageButton_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }


        //======================================================================================================
        // RELATED TO INFORMATION PAGE

        public void SetInformationPage()
        {
            PageIsNotVisible(InformationPage);
        }

        // ================================================================================================
        // CALENDAR PAGE

        public void SetCalendarPage()
        {
            PageIsNotVisible(CalendarPage);
            EventInformationTextBlock.Visibility = Visibility.Hidden;

            DayButton.Click += DayButton_Click;
        }

        private void DayButton_Click(object sender, RoutedEventArgs e)
        {
            EventInformationTextBlock.Visibility = Visibility.Visible;
        }

        //===========================================================================================
        // NOTIFICATIONS

        public void SetNotificationsPage()
        {
            PageIsNotVisible(NotificationsPage);
           
            CheckBoxNotificationTest.Checked += CheckBoxNotificationTest_Checked;
            CheckBoxNotificationTest.Unchecked += CheckBoxNotificationTest_Unchecked;

            AttractionsNotificationsButton.Click += AttractionsNotificationsButton_Click;
            EventsNotificationsButton.Click += EventsNotificationsButton_Click;
            RestaurantsNotificationButton.Click += RestaurantsNotificationButton_Click;
            ShoppingNotificationsButton.Click += ShoppingNotificationsButton_Click;
            SportNotificationsButton.Click += SportNotificationsButton_Click;
            
        }

        private void SportNotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            CateoryTextBox.Text = "SPORT";
        }

        private void ShoppingNotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            CateoryTextBox.Text = "SHOPPING";
        }

        private void RestaurantsNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            CateoryTextBox.Text = "RESTAURANTS";
        }

        private void EventsNotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            CateoryTextBox.Text = "EVENTS";
        }

        private void AttractionsNotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            CateoryTextBox.Text = "ATTRACTIONS";
            ////NotificationsPresenter obj = new NotificationsPresenter();
            //NotificationsPresenterCanvas.Children.Add(obj);
        }

        private void CheckBoxNotificationTest_Unchecked(object sender, RoutedEventArgs e)
        {
            notificationsOn = false;
            NotificationsButtonWithRedEllipse.Visibility = Visibility.Hidden;

        }

        private void CheckBoxNotificationTest_Checked(object sender, RoutedEventArgs e)
        {
            notificationsOn = true;
            NotificationsButtonWithRedEllipse.Visibility = Visibility.Visible;
        }


        //======================================================================
        // RELATED TO ALL PAGES
        public void PageIsVisible(Canvas page)
        {
            page.Visibility = Visibility.Visible;
        }

        public void PageIsNotVisible(Canvas page)
        {
            page.Visibility = Visibility.Hidden;
        }

        public void UpdateCurrentAndPreviousPages(Canvas newCurrent)
        {
            previousWindow = currentWindow;
            currentWindow = newCurrent;

            PageIsNotVisible(previousWindow);
            PageIsVisible(currentWindow);

            CheckLogoOrHomeIsVisible();
            CheckNotifications();
        }

        private void clickScreen(object sender, MouseButtonEventArgs e)
        {
            CompassCanvas.Children.Remove(currentPopout);
        }

        private List<Place> generatePlaces()
        {
            string details = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis placerat, erat sed mattis ultricies, urna elit ultricies tortor, eget sodales ipsum mauris ac libero.Morbi nec orci sollicitudin, elementum nibh at, lobortis magna.Praesent lacinia, magna vel egestas molestie, neque purus varius augue, nec semper mauris massa id tellus.";
            Place cgyTower = new Place("Calgary Tower", 0, 3.8, 202, 3.2, 18.00, 398, 98, "calgarytower.jpg", details);
            Place glenbowMuseum = new Place("Glenbow Museum", 0, 4.3, 152, 11.2, 16.00, 446, 384, "glenbowmuseum.jpg", details);
            Place studioBell = new Place("Studio Bell National Music Centre", 0, 4.8, 71, 37.2, 18.00, 683, 232, "studiobell.jpg", details);
            Place sledIsland = new Place("Sled Island Music Festival", 1, 4.5, 32, 57.2, 214.99, 683, 232, "sledisland.jpg", details);
            Place vgSymphony = new Place("Video Games Live: Legend of Zelda", 1, 4.1, 56, 44.2, 65.50, 446, 384, "videogameslive.jpg", details);
            Place pizzaHut = new Place("Pizza Hut", 2, 3.1, 10, 15.2, 13.50, 408, 65, "pizzahut.jpg", details);
            Place tubbyDog = new Place("Tubby Dog", 2, 4.3, 102, 23.2, 8.49, 909, 129, "tubbydog.jpg", details);
            Place fourSpot = new Place("Fourth Spot", 2, 4.5, 53, 62.1, 32.99, 160, 32, "fourthspot.jpg", details);
            Place stronghold = new Place("Stronghold", 3, 4.6, 84, 23.1, 16.99, 246, 196, "stronghold.jpg", details);
            Place farmersMarket = new Place("Farmer's Market", 4, 4.7, 184, 19.1, 31.99, 308, 285, "farmersmarket.jpg", details);
            List<Place> places = new List<Place> { cgyTower, glenbowMuseum, studioBell, sledIsland, vgSymphony, pizzaHut, tubbyDog, fourSpot, stronghold, farmersMarket };
            return places;
        }

        private void onSortBy(object sender, SelectionChangedEventArgs e)
        {
            List<Place> places = generatePlaces();
            renderPlaces(places);
        }

        private void onTypeCombo(object sender, SelectionChangedEventArgs e)
        {
            List<Place> places = generatePlaces();
            renderPlaces(places);
        }

        private void renderPlaces(List<Place> places)
        {
            CompassCanvas.Children.Remove(currentPopout);
            foreach (Ellipse e in currentPlacesShown)
            {
                this.CompassCanvas.Children.Remove(e);
            }
            currentPlacesShown.Clear();
            foreach (Place p in places)
            {
                if (p.placeType == this.TypeCombo.SelectedIndex)
                {
                    Ellipse ellipse = new Ellipse();
                    string ellipseName = p.name.Replace(" ", "");
                    ellipseName = ellipseName.Replace("'", "");
                    ellipseName = ellipseName.Replace(":", "");
                    ellipse.Name = ellipseName;
                    ellipse.Fill = Brushes.White;
                    ellipse.Stroke = Brushes.Black;
                    Canvas.SetLeft(ellipse, p.posLeft);
                    Canvas.SetTop(ellipse, p.posTop);
                    ellipse = generateColourSize(ellipse, p);
                    this.CompassCanvas.Children.Add(ellipse);
                    currentPlacesShown.Add(ellipse);
                }
            }
        }

        private double getPercentage(Place p)
        {
            double percentage = 0;
            if (this.SortBy.SelectedIndex == 0)
            {
                percentage = p.starRating / 5.0;
            }
            else if (this.SortBy.SelectedIndex == 1)
            {
                percentage = (p.obscurityRating / 100);
            }
            else if (this.SortBy.SelectedIndex == 2)
            {
                if (p.placeType == 0)
                {
                    percentage = (p.price / 40.0);
                }
                else if (p.placeType == 1)
                {
                    percentage = (p.price / 200.0);
                }
                else if (p.placeType == 2)
                {
                    percentage = (p.price / 100.0);
                }
                else if (p.placeType == 3)
                {
                    percentage = (p.price / 40.0);
                }
                else if (p.placeType == 4)
                {
                    percentage = (p.price / 100.0);
                }
                percentage = 1 - percentage;
            }
            if (percentage > 1)
            {
                return 1.0;
            }
            return percentage;
        }

        private Ellipse generateColourSize(Ellipse ellipse, Place p)
        {

            double percentage = getPercentage(p);
            double size = percentage * 50;
            if (size < 15)
            {
                size = 15;
            }
            ellipse.Width = size;
            ellipse.Height = size;

            double red;
            double green;
            if (percentage < 0.5)
            {
                red = 255;
                green = Math.Round(255 * (percentage * 2));
            }
            else
            {
                green = 255;
                red = Math.Round(255 - (255 * (percentage * 0.5)));
            }
            ellipse.Fill = new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, 0));

            ellipse.Cursor = Cursors.Hand;

            ellipse.MouseDown += onPlace;

            return ellipse;
        }

        private void onPlace(object sender, MouseButtonEventArgs e)
        {
            CompassCanvas.Children.Remove(currentPopout);
            Ellipse ellipse = (Ellipse)sender;
            List<Place> places = generatePlaces();
            foreach (Place p in places)
            {
                string placeName = p.name.Replace(" ", "");
                placeName = placeName.Replace("'", "");
                placeName = placeName.Replace(":", "");
                if (placeName == ellipse.Name)
                {
                    string details = "";
                    if (SortBy.SelectedIndex == 0)
                    {
                        details = "Star Rating: " + p.starRating.ToString() + "/5 (" + p.numOfReviews.ToString() + ").";
                    }
                    else if (SortBy.SelectedIndex == 1)
                    {
                        details = "Obscurity Rating: " + p.obscurityRating.ToString() + "/100.";
                    }
                    else
                    {
                        details = "Average Price of Visit: $" + p.price.ToString() + ".";
                    }
                    double percentage = getPercentage(p);
                    PlacePopout popout = new PlacePopout();
                    popout.Width = 145;
                    popout.Height = 88;
                    if (Canvas.GetTop(ellipse) > 100)
                    {
                        popout = new PlacePopout(p.name, percentage * 100, details, true);
                        Canvas.SetTop(popout, Canvas.GetTop(ellipse) - (popout.Height / 2) - 4);
                    }
                    else
                    {
                        popout = new PlacePopout(p.name, percentage * 100, details, false);
                        Canvas.SetTop(popout, Canvas.GetTop(ellipse) + ellipse.Height);
                    }
                    Canvas.SetLeft(popout, Canvas.GetLeft(ellipse) - (popout.Width / 4) - (50 - ellipse.Width) / 2);
                    popout.viewButton.Click += ViewButton_Click;
                    this.CompassCanvas.Children.Add(popout);
                    currentPopout = popout;
                }
            }
        }

        private void setPlaceProfile(string name)
        {
            List<Place> places = generatePlaces();
            Place currentPlace = new Place();
            foreach (Place place in places)
            {
                if (name == place.name)
                {
                    GlanceView glanceview = new GlanceView(place.name, place.details, place.starRating, place.obscurityRating, place.price, place.imagePath);
                    glanceview.Height = 285;
                    glanceview.Width = 902.5;
                    glanceview.obscurityBar.Value = place.obscurityRating;
                    double percentage = 0;
                    if (place.placeType == 0)
                    {
                        percentage = (place.price / 40.0);
                    }
                    else if (place.placeType == 1)
                    {
                        percentage = (place.price / 200.0);
                    }
                    else if (place.placeType == 2)
                    {
                        percentage = (place.price / 100.0);
                    }
                    else if (place.placeType == 3)
                    {
                        percentage = (place.price / 40.0);
                    }
                    else if (place.placeType == 4)
                    {
                        percentage = (place.price / 100.0);
                    }
                    percentage = 1 - percentage;
                    glanceview.priceBar.Value = percentage * 100;
                    glanceview.expandButton.Visibility = Visibility.Hidden;
                    profileStack.Children.Add(glanceview);
                    currentPlace = place;
                    break;
                }
            }
            ProfileExpanded reflections = new ProfileExpanded();
            reflections.Height = 285;
            reflections.Width = 902.5;
            profileStack.Children.Add(reflections);
            InfoExpander ntkInfo = new InfoExpander("Why Should I Visit?");
            ntkInfo.Width = (902.5);
            profileStack.Children.Add(ntkInfo);
            InfoExpander wtgInfo = new InfoExpander("Planning My Visit");
            wtgInfo.Width = (902.5);
            profileStack.Children.Add(wtgInfo);

        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            setPlaceProfile(((PlacePopout)((Canvas)button.Parent).Parent).PlaceName.Text);
            UpdateCurrentAndPreviousPages(ProfilePageCanvas);
        }

        private void onTimeSlider(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderChange();
        }

        private void sliderChange()
        {
            string sliderValue = getTimeFromValue(TimeSlider.Value);
            TimeText.Text = sliderValue;
            string strSunriseTime = "7:41am";
            string strSunsetTime = "7:22pm";
            int sunriseTime = getTimeInMins(strSunriseTime);
            int sunsetTime = getTimeInMins(strSunsetTime);
            int curTime = getTimeInMins(sliderValue);

            if (curTime > sunriseTime && curTime < sunsetTime)         //the day
            {

                ImageBrush sunmoon = new ImageBrush();
                sunmoon.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/images/Sun.png"));
                this.SunMoonSphere.Fill = sunmoon;
                this.NightShader.Opacity = 0;
                double minutesInDay = sunsetTime - sunriseTime;
                double minutesIntoDay = sunsetTime - curTime;
                double percentageIntoDay = minutesIntoDay / minutesInDay;
                double position = 900 + (125.0 * percentageIntoDay);
                Canvas.SetLeft(this.SunMoonSphere, position);
            }
            else if (curTime < sunriseTime)                                              //before sunrise
            {
                ImageBrush sunmoon = new ImageBrush();
                sunmoon.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/images/moon.png"));
                this.SunMoonSphere.Fill = sunmoon;
                int previousSunsetTime = sunsetTime;
                double tempSunriseTime = sunriseTime + 1440;
                double minutesIntoNight = curTime + 1440;
                double percentageIntoNight = minutesIntoNight / tempSunriseTime;
                double position = 1025.0 - (125.0 * percentageIntoNight);
                Canvas.SetLeft(this.SunMoonSphere, position);
                double shaderPercentage = 0.4 * percentageIntoNight;
                this.NightShader.Opacity = 0.4 - shaderPercentage;
            }
            else                                                                                            //after sunset
            {
                int nextSunriseTime = sunriseTime + 1440;
                ImageBrush sunmoon = new ImageBrush();
                sunmoon.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/images/moon.png"));
                this.SunMoonSphere.Fill = sunmoon;
                double timeInNight = nextSunriseTime - sunsetTime;
                double timeIntoNight = nextSunriseTime - curTime;
                double percentageIntoNight = timeIntoNight / timeInNight;
                double position = 900.0 + (125.0 * percentageIntoNight);
                Canvas.SetLeft(this.SunMoonSphere, position);
                double shaderPercentage = 0.4 * percentageIntoNight;
                this.NightShader.Opacity = 0.4 - shaderPercentage;
            }
        }

        //turns string like 7:47pm into a number of minutes 
        private int getTimeInMins(string time)
        {
            Boolean afternoon = false;
            int hour = int.Parse(time.Substring(0, time.IndexOf(":")));
            if (time.Contains("p"))
                afternoon = true;
            if (afternoon && hour == 12)
            {
                afternoon = false;
            }
            else if (!afternoon && hour == 12)
            {
                hour = 0;
            }
            char[] charsToTrim = { 'a', 'p', 'm', 'M', 'A', 'P' };
            time = time.TrimEnd(charsToTrim);
            int minutes = int.Parse(time.Substring(time.IndexOf(":") + 1));
            if (afternoon)
                hour += 12;
            return hour * 60 + minutes;
        }

        //turns value of text slider into time
        private string getTimeFromValue(double value)
        {
            Boolean am = false;
            double decValue = (value % 1.0);
            var minutes = Math.Round((60 * decValue), 0).ToString();
            if (int.Parse(minutes) < 10)
                minutes = "0" + minutes.ToString();
            var hours = Math.Floor(value);
            if (hours == 0)
            {
                hours = 12;
                am = true;
            }
            else if (hours > 12)
            {
                hours -= 12;
            }
            else if (hours == 12)
            {
            }
            else
            {
                am = true;
            }
            if (am)
                return hours + ":" + minutes + "am";
            else
                return hours + ":" + minutes + "pm";
        }
        private void Check_Unchecked(object sender, RoutedEventArgs e)
        {
            startSearch();
        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            startSearch();
        }

        private void searchEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                startSearch();
        }

        private void searchClick(object sender, RoutedEventArgs e)
        {
            startSearch();
        }

        private void startSearch()
        {
            loadProgress.Visibility = Visibility.Visible;
            List<Place> places = generatePlaces();
            List<Place> filteredPlaces = new List<Place>();
            foreach (Place place in places)
            {
                filteredPlaces.Add(place);
            }
            foreach (Place place in places)
            {
                if (place.placeType == 0 && attCheck.IsChecked.Value == false)
                    filteredPlaces.Remove(place);
                if (place.placeType == 1 && eveCheck.IsChecked.Value == false)
                    filteredPlaces.Remove(place);
                if (place.placeType == 2 && restCheck.IsChecked.Value == false)
                    filteredPlaces.Remove(place);
                if (place.placeType == 3 && sportCheck.IsChecked.Value == false)
                    filteredPlaces.Remove(place);
                if (place.placeType == 4 && shopCheck.IsChecked.Value == false)
                    filteredPlaces.Remove(place);
                if (searchBar.Text != "Search...")
                    if (!place.name.ToLower().Contains(searchBar.Text.ToLower()) && !place.details.ToLower().Contains(searchBar.Text.ToLower()))
                        filteredPlaces.Remove(place);
            }
            renderGlanceViews(filteredPlaces);
            loadProgress.Visibility = Visibility.Hidden;
        }

        private void onSearchFocus(object sender, RoutedEventArgs e)
        {
            if (!searchClicked)
            {
                this.searchBar.Text = "";
                this.searchBar.TextAlignment = TextAlignment.Right;
                searchClicked = true;
            }
        }
        private void renderGlanceViews(List<Place> places)
        {
            Boolean listNotEmpty = false;
            this.glanceStack.Children.Clear();
            foreach (Place place in places)
            {
                listNotEmpty = true;
                GlanceView glanceview = new GlanceView(place.name, place.details, place.starRating, place.obscurityRating, place.price, place.imagePath);
                glanceview.Height = 285;
                glanceview.Width = 902.5;
                glanceview.obscurityBar.Value = place.obscurityRating;
                double percentage = 0;
                if (place.placeType == 0)
                {
                    percentage = (place.price / 40.0);
                }
                else if (place.placeType == 1)
                {
                    percentage = (place.price / 200.0);
                }
                else if (place.placeType == 2)
                {
                    percentage = (place.price / 100.0);
                }
                else if (place.placeType == 3)
                {
                    percentage = (place.price / 40.0);
                }
                else if (place.placeType == 4)
                {
                    percentage = (place.price / 100.0);
                }
                percentage = 1 - percentage;
                glanceview.priceBar.Value = percentage * 100;
                glanceview.expandButton.Click += ExpandButton_Click;
                this.glanceStack.Children.Add(glanceview);
            }
            if (!listNotEmpty)
            {
                GlanceView glanceview = new GlanceView("Place Not Found", "No Search results found", 0.0, 0.0, 0.0, "");
                glanceview.Height = 285;
                glanceview.Width = 902.5;
                glanceview.obscurityBar.Visibility = Visibility.Hidden;
                glanceview.priceBar.Visibility = Visibility.Hidden;
                glanceview.transitImage.Visibility = Visibility.Hidden;
                glanceview.carImage.Visibility = Visibility.Hidden;
                glanceview.walkImage.Visibility = Visibility.Hidden;
                glanceview.priceText.Visibility = Visibility.Hidden;
                glanceview.obscurityText.Visibility = Visibility.Hidden;
                glanceview.priceValueText.Visibility = Visibility.Hidden;
                glanceview.obscurityBar.Visibility = Visibility.Hidden;
                glanceview.obscValueText.Visibility = Visibility.Hidden;
                glanceview.blacklistImage.Visibility = Visibility.Hidden;
                glanceview.calendarImage.Visibility = Visibility.Hidden;
                glanceview.wishlistImage.Visibility = Visibility.Hidden;
                glanceview.expandButton.Visibility = Visibility.Hidden;
                this.glanceStack.Children.Add(glanceview);
            }
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            setPlaceProfile(((GlanceView)((Canvas)button.Parent).Parent).nameText.Text);
            UpdateCurrentAndPreviousPages(ProfilePageCanvas);
        }
    }
}
