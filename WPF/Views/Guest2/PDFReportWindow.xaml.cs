using Syncfusion.Pdf.Graphics;
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
using System.Windows.Shapes;

namespace SOSTeam.TravelAgency.WPF.Views.Guest2
{
    /// <summary>
    /// Interaction logic for PDFReportWindow.xaml
    /// </summary>
    public partial class PDFReportWindow : Window
    {
        public PDFReportWindow(string pdfPath)
        {
            InitializeComponent();
            DataContext = this;

            FileStream stream = new FileStream(pdfPath, FileMode.Open);
            PdfViewer.Load(stream);
        }
    }
}
