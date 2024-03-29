﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Utils
{
    public class AppSettings
    {
        public string Secret { get; set; }

        // refresh token time to live (in days), inactive tokens are
        // automatically deleted from the database after this time
        public int RefreshTokenTTL { get; set; }

        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string MailBaseUrl { get; set; }
        public string AtvImageUploadPath { get; set; }
        public string NetopiaSignature { get; set; }
        public string NetopiaUrl { get; set; }
    }
}
