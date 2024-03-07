using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class StatsPerTourOverviewViewModel : ViewModel
    {
        private ObservableCollection<TourCardViewModel> _tourCards;

        public ObservableCollection<TourCardViewModel> TourCards
        {
            get => _tourCards;
            set
            {
                if (_tourCards != value)
                {
                    _tourCards = value;
                    OnPropertyChanged("TourCards");
                }
            }
        }

        private string _selectedYear;

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged("SelectedYear");
                }
            }
        }

        private ObservableCollection<string> _availableYears;

        public ObservableCollection<string> AvailableYears
        {
            get => _availableYears;
            set
            {
                if (_availableYears != value)
                {
                    _availableYears = value;
                    OnPropertyChanged("AvailableYears");
                }
            }

        }

        public User LoggedUser { get; set; }

        public TourCardViewModel SelectedTourCard { get; set; }

        private readonly TourCardCreatorViewModel _tourCardCreator;

        public RelayCommand YearSelectionChangedCommand { get; set; }
        public RelayCommand ShowTourStatsCommand { get; set; }

        public StatsPerTourOverviewViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;

            _tourCardCreator = new TourCardCreatorViewModel();
            var availableYearsCreator = new AvailableYearsCreator();

            AvailableYears = availableYearsCreator.GetAvailableYears(loggedUser);

            if (AvailableYears.Count > 0)
            {
                SelectedYear = AvailableYears[0];
            }

            TourCards = _tourCardCreator.CreateCardsPerYear(loggedUser, SelectedYear);

            
            YearSelectionChangedCommand = new RelayCommand(ExecuteYearSelectionChanged, CanExecuteMethod);
            ShowTourStatsCommand = new RelayCommand(ShowTourStats, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public void ExecuteYearSelectionChanged(object parameter)
        {
            YearSelectionChanged(parameter, null);
        }

        public void YearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TourCards = _tourCardCreator.CreateCardsPerYear(LoggedUser, SelectedYear);
        }

        private void ShowTourStats(object sender)
        {
            var selectedTourCard = sender as TourCardViewModel;
            SelectedTourCard = selectedTourCard;

            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("StatByTour", App.LoggedUser);

            //StatByTourPage statByTourPage = new StatByTourPage(selectedTourCard);
            //System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MainFrame.Content = statByTourPage;
        }

    }
}
