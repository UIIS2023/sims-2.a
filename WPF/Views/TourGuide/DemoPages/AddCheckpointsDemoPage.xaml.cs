using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;

namespace SOSTeam.TravelAgency.WPF.Views.TourGuide.DemoPages
{
    /// <summary>
    /// Interaction logic for AddCheckpointsDemoPage.xaml
    /// </summary>
    public partial class AddCheckpointsDemoPage : Page, INotifyPropertyChanged
    {
        private DispatcherTimer timer;
        private int currentLetterIndex = 0;
        private bool isFillingNameForStart = true; // Flag to track the state of the demo
        private bool isTypeSelectedForStart = false;
        private bool isAddedStartCheckpoint = false;
        private bool isFillingNameForEnd = false;
        private bool isTypeSelectedForEnd = false;
        private bool isAddedEndCheckpoint = false;

        private CheckpointType _type;
        public CheckpointType Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<CheckpointCardViewModel> CheckpointCards { get; set; }

        public AddCheckpointsDemoPage()
        {
            InitializeComponent();
            DataContext = this;
            CheckpointCards = new ObservableCollection<CheckpointCardViewModel>();
            StartDemo();
        }

        private async void StartDemo()
        {
            await Task.Delay(100);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.2); // Set the initial interval to 0.2 seconds
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isFillingNameForStart)
            {
                FillTextBoxLetterByLetter(NameTextBox, "Belgrade");
            }
            else if (!isTypeSelectedForStart)
            {
                SimulateComboBoxSelection(TypeComboBox, 0);
            }
            else if (!isAddedStartCheckpoint)
            {
                SimulateAddingStartCheckpoint();
            }
            else if (!isAddedStartCheckpoint)
            {
                SimulateAddingStartCheckpoint();
            }
            else if(!isFillingNameForEnd)
            {
                FillTextBoxLetterByLetter(NameTextBox, "Plovdiv");
            }
            else if (!isTypeSelectedForEnd)
            {
                SimulateComboBoxSelection(TypeComboBox, 2);
            }
            else if (!isAddedEndCheckpoint)
            {
                SimulateAddingStartCheckpoint();
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

        private void SimulateAddingStartCheckpoint()
        {
            AddCheckpoint_OnClick(this, null);
            Pause();
        }

        private void Pause()
        {
            timer.Stop();
            currentLetterIndex = 0;
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Start();

            if (isFillingNameForStart)
            {
                isFillingNameForStart = false;
            }
            else if (!isTypeSelectedForStart)
            {
                isTypeSelectedForStart = true;
            }
            else if (!isAddedStartCheckpoint)
            {
                isAddedStartCheckpoint = true;
            }
            else if (!isFillingNameForEnd)
            {
                isFillingNameForEnd = true;
            }
            else if (!isTypeSelectedForEnd)
            {
                isTypeSelectedForEnd = true;
            }
            else if (!isAddedEndCheckpoint)
            {
                isAddedEndCheckpoint = true;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void AddCheckpoint_OnClick(object sender, RoutedEventArgs e)
        {
            CheckpointCardViewModel checkpointCard = new CheckpointCardViewModel();
            checkpointCard.Name = NameTextBox.Text;
            checkpointCard.Type = Type;

            // Set the initial color
            AddButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(196, 229, 246));

            // Delay for 0.3 seconds
            await Task.Delay(100);

            // Set the final color
            AddButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(211, 211, 211));
            CheckpointCards.Add(checkpointCard);

            // Reset other UI elements
            NameTextBox.Text = string.Empty;
            TypeComboBox.SelectedItem = null;
        }
    }
}
