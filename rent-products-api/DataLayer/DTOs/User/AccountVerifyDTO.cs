using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.DTOs.User
{
    public class AccountVerifyDTO
    {
        public string Token { get; set; }
        
    }
}
