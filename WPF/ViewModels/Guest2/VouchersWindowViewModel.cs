using Microsoft.Win32;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class VouchersWindowViewModel : ViewModel
    {
        public User LoggedInUser { get; set; }
        public event EventHandler CloseRequested;
        private RelayCommand _backCommand;
        private VoucherService _voucherService;
        public static ObservableCollection<VouchersViewModel> Vouchers { get; set; }

        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        private RelayCommand _helpCommand;

        public RelayCommand HelpCommand
        {
            get { return _helpCommand; }
            set
            {
                _helpCommand = value;
            }
        }

        public VouchersWindowViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            BackCommand = new RelayCommand(Execute_CancelCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand, CanExecuteMethod);
            Vouchers = new ObservableCollection<VouchersViewModel>();
            _voucherService = new VoucherService();
            FillVouchersForShowingList();
            LoggedInUser = loggedInUser;
        }
        private void Execute_HelpCommand(object obj)
        {
            HelpWindow window = new HelpWindow();
            window.Show();
        }

        private void FillVouchersForShowingList()
        {
            foreach(Voucher voucher in _voucherService.GetAll())
            {
                if(voucher.ExpiryDate > DateOnly.FromDateTime(DateTime.Today) && voucher.Used == false && voucher.UserId == LoggedInUser.Id)
                {
                    Vouchers.Add(new VouchersViewModel(voucher.Id,voucher.ExpiryDate));
                }
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void Execute_CancelCommand(object sender)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
