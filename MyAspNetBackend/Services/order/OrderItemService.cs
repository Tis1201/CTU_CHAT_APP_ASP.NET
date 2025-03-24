using MyAspNetBackend.DTOs.order;
using MyAspNetBackend.Models;
using MyAspNetBackend.Repositories;
using MyAspNetBackend.Repositories.orderitem;

namespace MyAspNetBackend.Services.order;

public class OrderItemService : IOderItemService
{
    private readonly IOrderItemRepository _orderItemRepository;


    public OrderItemService(IOrderItemRepository orderItemRepository)
    {
        _orderItemRepository = orderItemRepository;
    }

    public async  Task<PagedResult<OrderItemDto>> GetAllOrders(int page, int limit,int? customerId = null, bool status = false)
    {
        return await  _orderItemRepository.GetAllOrders(page, limit,customerId);
    }

    public async  Task<PagedResult<OrderItemDto>> GetAllOrdersAdmin(int page, int limit)
    {
        return await  _orderItemRepository.GetAllOrdersAdmin(page, limit);
    }

    public async  Task<OrderItem?> GetOrderItemById(int id)
    {
        return await  _orderItemRepository.GetOrderItemById(id);
    }

    public async  Task<OrderItem> AddOrderItem(CreateRequest order)
    {
        return await  _orderItemRepository.AddOrderItem(order);
    }

    public async  Task<OrderItem?> UpdateOrder(int id, UpdateRequest order)
    {
        return await  _orderItemRepository.UpdateOrder(id, order);
    }

    public async  Task<bool> DeleteOrder(int id)
    {
        return await _orderItemRepository.DeleteOrder(id);
    }
}