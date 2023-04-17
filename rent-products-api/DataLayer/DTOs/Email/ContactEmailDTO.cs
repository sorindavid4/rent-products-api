using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.DTOs.Email
{
    public class ContactEmailDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public string RentDate { get; set; }
        public string AtvWanted { get; set; }
        public string RentPeriod { get; set; }
    }
}
