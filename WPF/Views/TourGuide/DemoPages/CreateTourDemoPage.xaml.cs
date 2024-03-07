using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace SOSTeam.TravelAgency.WPF.Views.TourGuide.DemoPages
{
    public partial class CreateTourDemoPage : Page
    {
        private DispatcherTimer timer;
        private int currentLetterIndex = 0;
        private bool isFillingName = true; // Flag to track the state of the demo
        private bool isCountrySelected = false;
        private bool isCitySelected = false;
        private bool isDescriptionFilled = false;
        private bool isLanguageSelected = false;
        private bool isMaxNumOfGuestFilled = false;
        private bool isDurationFilled = false;
        private bool isOpenedAddCheckpoint = false;
        private int currentDurationValue = 1; // Current value for DurationPicker simulation
        private int currentNumOfGuests = 1;

        public CreateTourDemoPage()
        {
            InitializeComponent();
            DataContext = this;
            StartDemo();

            var currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var mainWindow = (MainWindow)currentWindow;

            mainWindow.BackButton.IsEnabled = false;
            mainWindow.CreateButton.IsEnabled = false;
            mainWindow.HomeButton.IsEnabled = false;
            mainWindow.LiveTourButton.IsEnabled = false;
            mainWindow.DemoButton.IsEnabled = false;
            mainWindow.BurgerMenuButton.IsEnabled = false;
        }

        private void StartDemo()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.15); // Set the initial interval to 0.2 seconds
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isFillingName)
            {
                FillTextBoxLetterByLetter(NameTextBox, "Bulgaria 2024");
            }
            else if (!isCountrySelected)
            {
                SimulateComboBoxSelection(CountryComboBox, 0);
            }
            else if (!isCitySelected)
            {
                SimulateComboBoxSelection(CityComboBox, 0);
            }
            else if (!isDescriptionFilled)
            {
                FillTextBoxLetterByLetter(DescriptionTextBox, "This is tour description");
            }
            else if (!isLanguageSelected)
            {
                SimulateComboBoxSelection(LanguageComboBox, 0);
            }
            else if(!isMaxNumOfGuestFilled)
            {
                SimulateNumOfGuestsPicker();
            }
            else if(!isDurationFilled)
            {
                SimulateDurationPicker();
            }
            else if (!isOpenedAddCheckpoint)
            {
                SimulateOpenCheckpointStartCheckpoint();
            }
        }

        private void FillTextBoxLetterByLetter(TextBox textBox, string text)
        {
            if (currentLetterIndex < text.Length)
            {
                textBox.Text += text[currentLetterIndex];
                currentLetterIndex++;
            }
            else
            {
                Pause();
            }
        }

        private void SimulateComboBoxSelection(ComboBox comboBox, int index)
        {
            comboBox.SelectedIndex = index;
            Pause();
        }

        private void SimulateNumOfGuestsPicker()
        {
            if (currentNumOfGuests <= 20)
            {
                MaxNumOfGuestsPicker.Value = currentNumOfGuests;
                currentNumOfGuests++;
            }
            else
            {
                Pause();
            }
        }

        private void SimulateDurationPicker()
        {
            if (currentDurationValue <= 13)
            {
                DurationPicker.Value = currentDurationValue;
                currentDurationValue++;
            }
            else
            {
                Pause();
            }
        }

        private void SimulateOpenCheckpointStartCheckpoint()
        {
            OpenAddCheckpointsPage_OnClick(this, null);
            Pause();
        }

        private void Pause()
        {
            timer.Stop();
            currentLetterIndex = 0;
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Start();

            if (isFillingName)
            {
                isFillingName = false;
            }
            else if (!isCountrySelected)
            {
                isCountrySelected = true;
            }
            else if (!isCitySelected)
            {
                isCitySelected = true;
            }
            else if (!isDescriptionFilled)
            {
                isDescriptionFilled = true;
            }
            else if (!isLanguageSelected)
            {
                isLanguageSelected = true;
            }
            else if (!isMaxNumOfGuestFilled)
            {
                isMaxNumOfGuestFilled = true;
            }
            else if(!isDurationFilled)
            {
                isDurationFilled = true;
            }
            else if(!isOpenedAddCheckpoint)
            {
                isOpenedAddCheckpoint = true;
            }
        }

        private async void OpenAddCheckpointsPage_OnClick(object sender, RoutedEventArgs e)
        {
            var currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var mainWindow = (MainWindow)currentWindow;

            mainWindow.BackButton.IsEnabled = false;

            AddCheckpointsButton.Background = new SolidColorBrush(Color.FromRgb(196, 229, 246));
            
            await Task.Delay(200);
            
            AddCheckpointsButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));

            

            mainWindow.MainFrame.Content = new AddCheckpointsDemoPage();
            await Task.Delay(8000);

            PageScrollViewer.ScrollToEnd();

            var navigationService = mainWindow.MainFrame.NavigationService;
            navigationService.GoBack();

            await Task.Delay(2000);

            AddAppointmentsButton.Background = new SolidColorBrush(Color.FromRgb(196, 229, 246));
            await Task.Delay(200);
            AddAppointmentsButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));


            mainWindow.MainFrame.Content = new AddAppointmentsDemoPage();
            await Task.Delay(8000);
            navigationService.GoBack();

            await Task.Delay(2000);

            CreateTourButton.Background = new SolidColorBrush(Color.FromRgb(196, 229, 246));
            await Task.Delay(200);
            CreateTourButton.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));

            App.TourGuideNavigationService.GoBack("Back");
            //navigationService.GoBack();

            mainWindow.BackButton.IsEnabled = true;
            mainWindow.CreateButton.IsEnabled = true;
            mainWindow.HomeButton.IsEnabled = true;
            mainWindow.LiveTourButton.IsEnabled = true;
            mainWindow.DemoButton.IsEnabled = true;
            mainWindow.BurgerMenuButton.IsEnabled = true;
        }
    }
}
