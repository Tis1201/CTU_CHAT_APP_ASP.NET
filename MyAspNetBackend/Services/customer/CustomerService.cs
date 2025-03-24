using AutoMapper;
using MyAspNetBackend.DTOs;
using MyAspNetBackend.Models;
using MyAspNetBackend.Repositories;


namespace MyAspNetBackend.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;


        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

        }

        public async Task<PagedResult<CustomerDTO>> GetAllCustomers(int page, int limit)
        {
            return await _customerRepository.GetAllCustomers(page,limit);
        }

        public async Task<CustomerDTO?> GetCustomerById(int id)
        {
            return await _customerRepository.GetCustomerById(id);
        }
        
        public async Task<Customer?> UpdateCustomer(int id, UpdateRequest customer)
        {
            return await _customerRepository.UpdateCustomer(id, customer);
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            return await _customerRepository.DeleteCustomer(id);
        }
    }
}