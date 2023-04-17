using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Utils
{
    public class ServiceResponse<T>
    {
        public T Response { get; set; }
        public bool Success { get; set; }
        public Message Message { get; set; }
    }
}
