using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class VouchersViewModel : ViewModel
    {
        public string Message { get; set; }
        public string VoucherName { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int VoucherId { get; set; }

        public VouchersViewModel(int voucherId,DateOnly expiryDate)
        {
            VoucherId = voucherId;
            ExpiryDate = expiryDate;
            Message = "Cestitamo, osvojili ste vaucer! Vaucer vazi do " + ExpiryDate.ToString();
            VoucherName = "Vaucer " + VoucherId.ToString();
        }
    }
}
