using Prism.Services.Dialogs;
using SOSTeam.TravelAgency.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class OkMessageBoxViewModel
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Message { get; set; }

        public RelayCommand ClickOkCommand { get; private set; }

        public OkMessageBoxViewModel(string title, string imagePath, string message)
        {
            Title = title;
            ImagePath = imagePath;
            Message = message;

            ClickOkCommand = new RelayCommand(ClickOk);
        }

        private void ClickOk(object obj)
        {
            CloseMessageBox();
        }

        private void CloseMessageBox()
        {
            Window currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }
    }
}
