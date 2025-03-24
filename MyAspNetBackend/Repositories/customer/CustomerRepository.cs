using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAspNetBackend.Data;
using MyAspNetBackend.DTOs;
using MyAspNetBackend.Models;

namespace MyAspNetBackend.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CustomerRepository(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<CustomerDTO>> GetAllCustomers(int page = 1, int limit = 10)
        {
            var customersQuery = _context.Customers.AsQueryable();
            
            var paginator = new Paginator<Customer>(page, limit);
            
            var pagedResult = await paginator.GetPagedDataAsync(customersQuery);
            
            var pagedDtoResult = new PagedResult<CustomerDTO>
            {
                Items = _mapper.Map<List<CustomerDTO>>(pagedResult.Items),
                Metadata = pagedResult.Metadata
            };
    
            return pagedDtoResult;
        }

        public async Task<CustomerDTO?> GetCustomerById(int id)
        {
            var customers =  await _context.Customers.FindAsync(id);
            return _mapper.Map<CustomerDTO?>(customers);
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> UpdateCustomer(int id, UpdateRequest customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
                return null;

            if (!string.IsNullOrEmpty(customer.full_name))
                existingCustomer.FullName = customer.full_name;

            if (!string.IsNullOrEmpty(customer.phone_number))
                existingCustomer.PhoneNumber = customer.phone_number;

            if (!string.IsNullOrEmpty(customer.email))
                existingCustomer.Email = customer.email;

            if (!string.IsNullOrEmpty(customer.password))
                existingCustomer.Password = BCrypt.Net.BCrypt.HashPassword(customer.password);

            if (!string.IsNullOrEmpty(customer.address))
                existingCustomer.Address = customer.address;


            if (customer.role != null) // Kiểm tra kiểu dữ liệu bool nullable
                existingCustomer.Role = customer.role.Value;
            
            await _context.SaveChangesAsync();
            return existingCustomer;
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
