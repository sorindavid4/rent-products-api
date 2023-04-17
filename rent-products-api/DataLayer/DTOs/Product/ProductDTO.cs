using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.DTOs.Product
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PricePerDay { get; set; }
        public int PricePerHour { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
