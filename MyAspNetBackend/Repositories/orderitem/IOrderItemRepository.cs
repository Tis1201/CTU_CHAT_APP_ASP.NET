using MyAspNetBackend.DTOs.order;
using MyAspNetBackend.Models;

namespace MyAspNetBackend.Repositories.orderitem;

public interface IOrderItemRepository
{
    Task<PagedResult<OrderItemDto>> GetAllOrders(int page, int limit,int? customerId = null, bool status = false);
    Task<PagedResult<OrderItemDto>> GetAllOrdersAdmin(int page, int limit);
    Task<OrderItem?> GetOrderItemById(int id);
    
    Task<OrderItem> AddOrderItem(CreateRequest order);
    
    Task<OrderItem?> UpdateOrder(int id, UpdateRequest order);
    Task<bool> DeleteOrder(int id);
}