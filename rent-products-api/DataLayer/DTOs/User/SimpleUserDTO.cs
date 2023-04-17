using rent_products_api.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.DTOs.User
{
    public class SimpleUserDTO
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string CreatedAt { get; set; }
        public string? Updated { get; set; }
        public UserType UserType { get; set; }
        public bool IsVerified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
        public string NumarTelefon { get; set; }
    }
}
