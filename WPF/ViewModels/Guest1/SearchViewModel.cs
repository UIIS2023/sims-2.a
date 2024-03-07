using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Windows;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System.Windows.Navigation;
using SOSTeam.TravelAgency.Domain.DTO;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SearchViewModel
    {
        public User LoggedInUser { get; set; }
        public List<TextBox> textBoxes { get; set; }
        public ComboBox CBTypes { get; set; }
        public List<ComboBoxItem> comboBoxItems { get; set; }
        public RelayCommand searchCommand { get; set; }
        public NavigationService NavigationService { get; set; }

        public SearchViewModel(User user, List<TextBox> list1, List<ComboBoxItem> list2, ComboBox comboBox, NavigationService service)
        {
            LoggedInUser = user;
            CBTypes = comboBox;
            textBoxes = list1;
            comboBoxItems = list2;
            NavigationService = service;
            
            searchCommand = new RelayCommand(ExecuteAccommodationSearch);
        }

        private void ExecuteAccommodationSearch(object sender)
        {
            int guests;
            int days;
            if (textBoxes[3].Text.Equals(string.Empty)) guests = 0;
            else guests = int.Parse(textBoxes[3].Text);
            if (textBoxes[4].Text.Equals(string.Empty)) days = 0;
            else days = int.Parse(textBoxes[4].Text);

            AccommodationService accommodationService = new AccommodationService();
            SuperOwnerService superOwnerService = new();
            List<LocAccommodationDTO> searchResult = accommodationService.ExecuteAccommodationSearch(textBoxes[0].Text, textBoxes[1].Text, textBoxes[2].Text, 
                                                                                                            GetSelectedItem(CBTypes), guests, days);
            foreach (LocAccommodationDTO item in searchResult) 
            {
                item.IsSuperOwned = superOwnerService.IsSuperOwnerAccommodation(item.AccommodationId);
            }

            ShowResults(searchResult.OrderByDescending(a => a.IsSuperOwned).ToList());
        }

        private LocAccommodationDTO.AccommType GetSelectedItem(ComboBox cb)
        {
            if (cb.SelectedItem == comboBoxItems[0]) return LocAccommodationDTO.AccommType.APARTMENT;
            else if (cb.SelectedItem == comboBoxItems[1]) return LocAccommodationDTO.AccommType.HOUSE;
            else if (cb.SelectedItem == comboBoxItems[2]) return LocAccommodationDTO.AccommType.HUT;
            else return LocAccommodationDTO.AccommType.NOTYPE;
        }

        private void ShowResults(List<LocAccommodationDTO> results)
        {
            if (results.Count > 0)
            {
                NavigationService.Navigate(new SearchResultsPage(results, LoggedInUser, NavigationService));
            }
            else
            {
                MessageBox.Show("Nepostojeća kombinacija podataka. Pokušajte ponovo.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
