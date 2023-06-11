using Ecommerce.DAL.Domain;
using Ecommerce.Repositories.DTO;
using Microsoft.EntityFrameworkCore;
using Ecommerce.DAL.DBContext;

namespace Ecommerce.Repositories.Repositories
{
    public class SQLCustomerRepository : ICustomerRepository
    {
        private readonly CustomerDBContext dbContext;
        public SQLCustomerRepository(CustomerDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customer)
        {
            var customerDomain = new Customer()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                City = customer.City,
                Country = customer.Country,
                Phone = customer.Phone
            };
            await dbContext.Customers.AddAsync(customerDomain);
            await dbContext.SaveChangesAsync();
            customer.Id = customerDomain.Id;
            return customer;
        }

        public async Task<CustomerDto?> DeleteCustomerAsync(int id)
        {
            var customerDomain = await dbContext.Customers.Include(o=>o.Orders).FirstOrDefaultAsync(x => x.Id == id);

            if (customerDomain == null)
            {
                return null;
            }
            dbContext.Customers.Remove(customerDomain);
            int result = await dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return new CustomerDto { Id = customerDomain.Id };
            }
            else
            { 
                return null; 
            }
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customersDto = new List<CustomerDto>();
            var customersDomain = await dbContext.Customers.ToListAsync();


            foreach (var customerDomain in customersDomain)
            {
                customersDto.Add(new CustomerDto()
                {
                    Id = customerDomain.Id,
                    FirstName = customerDomain.FirstName,
                    LastName = customerDomain.LastName,
                    City = customerDomain.City,
                    Country = customerDomain.Country,
                    Phone = customerDomain.Phone
                });
            }

            return customersDto;
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            var customerDomain = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (customerDomain != null)
            {
                var customerDto = new CustomerDto()
                {
                    Id = customerDomain.Id,
                    FirstName = customerDomain.FirstName,
                    LastName = customerDomain.LastName,
                    City = customerDomain.City,
                    Country = customerDomain.Country,
                    Phone = customerDomain.Phone
                };
                return customerDto;
            }
            return null;
        }

        public async Task<CustomerDto?> UpdateCustomerAsync(int id, CustomerDto customer)
        {

            var customerDomain = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customerDomain == null)
            {
                return null;
            }

            customerDomain.FirstName = customer.FirstName;
            customerDomain.LastName = customer.LastName;
            customerDomain.City = customer.City;
            customerDomain.Country = customer.Country;
            customerDomain.Phone = customer.Phone;
            await dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<List<CustomerOrderDto?>> GetOrderbyCustomerIdAsync(int customerId)
        {

            var customerOrdersDomain = await dbContext.Orders.Where(x => x.CustomerId == customerId).ToListAsync();
            var customerOrderDto = new List<CustomerOrderDto?>();

            foreach (var customerOrderDomain in customerOrdersDomain)
            {
                customerOrderDomain.OrderItems = await dbContext.OrderItems.Where(x => x.OrderId == customerOrderDomain.Id).ToListAsync();
                var customerOrderItemsDto = new List<OrderItemDto>();
                foreach (var customerOrderItemDto in customerOrderDomain.OrderItems)
                {
                    customerOrderItemsDto.Add(new OrderItemDto
                    {
                        Id = customerOrderItemDto.Id,
                        OrderId = customerOrderItemDto.OrderId,
                        ProductId = customerOrderItemDto.ProductId,
                        UnitPrice = customerOrderItemDto.UnitPrice,
                        Quantity = customerOrderItemDto.Quantity
                    });
                }
                customerOrderDto.Add(
                    new CustomerOrderDto
                    {
                        Id = customerOrderDomain.Id,
                        OrderDate = customerOrderDomain.OrderDate,
                        OrderNumber = customerOrderDomain.OrderNumber,
                        CustomerId = customerOrderDomain.CustomerId,
                        TotalAmount = customerOrderDomain.TotalAmount,
                        OrderItems = customerOrderItemsDto
                    });
            }
            return customerOrderDto;
        }

        public async Task<CustomerOrderDto> CreateOrderToCustomerAsync(CustomerOrderDto customerOrderDto)
        {
            var orderItemsDomain = new List<OrderItem>();
            if (customerOrderDto.OrderItems != null)
            {
                foreach (var orderItems in customerOrderDto.OrderItems)
                {
                    var orderItem = new OrderItem()
                    {
                        ProductId = orderItems.ProductId,
                        Quantity = orderItems.Quantity,
                        UnitPrice = orderItems.UnitPrice
                    };
                    orderItemsDomain.Add(orderItem);
                }

            }
            var orderDomain = new Order()
            {
                OrderDate = customerOrderDto.OrderDate,
                OrderNumber = customerOrderDto.OrderNumber,
                CustomerId = customerOrderDto.CustomerId,
                TotalAmount = customerOrderDto.TotalAmount,
                OrderItems = orderItemsDomain
            };
            await dbContext.Orders.AddAsync(orderDomain);

            var id = await dbContext.SaveChangesAsync();


            return customerOrderDto;
        }
    }
}
