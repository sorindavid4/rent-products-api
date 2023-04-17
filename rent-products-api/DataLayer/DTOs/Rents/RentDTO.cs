using rent_products_api.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.DTOs.Rents
{
    public class RentDTO
    {
        public Guid RentId { get; set; }
        public Guid ProductId { get; set; }
        public string RentedDate { get; set; }
        public string ProductName { get; set; }
        public RentStatus Status { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public PaymentType PaymentType { get; set; }
        public RentType RentType { get; set; }
        public bool Timed =>  RentType == RentType.FewHours ;
        public Guid? PaymentId { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
    
    }
}
