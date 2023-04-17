using rent_products_api.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.DTOs.User
{
    public class AdminUserDTO : BaseUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
