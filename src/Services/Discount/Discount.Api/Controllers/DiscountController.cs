using Discount.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Discount.Api.Entities;

namespace Discount.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
        

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            try
            {
                var discount = await _discountRepository.GetDiscount(productName);
                return Ok(discount);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            
            try
            {
                var discount = await _discountRepository.CreateDiscount(coupon);
                return CreatedAtRoute("GetDiscount", new {productName = coupon.ProductName}, coupon);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Coupon>> DeleteDiscount([FromBody] Coupon coupon)
        {
            try
            {
                var discount = await _discountRepository.DeleteDiscount(coupon.ProductName);
                return Ok(new
                {
                    message = "deleted successfully",
                    coupon = coupon,
                });
            }
            catch (Exception ex)
            {
    
                throw;
            }
        }

        [HttpPut]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
        {
            try
            {
                var discount = await _discountRepository.UpdateDiscount(coupon);
                return Ok(new
                {
                    message = discount,
                    coupon = coupon,
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
