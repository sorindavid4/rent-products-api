using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Models.Product
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PricePerHour { get; set; }
        public int PricePerDay { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public ICollection<Rent> Rents { get; set; }
    }
}
