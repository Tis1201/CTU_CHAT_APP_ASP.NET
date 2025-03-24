using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAspNetBackend.DTOs;
using MyAspNetBackend.Models;
using MyAspNetBackend.Responses;
using MyAspNetBackend.Services;


namespace MyAspNetBackend.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetCustomers(int page, int limit)
        {
            try
            {
                var customers = await _customerService.GetAllCustomers(page, limit);
                return Ok(ApiResponse<object>.Success(customers));
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while fetching the customers."));
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CustomerDTO>>> GetCustomer(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerById(id);
                if (customer == null)
                {
                    return NotFound(ApiResponse<object>.Fail("Customer not found."));
                }

                return Ok(ApiResponse<CustomerDTO>.Success(customer));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while fetching the customer."));
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, [FromBody] UpdateRequest customer)
        {
            try
            {
                var updatedCustomer = await _customerService.UpdateCustomer(id, customer);
                if (updatedCustomer == null)
                {
                    return NotFound(ApiResponse<object>.Fail("Customer not found or no fields updated."));
                }

                return Ok(ApiResponse<Customer>.Success(updatedCustomer));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while updating the customer."));
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var deleted = await _customerService.DeleteCustomer(id);
                if (!deleted)
                {
                    return NotFound(ApiResponse<object>.Fail("Customer not found."));
                }

                return Ok(ApiResponse<bool>.Success(deleted, "Deleted success"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while deleting the customer."));
            }
        }
    }
}
