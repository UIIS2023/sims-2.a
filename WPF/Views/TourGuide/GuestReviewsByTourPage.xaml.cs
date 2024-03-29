﻿using System;
using System.Collections.Generic;
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
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;

namespace SOSTeam.TravelAgency.WPF.Views.TourGuide
{
    /// <summary>
    /// Interaction logic for FinishedTourReviewsPage.xaml
    /// </summary>
    public partial class GuestReviewsByTourPage : Page
    {
        public GuestReviewsByTourPage(User loggedUser)
        {
            InitializeComponent();
            DataContext = new GuestReviewsByTourViewModel(loggedUser);
        }
    }
}
