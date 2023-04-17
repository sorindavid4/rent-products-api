using Microsoft.EntityFrameworkCore;
using rent_products_api.Models;
using rent_products_api.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Utils
{
    [Owned]
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public BaseUser Account { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => GenericFunctions.GetCurrentDateTime() >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
