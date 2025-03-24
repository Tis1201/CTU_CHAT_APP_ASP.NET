using MyAspNetBackend.DTOs;
using MyAspNetBackend.Models;
using MyAspNetBackend.Repositories;

namespace MyAspNetBackend.Services
{
    public interface ICustomerService
    {
        Task<PagedResult<CustomerDTO>> GetAllCustomers(int page, int limit);
        Task<CustomerDTO?> GetCustomerById(int id);
        Task<Customer?> UpdateCustomer(int id, UpdateRequest customer);
        Task<bool> DeleteCustomer(int id);
    }
}