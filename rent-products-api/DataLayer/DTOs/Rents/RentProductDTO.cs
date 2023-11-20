using rent_products_api.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.DTOs.Rents
{
    public class RentProductDTO
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid RentedByUserId { get; set; }
        public string Username { get; set; }
        [Required]
        public string RentedDate { get; set; }
        public string StartingHour { get; set; }
        public string EndingHour { get; set; }
        [Required]
        public RentType RentType { get; set; }
        [Required]
        public PaymentType PaymentType { get; set; }

    }
}
