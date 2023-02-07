 using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
        }

        [HttpGet("{userName}", Name ="GetBasket")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            try
            {
                var basket = await _basketRepository.GetBasket(userName);
                return Ok(basket ?? new ShoppingCart(userName));
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
        
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> updateBasket([FromBody] ShoppingCart basket)
        {
            try
            {
                foreach(var item in basket.Items)
                {
                    var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                    item.Price -= coupon.Amount;
                }
                return Ok(await _basketRepository.UpdateBasket(basket));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        [HttpDelete("{userName}", Name ="DeleteBasket")]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            try
            {
                await _basketRepository.DeleteBasket(userName);
                return Ok("item was deleted");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
