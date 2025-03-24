using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyAspNetBackend.DTOs.order
{
    public class CreateRequest
    {
        [Required]
        public int? CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        public int? ProductId { get; set; }

        [Required]
        public int? Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal? Price { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Price must be greater than zero.")]
        public decimal? TotalPrice { get; set; }

        public bool Status { get; set; } = false;

        // Chỉ cần giữ lại CustomerId và ProductId để client gửi đúng các khóa ngoại
        // Không cần `JsonIgnore` nữa vì các thuộc tính không tồn tại nữa
    }
}