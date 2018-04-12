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
using System.Windows.Threading;

namespace CityAttractionsAndEvents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {   private Canvas previousWindow;
        private Canvas currentWindow;
        private bool userAuthorized = false;
        private bool userRegistering = false;
        private bool notificationsOn = false;
        private int curUserID;
        static List<Ellipse> currentPlacesShown = new List<Ellipse>();
        static PlacePopout currentPopout = new PlacePopout();
        private List<string> usernames;
        private List<string> passwords;
        private List<string> emails;
        public static List<WishEntry> wishlist { get; set; } = new List<WishEntry>();
        static List<string> blacklist = new List<string>();

        private DispatcherTimer dispatchTimer;

        static GlanceView curGlanceView;
        static ProfileExpanded curProfileExpanded;
        static InfoExpander curWTG;
        static InfoExpander curNTK;

        double priceMin;
        double priceMax;

        Boolean searchClicked = false;
        private bool isPrinting;

        public MainWindow()
        {  
            InitializeComponent();
            InitializeUserLists();

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

        private void InitializeUserLists()
        {
            this.usernames = new List<string>();
            this.passwords = new List<string>();
            this.emails = new List<string>();

            this.usernames.Add("Travor Trapp");
            this.usernames.Add("Loren Lane");

            this.passwords.Add("123");
            this.passwords.Add("123");

            this.emails.Add("travor@email.com");
            this.emails.Add("loren@email.com");
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
            this.checkAttraction.Checked += MapChecked;
            this.checkAttraction.Unchecked += MapChecked;
            this.checkEvent.Checked += MapChecked;
            this.checkEvent.Unchecked += MapChecked;
            this.checkShopping.Checked += MapChecked;
            this.checkShopping.Unchecked += MapChecked;
            this.checkSport.Checked += MapChecked;
            this.checkSport.Unchecked += MapChecked;
            this.checkRestaurant.Checked += MapChecked;
            this.checkRestaurant.Unchecked += MapChecked;
            this.SortBy.SelectionChanged += onSortBy;
            this.NightShader.MouseDown += clickScreen;

            this.HelpHover.MouseEnter += DisplayMapHelp;
            this.HelpHover.MouseLeave += HideMapHelp;

            this.PrintButton.Click += StartPrinting;
            this.CancelButton.Click += CancelPrinting;
            dispatchTimer = new System.Windows.Threading.DispatcherTimer();

        }

        private void StartPrinting(object sender, RoutedEventArgs e)
        {
            if (!isPrinting)
            {
                isPrinting = true;
                this.PrintLabel.Text = "Printing";

                Storyboard sb = this.FindResource("PrintBegin") as Storyboard;
                sb.Begin();

                dispatchTimer.Tick += new EventHandler(DonePrinting);
                dispatchTimer.Interval = new TimeSpan(0, 0, 4);
                dispatchTimer.Start();
            }
        }

        private void DonePrinting(object sender, EventArgs e)
        {
            isPrinting = false;
            this.PrintLabel.Text = "Done!";
            Storyboard sb = this.FindResource("PrintEnd") as Storyboard;
            sb.Begin();
            dispatchTimer.Tick -= new EventHandler(DonePrinting);
        }

        private void CancelPrinting(object sender, EventArgs e) {
            this.PrintLabel.Text = "Cancelling";
            isPrinting = false;
            dispatchTimer.Tick -= new EventHandler(DonePrinting);
        }

        private void HideMapHelp(object sender, MouseEventArgs e)
        {
            this.MapHelpBox.Visibility = Visibility.Hidden;
        }

        private void DisplayMapHelp(object sender, MouseEventArgs e)
        {
            this.MapHelpBox.Visibility = Visibility.Visible;
        }

        private void MapChecked(object sender, RoutedEventArgs e)
        {
            List<Place> places = generatePlaces();
            renderPlaces(places);
        }

        private void CheckMap(object sender, MouseButtonEventArgs e)
        {
            List<Place> places = generatePlaces();
            renderPlaces(places);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(HomePage);

        }

        public void addToBlacklist(string name)
        {
            blacklist.Add(name);
            List<Place> places = generatePlaces();
            renderGlanceViews(places);
            renderPlaces(places);
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
            MyProfileButton.IsEnabled = false;
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(NotificationsPage);
            NotificationsButton.IsEnabled = false;
            NotificationsButtonWithRedEllipse.IsEnabled = false;
        }

        private void NotificationsButtonWithRedEllipse_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(NotificationsPage);
            NotificationsButton.IsEnabled = false;
            NotificationsButtonWithRedEllipse.IsEnabled = false;
        }


