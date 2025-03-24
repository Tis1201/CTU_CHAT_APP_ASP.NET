using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyAspNetBackend.Models
{
    public class Product
    {
        [Key]
        [Column("product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }  // Tương ứng với product_id (int IDENTITY)

        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; }  // Tương ứng với name (NVARCHAR(100))

        [Column("description")]
        public string Description { get; set; }  // Tương ứng với description (NVARCHAR(MAX))

        [Precision(18, 2)]
        [Column("price")]
        public decimal Price { get; set; }  // Tương ứng với price (DECIMAL(10,2))

        [MaxLength(50)]
        [Column("category")]
        public string Category { get; set; }  // Tương ứng với category (NVARCHAR(50))

        [MaxLength(255)]
        [Column("product_img")]
        public string ProductImg { get; set; }  // Tương ứng với product_img (NVARCHAR(255))
    }
}