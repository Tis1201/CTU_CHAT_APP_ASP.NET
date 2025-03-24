using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAspNetBackend.DTOs.order;
using MyAspNetBackend.Models;
using MyAspNetBackend.Responses;
using MyAspNetBackend.Services.order;

namespace MyAspNetBackend.Controllers
{
    [Route("api/v1/order_items")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOderItemService _orderItemService;

        public OrderItemController(IOderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }
        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetOrders(int page, int limit, bool status = false)
        {
            try
            {
                var customerIdClaim = User.FindFirst("customer_id")?.Value;

                if (string.IsNullOrEmpty(customerIdClaim))
                    return Unauthorized(ApiResponse<object>.Error("Token không hợp lệ."));

                var customerId = int.Parse(customerIdClaim);

                var orders = await _orderItemService.GetAllOrders(page, limit, customerId, status);
                return Ok(ApiResponse<object>.Success(orders));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while fetching the order."));
            }
        }

        [Authorize]
        [HttpGet("admin")]
        public async Task<ActionResult<ApiResponse<object>>> GetOrdersAdmin(int page, int limit)
        {
            try
            {
                var product = await _orderItemService.GetAllOrdersAdmin(page, limit);
                return Ok(ApiResponse<object>.Success(product));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while fetching the order."));
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetOrderById(int id)
        {
            try
            {
                var order = await _orderItemService.GetOrderItemById(id);
                if (order == null)
                {
                    return NotFound(ApiResponse<object>.Fail("Order not found."));
                }

                return Ok(ApiResponse<OrderItem>.Success(order));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while fetching the order with id: ", id));
            }
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] CreateRequest order)
        {
            try
            {
                
                var newOrder = await _orderItemService.AddOrderItem(order);

                if (newOrder == null)
                {
                    return BadRequest(ApiResponse<object>.Fail("Failed to add the order."));
                }

                
                return Ok(ApiResponse<object>.Success(newOrder));
            }
            catch (Exception e)
            {
                
                Console.WriteLine($"Error: {e.Message}");
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while adding the order. Please try again later."));
            }
        }   
        
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromBody] UpdateRequest product)
        {
            try
            {
                var updatedProduct = await _orderItemService.UpdateOrder(id, product);
                if (updatedProduct == null)
                {
                    return NotFound(ApiResponse<object>.Fail("Order not found or no fields updated."));
                }

                return Ok(ApiResponse<OrderItem>.Success(updatedProduct));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while updating the order."));
            }
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var deleted = await _orderItemService.DeleteOrder(id);
                if (!deleted)
                {
                    return NotFound(ApiResponse<object>.Fail("Order not found."));
                }

                return Ok(ApiResponse<bool>.Success(deleted, "Deleted success"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while deleting the order."));
            }
        }
    }
}
