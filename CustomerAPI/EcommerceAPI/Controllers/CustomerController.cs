using Ecommerce.DAL.Domain;
using Ecommerce.Repositories.DTO;
using Ecommerce.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ICustomerRepository customerRepository, ILogger<CustomerController> logger)
        //public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var customersDto = await customerRepository.GetAllAsync();
            if(customersDto==null)
            {
                return NotFound(); 
            }
            
            return Ok(customersDto);
        }


        [HttpGet()]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int id)
        {
            var customerDomain = await customerRepository.GetCustomerByIdAsync(id);

            if (customerDomain == null)
            {
                return NotFound();
            }

            return Ok(customerDomain);
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] AddCustomerDto addCustomerDto)
        {
            var customerDomain = new CustomerDto
            {
                FirstName = addCustomerDto.FirstName,
                LastName = addCustomerDto.LastName,
                City = addCustomerDto.City,
                Country = addCustomerDto.Country,
                Phone = addCustomerDto.Phone
            };

            var customerDto = await customerRepository.CreateCustomerAsync(customerDomain);
            
            return CreatedAtAction(nameof(GetCustomerById), new { id = customerDto.Id }, customerDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int id, [FromBody] UpdateCustomerDto updateCustomerDto)
        {
            var customerDomain = new CustomerDto
            {
                FirstName = updateCustomerDto.FirstName,
                LastName = updateCustomerDto.LastName,
                City = updateCustomerDto.City,
                Country = updateCustomerDto.Country,
                Phone = updateCustomerDto.Phone
            };
            customerDomain = await customerRepository.UpdateCustomerAsync(id, customerDomain);

            if (customerDomain == null)
            {
                return NotFound();
            }
                        
            return Ok(customerDomain);
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            try
            {
                var customerDomain = await customerRepository.DeleteCustomerAsync(id);

                if (customerDomain == null)
                {
                    return NotFound();
                }
                _logger.Log(LogLevel.Information, "Customer {id} deleted Successfully", customerDomain.Id);
                return Ok("Customer deleted successfully");
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500,"Customer can not be deleted");
            }
            
        }

        [HttpGet()]
        [Route("{customerId:int}")]
        public async Task<IActionResult> GetOrderbyCustomerId([FromRoute] int customerId)
        {
            var customerOrdersDomain = await customerRepository.GetOrderbyCustomerIdAsync(customerId);

            if (customerOrdersDomain == null)
            {
                return NotFound();
            }
                        
            return Ok(customerOrdersDomain);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerOrder([FromBody] CustomerOrderDto customerOrderDto)
        {

            var customerDetail = await customerRepository.CreateOrderToCustomerAsync(customerOrderDto);

            return CreatedAtAction(nameof(GetOrderbyCustomerId), new { id = customerOrderDto.CustomerId }, customerOrderDto);
        }
    }
}
