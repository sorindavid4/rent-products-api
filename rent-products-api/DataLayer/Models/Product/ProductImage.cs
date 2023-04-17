using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Models.Product
{
    public class ProductImage
    {
        public Guid ProductImageId { get; set; }
        public string ImagePath { get; set; }
        public string ImageExtension { get; set; }
        public string ImageTitle { get; set; }
    }
}
