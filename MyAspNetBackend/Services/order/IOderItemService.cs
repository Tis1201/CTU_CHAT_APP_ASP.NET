using MyAspNetBackend.DTOs.order;
using MyAspNetBackend.Models;
using MyAspNetBackend.Repositories;

namespace MyAspNetBackend.Services.order;

public interface IOderItemService
{
    Task<PagedResult<OrderItemDto>> GetAllOrders(int page, int limit,int? customerId = null, bool status = false);
    Task<PagedResult<OrderItemDto>> GetAllOrdersAdmin(int page, int limit);
    Task<OrderItem?> GetOrderItemById(int id);
    
    Task<OrderItem> AddOrderItem(CreateRequest order);
    
    Task<OrderItem?> UpdateOrder(int id, UpdateRequest order);
    Task<bool> DeleteOrder(int id);
}