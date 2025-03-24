using MyAspNetBackend.DTOs;
using MyAspNetBackend.Models;

namespace MyAspNetBackend.Repositories
{
    public interface ICustomerRepository
    {
        Task<PagedResult<CustomerDTO>> GetAllCustomers(int page, int limit);
        Task<CustomerDTO?> GetCustomerById(int id);
        Task<Customer?> UpdateCustomer(int id, UpdateRequest customer);
        Task<bool> DeleteCustomer(int id);
    }
}
