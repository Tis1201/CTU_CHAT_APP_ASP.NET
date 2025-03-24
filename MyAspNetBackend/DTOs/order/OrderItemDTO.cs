namespace MyAspNetBackend.DTOs.order;

public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int? CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public string PaymentMethod { get; set; }
    public int? ProductId { get; set; }
    public string ProductName { get; set; } 
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
    public decimal? TotalPrice { get; set; }
    public bool Status { get; set; }
}
