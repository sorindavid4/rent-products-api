using rent_products_api.DataLayer.Models.Payments;
using rent_products_api.DataLayer.Utils;
using rent_products_api.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Models
{
    public class Rent
    {
        public Guid RentId { get; set; }
        public DateTime RentedDate { get; set; }
        public TimeSpan StartingHour { get; set; }
        public TimeSpan EndingHour { get; set; }
        public PaymentType PaymentType { get; set; }
        public RentType RentType { get; set; }
        public RentStatus Status { get; set; }

        public Guid ProductId { get; set; }
        public Models.Product.Product Product { get; set; }
        public Guid RentedByUserId { get; set; }
        public User RentedByUser { get; set; }

        public Guid? PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public DateTime RejectionTime { get; set; }
        public DateTime ConfirmationTime { get; set; }


    }
}
