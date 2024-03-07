using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for ShowPDFPage.xaml
    /// </summary>
    public partial class ShowPDFPage : Page
    {
        public ShowPDFPage(string pdfPath, NavigationService service)
        {
            InitializeComponent();
            FileStream stream = new FileStream(pdfPath, FileMode.Open);
            ShowPDFViewModel viewModel = new ShowPDFViewModel(stream, service);
            DataContext = viewModel;
            PdfViewer.Load(stream);
        }
    }
}
