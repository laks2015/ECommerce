using Ecommerce.Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repositories.Repositories
{
    public interface IOrderRepository
    {
        Task<CustomerOrderDto?> GetOrderByIdAsync(int orderId);
        Task<OrderDto?> DeleteOrderAsync(int id);
    }
}
