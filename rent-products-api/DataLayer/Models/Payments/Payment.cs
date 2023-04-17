using rent_products_api.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Models.Payments
{
    public class Payment
    {
       public Guid PaymentId { get; set; }
       public int Amount { get; set; }

        public bool PaymentConfirmed { get; set; }
        public Guid UserPayingId { get; set; }
        public User UserPaying { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string PaymentStatus { get; set; }
    }
}
