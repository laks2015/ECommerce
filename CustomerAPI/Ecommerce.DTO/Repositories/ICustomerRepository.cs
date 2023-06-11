using Ecommerce.DAL.Domain;
using Ecommerce.Repositories.DTO;

namespace Ecommerce.Repositories.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(int id);
        Task<CustomerDto> CreateCustomerAsync(CustomerDto customer);
        Task<CustomerDto?> UpdateCustomerAsync(int id, CustomerDto customer);
        Task<CustomerDto?> DeleteCustomerAsync(int id);
        Task<List<CustomerOrderDto?>> GetOrderbyCustomerIdAsync(int CustomerId);
        Task<CustomerOrderDto> CreateOrderToCustomerAsync(CustomerOrderDto customerOrderDto);
    }
}
