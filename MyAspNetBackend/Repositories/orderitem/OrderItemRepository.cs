using Microsoft.EntityFrameworkCore;
using MyAspNetBackend.Data;
using MyAspNetBackend.DTOs.order;
using MyAspNetBackend.Models;

namespace MyAspNetBackend.Repositories.orderitem;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly ApplicationDbContext _context;

    public OrderItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<OrderItemDto>> GetAllOrders(int page, int limit, int? customerId = null, bool status = false)
    {
        var orderQuery = _context.OrderItems
            .Include(o => o.Product)
            .AsQueryable();

        if (customerId.HasValue)
        {
            orderQuery = orderQuery.Where(o => o.CustomerId == customerId.Value);
        }

        orderQuery = orderQuery.Where(o => o.Status == status);

        // 👉 vẫn dùng OrderItem để phân trang
        var paginator = new Paginator<OrderItem>(page, limit);
        var pagedResult = await paginator.GetPagedDataAsync(orderQuery);

        // 👉 chuyển kết quả thành OrderItemDto (sau phân trang)
        var dtoItems = pagedResult.Items.Select(o => new OrderItemDto
        {
            OrderItemId = o.OrderItemId,
            CustomerId = o.CustomerId,
            OrderDate = o.OrderDate,
            PaymentMethod = o.PaymentMethod,
            ProductId = o.ProductId,
            ProductName = o.Product?.Name ?? "Unknown", // 👈 thêm tên
            Quantity = o.Quantity,
            Price = o.Price,
            TotalPrice = o.TotalPrice,
            Status = o.Status
        }).ToList();

        return new PagedResult<OrderItemDto>
        {
            Items = dtoItems,
            Metadata = pagedResult.Metadata
        };
    }

    public async Task<PagedResult<OrderItemDto>> GetAllOrdersAdmin(int page, int limit)
    {
        var orderQuery = _context.OrderItems
            .Include(o => o.Product) // Để lấy Product.Name
            .AsQueryable();

        var paginator = new Paginator<OrderItem>(page, limit);
        var pagedResult = await paginator.GetPagedDataAsync(orderQuery);

        var dtoItems = pagedResult.Items.Select(o => new OrderItemDto
        {
            OrderItemId = o.OrderItemId,
            CustomerId = o.CustomerId,
            OrderDate = o.OrderDate,
            PaymentMethod = o.PaymentMethod,
            ProductId = o.ProductId,
            ProductName = o.Product?.Name ?? "Unknown", // 👈 Thêm ProductName
            Quantity = o.Quantity,
            Price = o.Price,
            TotalPrice = o.TotalPrice,
            Status = o.Status
        }).ToList();

        return new PagedResult<OrderItemDto>
        {
            Items = dtoItems,
            Metadata = pagedResult.Metadata
        };
    }

    public async  Task<OrderItem?> GetOrderItemById(int id)
    {
        return await _context.OrderItems.FindAsync(id);
    }

    public async Task<OrderItem> AddOrderItem(CreateRequest order)
    {
        var orders = new OrderItem
        {
            CustomerId = order.CustomerId,
            PaymentMethod = order.PaymentMethod,
            ProductId = order.ProductId,
            Quantity = order.Quantity,
            Price = order.Price,
            TotalPrice = order.TotalPrice

        };
        _context.OrderItems.Add(orders);
        await _context.SaveChangesAsync();
        return orders;
    }

    public async Task<OrderItem?> UpdateOrder(int id, UpdateRequest order)
    {
        
        var updatedOrder = await _context.OrderItems.FindAsync(id);
    
        if (updatedOrder == null)
        {
            return null; 
        }
        
        if (order.CustomerId.HasValue)
        {
            updatedOrder.CustomerId = order.CustomerId.Value;
        }

        if (!string.IsNullOrEmpty(order.PaymentMethod))
        {
            updatedOrder.PaymentMethod = order.PaymentMethod;
        }

        if (order.ProductId.HasValue)
        {
            updatedOrder.ProductId = order.ProductId.Value;
        }

        if (order.Quantity.HasValue)
        {
            updatedOrder.Quantity = order.Quantity.Value;
        }

        if (order.Price.HasValue)
        {
            updatedOrder.Price = order.Price.Value;
        }

        if (order.TotalPrice.HasValue)
        {
            updatedOrder.TotalPrice = order.TotalPrice.Value;
        }

        updatedOrder.Status = order.Status; 
        
        _context.OrderItems.Update(updatedOrder);
        await _context.SaveChangesAsync();

        return updatedOrder;
    }


    public async Task<bool> DeleteOrder(int id)
    {
        var order = await _context.OrderItems.FindAsync(id);
        if (order == null)
            return false;

        _context.OrderItems.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
}