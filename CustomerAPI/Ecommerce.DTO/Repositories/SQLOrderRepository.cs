using Ecommerce.DAL.DBContext;
using Ecommerce.DAL.Domain;
using Ecommerce.Repositories.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repositories.Repositories
{
    public class SQLOrderRepository : IOrderRepository
    {
        private readonly CustomerDBContext dbContext;
        public SQLOrderRepository(CustomerDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CustomerOrderDto?> GetOrderByIdAsync(int orderId)
        {
            var orderDto = new CustomerOrderDto();
            var orderDomain = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (orderDomain != null)
            {
                orderDomain.OrderItems = dbContext.OrderItems.Where(x => x.OrderId == orderDomain.Id).ToList();
                var orderItemsDomain = new List<OrderItemDto>();

                foreach (var orderItems in orderDomain.OrderItems)
                {
                    var orderItem = new OrderItemDto()
                    {
                        OrderId = orderItems.OrderId,
                        ProductId = orderItems.ProductId,
                        Quantity = orderItems.Quantity,
                        UnitPrice = orderItems.UnitPrice
                    };
                    orderItemsDomain.Add(orderItem);
                }
                orderDto = new CustomerOrderDto()
                {
                    Id = orderDomain.Id,
                    OrderDate = orderDomain.OrderDate,
                    OrderNumber = orderDomain.OrderNumber,
                    CustomerId = orderDomain.CustomerId,
                    OrderItems = orderItemsDomain
                };
            }
            return orderDto;
        }

        public async Task<OrderDto?> DeleteOrderAsync(int id)
        {
            var orderDomain = await dbContext.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(x => x.Id == id);

            if (orderDomain == null)
            {
                return null;
            }
            dbContext.Orders.Remove(orderDomain);
            int result = await dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return new OrderDto { Id = orderDomain.Id };
            }
            else
            {
                return null;
            }
        }

    }
}
