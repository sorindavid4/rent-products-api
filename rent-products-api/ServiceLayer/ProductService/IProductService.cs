using rent_products_api.DataLayer.DTOs.Product;
using rent_products_api.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.ServiceLayer.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDTO>>> GetProducts();
        Task<ServiceResponse<Object>> AddProduct(AddProductDTO productDTO);

        Task<ServiceResponse<Object>> DeleteProduct(Guid productId);
        Task<ServiceResponse<ProductDTO>> GetProductDetails(Guid productId);

    }
}
