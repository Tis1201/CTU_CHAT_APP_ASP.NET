using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace MyAspNetBackend.Models
{
    public class Customer
    {
     

        [Key]
        [Column("customer_id")] 
        public int CustomerId { get; set; }

        [Required, MaxLength(100)]
        [Column("full_name")]
        public required string FullName { get; set; }

        [MaxLength(15)]
        [Column("phone_number")]
        public required string PhoneNumber { get; set; }

        [Required, MaxLength(100), EmailAddress]
        [Column("email")]
        public required string Email { get; set; }

        [Required, MaxLength(255)]
        [Column("password")]
        public required string Password { get; set; }

        [MaxLength(255)]
        [Column("address")]
        public required string Address { get; set; }

        [MaxLength(255)]
        [Column("refreshToken")]
        public string? RefreshToken { get; set; }  

        [Column("refreshTokenExpires")]
        public DateTime? RefreshTokenExpires { get; set; }

        [Column("role")]
        public bool Role { get; set; } 

        [Column("registered_at")]
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
    }
}