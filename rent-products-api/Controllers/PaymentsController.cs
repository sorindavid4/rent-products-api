using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rent_products_api.DataLayer.DTOs.Payments;
using rent_products_api.ServiceLayer.PaymentService;
using System;
using System.Threading.Tasks;

namespace rent_products_api.Controllers
{
    [ApiController]
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("/Payment/GoToPayment")]
        public async Task<IActionResult> GoToPayment([FromBody] GoToPaymentDTO payment)
        {
            var origin = Request.Scheme + "://" + Request.Host;

            var paymentDTO = new GoToPaymentDTO
            {
                UserId = Account.UserId,
                PaymentType = payment.PaymentType,
                Email = payment.Email,
                RentId = payment.RentId,
                PhoneNumber = payment.PhoneNumber,
            };

            var result = await _paymentService.GoToPayment(paymentDTO, origin);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);

        }


        [HttpPost("/Payment/AddOrUpdatePayment")]
        public async Task<IActionResult> AddOrUpdatePayment([FromForm] string env_key, [FromForm] string data)
        {

            var result = await _paymentService.AddOrUpdatePayment(env_key, data);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);

        }
        [HttpGet("/Payments/GetPayments")]
        public async Task<IActionResult> GetPayments([FromQuery] Guid userId)
        {
            var result = await _paymentService.GetPayments(userId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
