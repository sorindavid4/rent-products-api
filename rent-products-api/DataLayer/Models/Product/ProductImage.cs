using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Models.Product
{
    public class ProductImage
    {
        public Guid ProductImageId { get; set; }

        public byte[] Data { get; set; }
        public string ImageExtension { get; set; }
        public string ImageTitle { get; set; }
    }
}
