using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Utils
{
    public enum UserType
    {
        AdminUser = 1,
        SimpleUser = 2
    }

    public static class MessageType
    {
        public static string Error { get { return "error"; } }
        public static string Success { get { return "success"; } }
        public static string Info { get { return "info"; } }
    }
    public enum PaymentType
    {
        Online = 1,
        Cash = 2,
    }
    public enum RentType
    {
        WholeDay=1,
        FewHours=2,
    }

    public enum Months
    {
        None = 0,
        Ianuarie = 1,
        Februarie = 2,
        Martie = 3,
        Aprilie = 4,
        Mai = 5,
        Iunie = 6,
        Iulie = 7,
        August = 8,
        Septembrie = 9,
        Octombrie = 10,
        Noiembrie = 11,
        Decembrie = 12
    }

    public enum RentStatus
    {
        Rented = 1,
        Confirmed =2,
        Rejected =3,
        PaymentConfirmed =4,
        WaitingForOnlinePayment =5,
    }
}
