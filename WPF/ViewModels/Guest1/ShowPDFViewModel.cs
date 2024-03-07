using SOSTeam.TravelAgency.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Syncfusion.PdfViewer;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Windows.PdfViewer;
using System.ComponentModel;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class ShowPDFViewModel 
    {
        public NavigationService NavigationService { get; set; }
        public FileStream FileStream { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public ShowPDFViewModel(FileStream stream, NavigationService service)
        {
            NavigationService = service;
            FileStream = stream;

            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        private void Execute_GoBack(object sender)
        {
            FileStream.Close();
            NavigationService.GoBack();
        }
    }
}
