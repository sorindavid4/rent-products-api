using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.DTOs.Email
{
    public class EmailsInQueueDTO
    {
        public MimeMessage Email { get; set; }
        public int TryCount { get; set; }

        public EmailsInQueueDTO(MimeMessage email)
        {
            Email = email;
            TryCount = 5;
        }
    }
}
