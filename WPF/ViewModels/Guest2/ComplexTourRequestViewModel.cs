using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class ComplexTourRequestViewModel : ViewModel
    {
        public int UserId { get; set; }
        public int Counter { get; set; }

        private ObservableCollection<RequestViewModel> _complexTourRequestParts;
        public ObservableCollection<RequestViewModel> ComplexTourRequestParts
        {
            get { return _complexTourRequestParts; }
            set
            {
                if (value != _complexTourRequestParts)
                {
                    _complexTourRequestParts = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _status;

        public string Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public ComplexTourRequestViewModel()
        {

        }
        public ComplexTourRequestViewModel(int userId, StatusType status, ObservableCollection<RequestViewModel> requestParts, int counter)
        {
            UserId = userId;
            ComplexTourRequestParts = requestParts;
            if (status.ToString().Equals("ON_HOLD"))
            {
                Status = "na cekanju";
            }
            else if (status.ToString().Equals("INVALID"))
            {
                Status = "nevazeci";
            }
            else
                Status = "prihvacen";
            Counter = counter;
        }
    }
}
