using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Media;
using SOSTeam.TravelAgency.Commands;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class AddAppointmentsViewModel : ViewModel
    {

        private DateTime _start;

        public DateTime Start
        {
            get => _start;
            set
            {
                if (_start != value)
                {
                    _start = value;
                    OnPropertyChanged("Start");
                }
            }
        }

        private ObservableCollection<AppointmentCardViewModel> _appointmentCards;

        public ObservableCollection<AppointmentCardViewModel> AppointmentCards
        {
            get => _appointmentCards;
            set
            {
                if (_appointmentCards != value)
                {
                    _appointmentCards = value;
                    OnPropertyChanged("AppointmentCards");
                }
            }
        }

        private AppointmentCardViewModel? _selectedCard;

        public AppointmentCardViewModel? SelectedCard
        {
            get => _selectedCard;
            set
            {
                if (_selectedCard != value)
                {
                    _selectedCard = value;
                    OnPropertyChanged("SelectedCard");
                }
            }
        }

        private string _buttonContent;

        public string ButtonContent
        {
            get => _buttonContent;
            set
            {
                if (_buttonContent != value)
                {
                    _buttonContent = value;
                    OnPropertyChanged("ButtonContent");
                }
            }
        }

        public RelayCommand AddAppointmentCommand { get; set; }
        public RelayCommand EditAppointmentCommand { get; set; }
        public RelayCommand DeleteAppointmentCommand { get; set; }
        public RelayCommand ClearAppointmentsCommand { get; set; }

        public AddAppointmentsViewModel(ObservableCollection<AppointmentCardViewModel> appointmentCards)
        {
            _appointmentCards = appointmentCards;
            _selectedCard = null;
            _start = DateTime.Now.Add(TimeSpan.FromMinutes(1));
            _buttonContent = "Add";

            AddAppointmentCommand = new RelayCommand(AddAppointment, CanExecuteMethod);
            EditAppointmentCommand = new RelayCommand(EditAppointment, CanExecuteMethod);
            DeleteAppointmentCommand = new RelayCommand(DeleteAppointment, CanExecuteMethod);
            ClearAppointmentsCommand = new RelayCommand(DeleteAllAppointments, CanExecuteMethod);
           

        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public void EditAppointment(object sender)
        {
            var selectedAppointment = sender as AppointmentCardViewModel;

            if (SelectedCard != null)
            {
                var index = AppointmentCards.IndexOf(SelectedCard);
                AppointmentCards[index].Background = new SolidColorBrush(Colors.AliceBlue);
                AppointmentCards[index].CanEdit = true;
                AppointmentCards[index].CanDelete = true;
                AppointmentCards[index].CanEditImagePath = "/Resources/Icons/pencil.png";
                AppointmentCards[index].CanDeleteImagePath = "/Resources/Icons/bin.png";
            }
            SelectedCard = selectedAppointment;
            Start = SelectedCard.Start;
            selectedAppointment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8FFB7"));
            selectedAppointment.CanEdit = false;
            selectedAppointment.CanDelete = false;
            selectedAppointment.CanEditImagePath = "/Resources/Icons/pencil_light.png";
            selectedAppointment.CanDeleteImagePath = "/Resources/Icons/bin_light.png";

            ButtonContent = "Confirm";
        }

        public void AddAppointment(object sender)
        {
            if (ButtonContent == "Confirm")
            {
                var index = AppointmentCards.IndexOf(SelectedCard);
                AppointmentCards[index].Start = Start;
                AppointmentCards[index].Background = new SolidColorBrush(Colors.AliceBlue);
                AppointmentCards[index].CanEdit = true;
                AppointmentCards[index].CanDelete = true;
                AppointmentCards[index].CanEditImagePath = "/Resources/Icons/pencil.png";
                AppointmentCards[index].CanDeleteImagePath = "/Resources/Icons/bin.png";


                ButtonContent = "Add";
                SelectedCard = null;
            }
            else
            {
                var appointmentCard = new AppointmentCardViewModel
                {
                    Start = Start
                };

                AppointmentCards.Add(appointmentCard);
            }
            Start = DateTime.Now.Add(TimeSpan.FromMinutes(1)); ;
        }

        public void DeleteAppointment(object sender)
        {
            var selectedAppointment = sender as AppointmentCardViewModel;
            if (selectedAppointment == null) { return; }
            AppointmentCards.Remove(selectedAppointment);
        }

        public void DeleteAllAppointments(object sender)
        {
            AppointmentCards.Clear();

            ButtonContent = "Add";
            Start = DateTime.Now.Add(TimeSpan.FromMinutes(1)); ;
        }

    }
}
