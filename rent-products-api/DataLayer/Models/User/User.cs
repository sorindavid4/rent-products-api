using rent_products_api.DataLayer.Models;
using rent_products_api.DataLayer.Models.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.Models.User
{
    public class User : BaseUser
    {
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public ICollection<Rent> Rents { get; set; } 
        public ICollection<Payment> Payments { get; set; } 
    }
}
