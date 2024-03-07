using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class WhateverResultsViewModel
    {
        public int Guests { get; set; }
        public int Days { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }  
        public WhateverSearchResultsDTO SelectedAccommodation { get; set; } 
        public List<WhateverSearchResultsDTO> _results { get; set; }
        public NavigationService NavigationService { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand ShowCatalogCommand { get; set; }

        public WhateverResultsViewModel(List<WhateverSearchResultsDTO> list, NavigationService service, DateTime fDate, DateTime lDate, int guests, int days)
        {
            _results = new List<WhateverSearchResultsDTO>();
            _results.Clear();
            _results = list;
            NavigationService = service;
            FirstDate = fDate;
            LastDate = lDate;
            Guests = guests;
            Days = days;

            GoBackCommand = new RelayCommand(Execute_GoBack);
            ShowCatalogCommand = new RelayCommand(Execute_Reserve);
        }

        private void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }

        
        private void Execute_Reserve(object sender)
        {
            if(SelectedAccommodation == null)
            {
                MessageBox.Show("Niste odabrali smještaj čiju ponudu želite da vidite.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                NavigationService.Navigate(new WhateverCatalogPage(SelectedAccommodation, NavigationService, FirstDate, LastDate, Guests, Days));
            }
        }
    }
}