        private void NotificationsEllipseButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationsButton_Click(sender, e);
            NotificationsButton.IsEnabled = false;
            NotificationsButtonWithRedEllipse.IsEnabled = false;
        }


        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(CalendarPage);
            bool alreadyAdded;
            List<WishEntry> cloneList = new List<WishEntry>();
            foreach (WishEntry w in wishStack.Children)
            {
                alreadyAdded = false;
                foreach (WishEntry j in cloneList)
                {
                    if (w.text.Text == j.text.Text)
                    {
                        alreadyAdded = true;
                    }
                }
                if (!alreadyAdded)
                {
                    cloneList.Add(w);
                }
            }
            wishStack.Children.Clear();
            foreach (WishEntry w in cloneList)
            {
                wishStack.Children.Add(w);
            }
            wishlist = cloneList;
            CalendarButton.IsEnabled = false;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
            SearchButton.IsEnabled = false;
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(CompassCanvas);
            MapButton.IsEnabled = false;
        }

      
        private void InformationButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(InformationPage);
            InformationButton.IsEnabled = false;
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

        //Based on: https://stackoverflow.com/questions/19694640/animating-gif-in-wpf
        private void myGif_MediaEnded(object sender, RoutedEventArgs e)
        {
            myGif.Position = new TimeSpan(0, 0, 1);
            myGif.Play();
        }

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
            SearchButton.IsEnabled = false;
        }

        private void ShoppingButton_Click(object sender, RoutedEventArgs e)
        {
            this.attCheck.IsChecked = false;
            this.eveCheck.IsChecked = false;
            this.restCheck.IsChecked = false;
            this.sportCheck.IsChecked = false;
            this.shopCheck.IsChecked = true;
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
            SearchButton.IsEnabled = false;
        }

        private void RestaurantsButton_Click(object sender, RoutedEventArgs e)
        {
            this.attCheck.IsChecked = false;
            this.eveCheck.IsChecked = false;
            this.restCheck.IsChecked = true;
            this.sportCheck.IsChecked = false;
            this.shopCheck.IsChecked = false;
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
            SearchButton.IsEnabled = false;
        }

        private void EventsButton_Click(object sender, RoutedEventArgs e)
        {
            this.attCheck.IsChecked = false;
            this.eveCheck.IsChecked = true;
            this.restCheck.IsChecked = false;
            this.sportCheck.IsChecked = false;
            this.shopCheck.IsChecked = false;
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
            SearchButton.IsEnabled = false;
        }

        private void AttractionsButton_Click(object sender, RoutedEventArgs e)
        {
            this.attCheck.IsChecked = true;
            this.eveCheck.IsChecked = false;
            this.restCheck.IsChecked = false;
            this.sportCheck.IsChecked = false;
            this.shopCheck.IsChecked = false;
            UpdateCurrentAndPreviousPages(SearchSortCanvas);
            SearchButton.IsEnabled = false;
        }
       
        //==========================================================================================
        // RELATED TO LOGIN PAGE:
        public void SetLoginPage()
        {
            PageIsNotVisible(LoginPage);
            PageIsNotVisible(this.CreateAccountCanvas);
            InvalidLoginMessage.Visibility = Visibility.Hidden;

            LoginButton.Click += LoginButton_Click;
            FacebookButton.Click += FacebookButton_Click;
            LinkedInButton.Click += LinkedInButton_Click;
            this.RegisterButton.Click += StartCreatingAccount;
            this.CreateAccountButton.Click += CreatingAccountAttempt;

            this.PasswordInput.KeyDown += LoginButton_Enter;
            this.UserNameInput.KeyDown += LoginButton_Enter;
            
        }

        private void LoginButton_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                attemptLogin();

            //if (e.Key == Key.Tab)
            //    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(PasswordInput), PasswordInput);
        }

        private void CreatingAccountAttempt(object sender, RoutedEventArgs e)
        {
            bool creationValid = true;

            if (this.CreateUsernameField.Text == "")
            {
                creationValid = false;
                this.UsernameWarning.Visibility = Visibility.Visible;
                this.UsernameWarning.Text = "Username cannot be empty";
            }
            else if (this.usernames.IndexOf(this.CreateUsernameField.Text) >= 0)
            {
                creationValid = false;
                this.UsernameWarning.Visibility = Visibility.Visible;
                this.UsernameWarning.Text = "Username already exists";
            }
            else
            {
                this.UsernameWarning.Visibility = Visibility.Hidden;
            }





            if (this.CreateEmailField.Text == "")
            {
                creationValid = false;
                this.EmailWarning.Visibility = Visibility.Visible;
                this.EmailWarning.Text = "Email cannot be empty";
            }
            else if (this.emails.IndexOf(this.CreateEmailField.Text) >= 0)
            {
                creationValid = false;
                this.EmailWarning.Visibility = Visibility.Visible;
                this.EmailWarning.Text = "Email already exists";

            }
            else
            {
                this.EmailWarning.Visibility = Visibility.Hidden;
            }


            if (this.CreatePasswordField1.Password == "")
            {
                creationValid = false;
                this.PasswordEmptyWarning.Visibility = Visibility.Visible;
            }
            else {
                this.PasswordEmptyWarning.Visibility = Visibility.Hidden;
            }

            if (this.CreatePasswordField1.Password != this.CreatePasswordField2.Password)
            {
                creationValid = false;
                this.PasswordMismatchWarning.Visibility = Visibility.Visible;
            }
            else {
                this.PasswordMismatchWarning.Visibility = Visibility.Hidden;
            }

            if (creationValid)
            {
                this.usernames.Add(this.CreateUsernameField.Text);
                this.emails.Add(this.CreateEmailField.Text);
                this.passwords.Add(this.CreatePasswordField1.Password);
                UpdateCurrentAndPreviousPages(LoginPage);
            }
            else {
                this.CreatePasswordField1.Password = "";
                this.CreatePasswordField2.Password = "";
            }



        }

        private void StartCreatingAccount(object sender, RoutedEventArgs e)
        {
            UpdateCurrentAndPreviousPages(this.CreateAccountCanvas);
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
            return this.usernames.Contains(userName);
        }

        public bool UserPasswordIsValid()
        {
            int index = getUserID();
            if (index < 0) return false;
            return PasswordInput.Password == passwords[index];

        }

        private bool UserNameAndPasswordAreValid()
        {
            return UserNameIsValid() && UserPasswordIsValid();
        }

        private void attemptLogin() {
            if (!UserNameAndPasswordAreValid())
            {
                InvalidLoginMessage.Visibility = Visibility.Visible;
                //UserNameInput.Text = "";
                PasswordInput.Password = "";

            }
            else
            {
                curUserID = getUserID();
                UpdateCurrentAndPreviousPages(NickProfileCanvas);
                userAuthorized = true;
                InvalidLoginMessage.Visibility = Visibility.Hidden;
            }
        }

        private int getUserID()
        {
            string name = UserNameInput.Text;
            return this.usernames.IndexOf(name);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            attemptLogin();
            
                
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

            // Fill calendar day cells dynamically:
            string [] daysOfApril = {"1", "2", "3", "4" , "5", "6", "7",
                                     "8", "9", "10", "11", "12", "13", "14",
                                     "15", "16", "17", "18", "19", "20", "21",
                                     "22", "23", "24", "25", "26", "27", "28",
                                     "29", "30", "1", "2", "3", "4", "5"};
            CalendarCell[] arrayOfCalendarCells = new CalendarCell [35];
            int i = 0;
            while(i< daysOfApril.Length)
            {
                CalendarCell calendarCell = new CalendarCell(daysOfApril[i], i) { };
                calendarCell.RaiseCalendarEvent += CalendarCell_RaiseCalendarEvent;
                arrayOfCalendarCells[i] = calendarCell;
                CalendarUniformGrid.Children.Add(calendarCell);
                i++;
            }

            // Fill days of the week dynamically:
            string[] daysOfWeek = { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };
            string [] arrayOfDaysOfWeek = new string[7];
            int j = 0;
            while (j < arrayOfDaysOfWeek.Length)
            {
                TextBlock textBlock = new TextBlock();
                CalendarDaysOfWeek.Children.Add(textBlock);
                textBlock.Text = daysOfWeek[j];
                j++;
            }
            //DayButton.Click += DayButton_Click;
        }

        private void CalendarCell_RaiseCalendarEvent(object sender, CalendarEventArgs e)
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

        //========================================================================
        // RELATED TO PRINTING

        //Based on: https://stackoverflow.com/questions/19694640/animating-gif-in-wpf
        private void printGif_MediaEnded (object sender, RoutedEventArgs e)
        {
            printGif.Position = new TimeSpan(0, 0, 1);
            printGif.Play();
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
            InformationButton.IsEnabled = true;
            MapButton.IsEnabled = true;
            SearchButton.IsEnabled = true;
            CalendarButton.IsEnabled = true;
            NotificationsButton.IsEnabled = true;
            HomeButton.IsEnabled = true;
            MyProfileButton.IsEnabled = true;
            NotificationsButtonWithRedEllipse.IsEnabled = true;

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
            Place fourSpot = new Place("Fourth Spot", 2, 4.5, 53, 62.1, 32.99, 160, 132, "fourthspot.jpg", details);
            Place stronghold = new Place("Stronghold", 3, 4.6, 84, 23.1, 16.99, 246, 196, "stronghold.jpg", details);
            Place farmersMarket = new Place("Farmer's Market", 4, 4.7, 184, 19.1, 31.99, 308, 285, "farmersmarket.jpg", details);
            List<Place> places = new List<Place> { cgyTower, glenbowMuseum, studioBell, sledIsland, vgSymphony, pizzaHut, tubbyDog, fourSpot, stronghold, farmersMarket };
            List<Place> tempPlaces = new List<Place>();
            foreach (Place p in places)
            {
                if (blacklist.Contains(p.name))
                {

                } else
                {
                    tempPlaces.Add(p);
                }
            }
            return tempPlaces;
        }

        private void onSortBy(object sender, SelectionChangedEventArgs e)
        {
            List<Place> places = generatePlaces();
            renderPlaces(places);
        }

        private void renderPlaces(List<Place> places)
        {
            CompassCanvas.Children.Remove(currentPopout);
            List<int> selectedPlaces = getSelectedPlaces();
            foreach (Ellipse e in currentPlacesShown)
            {
                this.JustMapCanvas.Children.Remove(e);
            }
            currentPlacesShown.Clear();
            foreach (Place p in places)
            {
                if (selectedPlaces.Contains(p.placeType))
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
                    this.JustMapCanvas.Children.Add(ellipse);
                    currentPlacesShown.Add(ellipse);
                }
            }
        }

        private List<int> getSelectedPlaces()
        {
            List<int> selectedPlaces = new List<int>();
            if (checkAttraction.IsChecked == true)
            {
                selectedPlaces.Add(0);
            }
            if (checkEvent.IsChecked == true)
            {
                selectedPlaces.Add(1);
            }
            if (checkRestaurant.IsChecked == true)
            {
                selectedPlaces.Add(2);
            }
            if (checkSport.IsChecked == true)
            {
                selectedPlaces.Add(3);
            }
            if (checkShopping.IsChecked == true)
            {
                selectedPlaces.Add(4);
            }
            return selectedPlaces;
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

            if (p.placeType == 0) {
                ellipse.Fill = new SolidColorBrush(Color.FromRgb(52,167,209));
            } else if (p.placeType == 1)
            {
                ellipse.Fill = new SolidColorBrush(Color.FromRgb(97, 224, 0));
            } else if (p.placeType == 2)
            {
                ellipse.Fill = new SolidColorBrush(Color.FromRgb(224,142,0));
            } else if (p.placeType == 3)
            {
                ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 2, 173));
            } else if (p.placeType == 4)
            {
                ellipse.Fill = new SolidColorBrush(Color.FromRgb(49, 94, 37));
            }


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
                    currentPlace = place;
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
                    curGlanceView = glanceview;
                    profileStack.Children.Add(glanceview);
                    break;
                }
            }
            ProfileExpanded reflections = new ProfileExpanded();
            reflections.Height = 285;
            reflections.Width = 902.5;
            curProfileExpanded = reflections;
            profileStack.Children.Add(reflections);
            InfoExpander ntkInfo = new InfoExpander("Why Should I Visit?");
            ntkInfo.Width = (902.5);
            curNTK = ntkInfo;
            profileStack.Children.Add(ntkInfo);
            InfoExpander wtgInfo = new InfoExpander("Planning My Visit");
            wtgInfo.Width = (902.5);
            curWTG = wtgInfo;
            profileStack.Children.Add(wtgInfo);

        }

        public void renderPlaceProfile()
        {
            profileStack.Children.Clear();
            profileStack.Children.Add(curGlanceView);
            profileStack.Children.Add(curProfileExpanded);
            profileStack.Children.Add(curNTK);
            profileStack.Children.Add(curWTG);
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
                if (place.price < priceMin || place.price > priceMax)
                {
                    filteredPlaces.Remove(place);
                }
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

        public void updatePriceRange(double min, double max)
        {
            this.priceMin = min;
            this.priceMax = max;
            startSearch();
        }
    }
}
