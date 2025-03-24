using MyAspNetBackend.DTOs.product;
using MyAspNetBackend.Models;
using MyAspNetBackend.Repositories;

namespace MyAspNetBackend.Services.product;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async  Task<PagedResult<Product>> GetAllProducts(int page, int limit)
    {
        return await _productRepository.GetAllProducts(page, limit);
    }

    public async  Task<Product?> GetProductById(int id)
    {
        return await  _productRepository.GetProductById(id);
    }

    public async Task<Product> AddProduct(ProductDTO product)
    {
        return await _productRepository.AddProduct(product);
    }

    public async Task<PagedResult<Product>> SearchProducts(int page = 1, int limit = 10, string category = "All", string name = "")
    {
        return await _productRepository.SearchProducts(page, limit, category, name);
    }

    public async  Task<Product?> UpdateProduct(int id, UpdateRequest product)
    {
        return await _productRepository.UpdateProduct(id, product);
    }

    public async  Task<bool> DeleteProduct(int id)
    {
        return await _productRepository.DeleteProduct(id);
    }
}