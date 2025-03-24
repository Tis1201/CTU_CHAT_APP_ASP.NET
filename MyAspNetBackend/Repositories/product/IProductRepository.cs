using MyAspNetBackend.DTOs.product;
using MyAspNetBackend.Models;

namespace MyAspNetBackend.Repositories;

public interface IProductRepository
{
    Task<PagedResult<Product>> GetAllProducts(int page, int limit);
    Task<Product?> GetProductById(int id);
    
    Task<Product> AddProduct(ProductDTO product);
    
    Task<PagedResult<Product>> SearchProducts(int page, int limit, string category, string name ="");
    Task<Product?> UpdateProduct(int id, UpdateRequest product);
    Task<bool> DeleteProduct(int id);
}