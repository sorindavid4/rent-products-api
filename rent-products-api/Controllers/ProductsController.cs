using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rent_products_api.DataLayer.DTOs.Product;
using rent_products_api.ServiceLayer.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.Controllers
{
    [ApiController]
    public class ProductsController : BaseController
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("/Products/AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDTO model)
        {
            var result = await _productService.AddProduct(model);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("/Products/GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _productService.GetProducts();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("/Products/GetProductDetails")]
        public async Task<IActionResult> GetProductDetails([FromQuery] Guid productId)
        {
            var result = await _productService.GetProductDetails(productId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Products/DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromForm] Guid productId)
        {
            var result = await _productService.DeleteProduct(productId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
