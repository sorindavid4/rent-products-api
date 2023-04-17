using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rent_products_api.DataLayer.DTOs.Rents;
using rent_products_api.ServiceLayer.RentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.Controllers
{
    [ApiController]
    public class RentController : BaseController
    {

        private readonly IRentService _rentService;

        public RentController(IRentService rentService)
        {
            _rentService = rentService;
        }


        [HttpPost("/Rents/RentProduct")]
        public async Task<IActionResult> RentProduct([FromBody] RentProductDTO rentDTO)
        {
            var result = await _rentService.RentProduct(rentDTO);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("/Rents/GetMyRents")]
        public async Task<IActionResult> GetMyRents([FromQuery] Guid userId)
        {
            var result = await _rentService.GetMyRents(userId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("/Rents/GetProductRentedTimes")]
        public async Task<IActionResult> GetProductRentedTimes([FromQuery] Guid productId)
        {
                var result = await _rentService.GetProductRentedTimes(productId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Rents/ConfirmRent")]
        public async Task<IActionResult> ConfirmRent([FromForm] Guid rentId)
        {
            var result = await _rentService.ConfirmRent(rentId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Rents/ConfirmRentPayment")]
        public async Task<IActionResult> ConfirmRentPayment([FromForm] Guid productId)
        {
            var result = await _rentService.ConfirmRentPayment(productId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Rents/CancelRent")]
        public async Task<IActionResult> CancelRent([FromForm] Guid rentId)
        {
            var result = await _rentService.CancelRent(rentId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Rents/UnconfirmRent")]
        public async Task<IActionResult> UnconfirmRent([FromForm] Guid productId)
        {
            var result = await _rentService.UnconfirmRent(productId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Rents/UncancelRent")]
        public async Task<IActionResult> UncancelRent([FromForm] Guid productId)
        {
            var result = await _rentService.UncancelRent(productId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

       
    }
}
