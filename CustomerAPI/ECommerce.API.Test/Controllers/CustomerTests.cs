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
    public class CustomerTests
    {
        private Mock<ICustomerRepository> customerRepository = null;
        private CustomerController customerController = null;
        private Mock<ILogger<CustomerController>> _logger = null;
        private readonly CustomerDto customerDto;
        private readonly AddCustomerDto addCustomerDto;
        private readonly UpdateCustomerDto updateCustomerDto;
        private readonly CustomerOrderDto customerOrderDto;
        public CustomerTests()
        {
            customerDto = new CustomerDto()
            {
                Id = 1,
                FirstName = "Maria",
                LastName = "Anders",
                City = "Berlin",
                Country = "Germany",
                Phone = "030-0074321"
            };


            addCustomerDto = new AddCustomerDto()
            {
                FirstName = "Maria",
                LastName = "Anders",
                City = "Berlin",
                Country = "Germany",
                Phone = "030-0074321"
            };

            updateCustomerDto = new UpdateCustomerDto()
            {
                Id = 1,
                FirstName = "Maria",
                LastName = "Anders",
                City = "Berlin",
                Country = "Germany",
                Phone = "030-0074321"
            };

            customerOrderDto = new CustomerOrderDto()
            {
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

            customerRepository = new Mock<ICustomerRepository>();
            _logger = new Mock<ILogger<CustomerController>>();
            customerController = new CustomerController(customerRepository.Object, _logger.Object);
        }

        [TestMethod()]
        public async Task GetAllCustomers_WhenSuccess_ReturnsCustomersDTOListTest()
        {
            var customersDto = new List<CustomerDto>() { customerDto, customerDto };

            customerRepository.Setup(t => t.GetAllAsync()).ReturnsAsync(customersDto);
            var result = await customerController.GetAll();
            
            if (result != null)
            {
                var actualResult = result as ObjectResult;
                var expectedResult = (List<CustomerDto>)actualResult.Value;
                Assert.IsNotNull(result);
                Assert.AreEqual(2, expectedResult.Count);
            }
            
        }

        [TestMethod()]
        public async Task GetAllCustomers_WhenCustomerDoesNotExist_ThrowsUserNotFoundExceptionTest()
        {
            customerRepository.Setup(t => t.GetAllAsync());
            var result = await customerController.GetAll();
            var actualResult = result as ObjectResult;
            Assert.IsNull(actualResult);
            
        }

       
        [TestMethod()]
        public async Task CreateCustomerTest()
        {
            customerRepository.Setup(t => t.CreateCustomerAsync(It.IsAny<CustomerDto>())).ReturnsAsync(customerDto);
            var result = await customerController.CreateCustomer(addCustomerDto);

            var actualResult = result as ObjectResult;
            var expectedResult = (CustomerDto)actualResult.Value;
            Assert.AreEqual("Maria",expectedResult.FirstName);
        }

        [TestMethod()]
        public async Task UpdateCustomerTest()
        {
            int id = 12;
            //customerRepository.Setup(t => t.CreateCustomerAsync(new CustomerDto { FirstName = "test", LastName = "test", Id = 12 })).ReturnsAsync(new CustomerDto { FirstName = "test", LastName = "test", Id = 12 });
            customerRepository.Setup(t => t.UpdateCustomerAsync(id, It.IsAny<CustomerDto>())).ReturnsAsync(customerDto);
            var result = await customerController.UpdateCustomer(id, updateCustomerDto);

            var actualResult = result as ObjectResult;
            var expectedResult = (CustomerDto)actualResult.Value;
            Assert.AreEqual("Anders",expectedResult.LastName);
        }

        [TestMethod()]
        public async Task DeleteCustomerTest()
        {
            int id = 12;
            
            customerRepository.Setup(t => t.DeleteCustomerAsync(id)).ReturnsAsync(customerDto);

            //customerController = new CustomerController(customerRepository.Object, _logger.Object);

            var result = await customerController.DeleteCustomer(id);
           
            var actualResult = result as ObjectResult;
            var expectedResult = actualResult.Value.ToString();
            Assert.AreEqual("Customer deleted successfully", expectedResult);
        }

        [TestMethod()]
        public async Task GetCustomer_WhenSuccess_ReturnsCustomerDTOListTest()
        {
            var id = 1;
            customerRepository.Setup(t => t.GetCustomerByIdAsync(id)).ReturnsAsync(customerDto);
            var result = await customerController.GetCustomerById(id);
            var actualResult = result as ObjectResult;
            var expectedResult = (CustomerDto)actualResult.Value;
            Assert.AreEqual(1, expectedResult.Id);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public async Task GetCustomer_WhenCustomerDoesNotExist_ThrowsUserNotFoundExceptionTest()
        {
            var id = 1;
            customerRepository.Setup(t => t.GetCustomerByIdAsync(id));
            var result = await customerController.GetCustomerById(id);
            var actualResult = result as ObjectResult;
            Assert.IsNull(actualResult);
        }

        [TestMethod()]
        public async Task GetOrderbyCustomerIdTest()
        {
            var customerOrders = new List<CustomerOrderDto>() { customerOrderDto, customerOrderDto };
            var customerId = 1;
            customerRepository.Setup(t => t.GetOrderbyCustomerIdAsync(customerId)).ReturnsAsync(customerOrders);
            var result = await customerController.GetOrderbyCustomerId(customerId);
            var actualResult = result as ObjectResult;
            var expectedResult = (List<CustomerOrderDto>)actualResult.Value;
            
            Assert.AreEqual(2, expectedResult.Count);
        }

        [TestMethod()]
        public async Task CreateCustomerOrderTest()
        {
            customerRepository.Setup(t => t.CreateOrderToCustomerAsync(It.IsAny<CustomerOrderDto>())).ReturnsAsync(customerOrderDto);
            var result = await customerController.CreateCustomerOrder(customerOrderDto);

            var actualResult = result as ObjectResult;
            var expectedResult = (CustomerOrderDto)actualResult.Value;
            Assert.AreEqual(2, expectedResult.OrderItems.Count);
        }
               
    }
}
