using rent_products_api.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.DTOs.Email
{
    public class EmailRentDetails
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string ATVname { get; set; }
        public RentType RentType { get; set; }
        public DateTime RentDate { get; set; }
        public TimeSpan StartingHour { get; set; }
        public TimeSpan EndingHour { get; set; }

    }
}
