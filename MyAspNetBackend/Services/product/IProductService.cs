using MyAspNetBackend.DTOs.product;
using MyAspNetBackend.Models;
using MyAspNetBackend.Repositories;

namespace MyAspNetBackend.Services.product;

public interface IProductService
{
    Task<PagedResult<Product>> GetAllProducts(int page, int limit);
    Task<Product?> GetProductById(int id);
    Task<Product> AddProduct(ProductDTO product);
    Task<PagedResult<Product>> SearchProducts(int page = 1, int limit = 10, string category = "All", string name = "");
    Task<Product?> UpdateProduct(int id, UpdateRequest product);
    Task<bool> DeleteProduct(int id);
}