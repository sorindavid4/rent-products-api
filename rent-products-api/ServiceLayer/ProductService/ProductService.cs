using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using rent_products_api.DataLayer.DTOs.Product;
using rent_products_api.DataLayer.Models.Product;
using rent_products_api.DataLayer.Utils;
using rent_products_api.DBContexts;
using rent_products_api.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.ServiceLayer.ProductService
{
    public class ProductService : IProductService
    {
        private readonly MainDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        public ProductService(MainDbContext context, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<ServiceResponse<object>> AddProduct(AddProductDTO productDTO)
        {
            try
            {
                var files = new List<ProductImage>();

                if (productDTO.Images != null)
                {
                    foreach (var file in productDTO.Images)
                    {
                        var image = _mapper.Map<ProductImage>(file);
                        image.Data = getBlobFromFile(file);
                        files.Add(image);
                    }
                }

                var product = _mapper.Map<Product>(productDTO);
                product.Images = files;
                _context.Products.Add(product);

                _context.SaveChanges();


                return new ServiceResponse<object> { Success = true, Message = Messages.Message_AddProductSuccess };
            }
            catch (Exception e)
            {
                return new ServiceResponse<object> { Success = false, Message = Messages.Message_AddProductError };
            }
        }

        private byte[] getBlobFromFile(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

            public async Task<ServiceResponse<object>> DeleteProduct(Guid productId)
        {
            try
            {
                var product = _context.Products.Include(x=>x.Images).FirstOrDefault(x => x.ProductId == productId);

                _context.Products.Remove(product);
                _context.ProductImages.RemoveRange(product.Images);

                _context.SaveChanges();

                return new ServiceResponse<object> { Success = true, Message = Messages.Message_DeleteProductSuccess };
            }
            catch (Exception e)
            {
                return new ServiceResponse<object> { Success = false, Message = Messages.Message_DeleteProductError };
            }
        }

        public async Task<ServiceResponse<ProductDTO>> GetProductDetails(Guid productId)
        {
            try
            {
                var products = _mapper.ProjectTo<ProductDTO>(_context.Products.Where(x => x.ProductId == productId)).FirstOrDefault();



                return new ServiceResponse<ProductDTO> { Response = products, Success = true };
            }
            catch (Exception)
            {
                return new ServiceResponse<ProductDTO> { Success = false, Message = Messages.Message_GetProductsError };
            }
        }

        public async Task<ServiceResponse<List<ProductDTO>>> GetProducts()
        {
            try
            {
                var products = _mapper.ProjectTo<ProductDTO>(_context.Products).ToList();

                return new ServiceResponse<List<ProductDTO>> { Response = products, Success = true };
            }
            catch (Exception)
            {
                return new ServiceResponse<List<ProductDTO>> { Success = false, Message = Messages.Message_GetProductsError };
            }
        }
    }
}
