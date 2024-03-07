using System.Linq;
using SOSTeam.TravelAgency.Commands;
using System.Windows;
using System.Windows.Input;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class YesNoMessageBoxViewModel
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Message { get; set; }

        public bool? DialogResult { get; private set; }

        public RelayCommand ClickYesCommand { get; private set; }
        public RelayCommand ClickNoCommand { get; private set; }

        public YesNoMessageBoxViewModel(string title, string imagePath, string message)
        {
            Title = title;
            ImagePath = imagePath;
            Message = message;

            ClickYesCommand = new RelayCommand(ClickYes);
            ClickNoCommand = new RelayCommand(ClickNo);
        }

        private void ClickYes(object obj)
        {
            DialogResult = true;
            CloseMessageBox();
        }

        private void ClickNo(object obj)
        {
            DialogResult = false;
            CloseMessageBox();
        }

        private void CloseMessageBox()
        {
            Window currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (currentWindow != null)
            {
                currentWindow.DialogResult = DialogResult;
                currentWindow.Close();
            }
        }
    }
}
