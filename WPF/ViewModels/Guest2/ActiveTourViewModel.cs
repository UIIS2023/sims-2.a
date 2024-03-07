using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class ActiveTourViewModel : ViewModel
    {
        public string TourName { get; set; }
        public string CheckpointMessage { get; set; }
        public string TourImagePath { get; set; }
        public ActiveTourViewModel(string tourName,string checkpointName,String tourImage) 
        {
            TourName = "Tura " + tourName;
            CheckpointMessage = "Kljucna tacka do koje je tura stigla: " + checkpointName;
            TourImagePath = tourImage;
        }
    }
}
