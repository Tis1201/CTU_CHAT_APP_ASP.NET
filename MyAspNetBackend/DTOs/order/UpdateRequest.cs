using System.ComponentModel.DataAnnotations;

namespace MyAspNetBackend.DTOs.order;

public class UpdateRequest
{
    
    public int? CustomerId { get; set; }

    
    [StringLength(50)]
    public string? PaymentMethod { get; set; }

    
    public int? ProductId { get; set; }

    
    public int? Quantity { get; set; }

    
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal? Price { get; set; }

    
    [Range(0.01, double.MaxValue, ErrorMessage = "Total Price must be greater than zero.")]
    public decimal? TotalPrice { get; set; }

    public bool Status { get; set; } = false;
}