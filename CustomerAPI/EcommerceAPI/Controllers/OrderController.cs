using Ecommerce.DAL.Domain;
using Ecommerce.Repositories.DTO;
using Ecommerce.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderRepository orderRepository, ILogger<OrderController> logger)
        {
            this.orderRepository = orderRepository;
            _logger = logger;
        }
        [HttpGet()]
        [Route("{orderId:int}")]
        public async Task<IActionResult> GetOrderByOrderId([FromRoute] int orderId)
        {
            var orderDomain = await orderRepository.GetOrderByIdAsync(orderId);

            if (orderDomain == null)
            {
                return NotFound();
            }
            
            return Ok(orderDomain);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            try
            {
                var orderDto = await orderRepository.DeleteOrderAsync(id);

                if (orderDto == null)
                {
                    return NotFound();
                }
                _logger.Log(LogLevel.Information, "Order {id} deleted Successfully", orderDto.Id);
                return Ok("Order deleted Successfully");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500, "Order can not be deleted");
            }

        }
    }
}
