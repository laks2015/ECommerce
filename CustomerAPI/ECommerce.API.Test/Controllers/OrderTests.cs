using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcommerceAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Repositories.Repositories;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Repositories.DTO;

namespace ECommerce.API.Tests
{
    [TestClass()]
    public class OrderTests
    {
        private Mock<IOrderRepository> orderRepository = null;
        private OrderController orderController = null;
        private Mock<ILogger<OrderController>> _logger = null;
        private readonly CustomerOrderDto customerOrderDto;
        private readonly OrderDto orderDto;
        public OrderTests()
        {
            customerOrderDto = new CustomerOrderDto()
            {
                Id=1,
                OrderDate = Convert.ToDateTime("2023-06-11T02:43:08.155Z"),
                OrderNumber = Guid.NewGuid().ToString(),
                CustomerId = 1,
                TotalAmount = 100,
                OrderItems = new List<OrderItemDto>()
                {
                    new OrderItemDto()
                    {
                        OrderId = 1,
                        ProductId = 1,
                        UnitPrice = 1,
                        Quantity = 3,
                    },
                     new OrderItemDto()
                    {
                        OrderId = 1,
                        ProductId = 2,
                        UnitPrice = 2,
                        Quantity = 2,
                    }
                }
            };
            orderDto = new OrderDto()
            {
                Id = 1,
                OrderDate = Convert.ToDateTime("2023-06-11T02:43:08.155Z"),
                OrderNumber = Guid.NewGuid().ToString(),
                CustomerId = 1,
                TotalAmount = 100,
            };
            orderRepository = new Mock<IOrderRepository>();
            _logger = new Mock<ILogger<OrderController>>();
            orderController = new OrderController(orderRepository.Object, _logger.Object);
        }
       
        [TestMethod()]
        public async Task GetOrderByOrderIdTest()
        {
            var orderId = 1;
            orderRepository.Setup(t => t.GetOrderByIdAsync(orderId)).ReturnsAsync(customerOrderDto);
            var result = await orderController.GetOrderByOrderId(orderId);
            var actualResult = result as ObjectResult;
            var expectedResult = (CustomerOrderDto?)actualResult.Value;
            if (expectedResult != null)
            {
                Assert.AreEqual(1, expectedResult.Id);
                Assert.AreEqual(2, expectedResult.OrderItems.Count);
            }
        }

        [TestMethod()]
        public async Task DeleteOrderTest()
        {
            int id = 12;
            //customerRepository.Setup(t => t.CreateCustomerAsync(new CustomerDto { FirstName = "test", LastName = "test", Id = 12 })).ReturnsAsync(new CustomerDto { FirstName = "test", LastName = "test", Id = 12 });
            orderRepository.Setup(t => t.DeleteOrderAsync(id)).ReturnsAsync(orderDto);

            //customerController = new CustomerController(customerRepository.Object, _logger.Object);

            var result = await orderController.DeleteOrder(id);

            var actualResult = result as ObjectResult;
            var expectedResult = actualResult.Value.ToString();
            Assert.AreEqual("Order deleted Successfully", expectedResult);
        }
    }
}