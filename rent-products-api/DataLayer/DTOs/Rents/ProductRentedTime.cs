using rent_products_api.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.DTOs.Rents
{
    public class ProductRentedTime
    {
        public Guid ProductId { get; set; }
        public string RentedDate { get; set; }
        public string StartingHour { get; set; }
        public string EndingHour { get; set; }
        public RentType RentType { get; set; }

    }
}
