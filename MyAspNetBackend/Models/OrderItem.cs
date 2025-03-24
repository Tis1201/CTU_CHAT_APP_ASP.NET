using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyAspNetBackend.Models;

[Table("orderitems")] 
public class OrderItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    [Column("order_item_id")] 
    public int OrderItemId { get; set; }

    [ForeignKey("Customer")]
    [Column("customer_id")] 
    public int? CustomerId { get; set; }

    [Column("order_date")] 
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Column("payment_method")] 
    [StringLength(50)] 
    public string PaymentMethod { get; set; }

    [ForeignKey("Product")]
    [Column("product_id")] 
    public int? ProductId { get; set; }

    [Column("quantity")] 
    public int? Quantity { get; set; }

    [Column("price", TypeName = "decimal(10,2)")] 
    public decimal? Price { get; set; }

    [Column("total_price", TypeName = "decimal(10,2)")] 
    public decimal? TotalPrice { get; set; }

    [Column("status")] 
    public bool Status { get; set; } = false;


    
    [JsonIgnore]
    public virtual Customer? Customer { get; set; }

    [JsonIgnore]
    public virtual Product? Product { get; set; }
    
    [NotMapped]
    public string? ProductName => Product?.Name;
}