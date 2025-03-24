using Microsoft.EntityFrameworkCore;
using MyAspNetBackend.Data;
using MyAspNetBackend.DTOs.product;
using MyAspNetBackend.Models;

namespace MyAspNetBackend.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Product>> GetAllProducts(int page = 1 , int limit =10)
    {
      
        var productsQuery = _context.Products.AsQueryable();
            
        var paginator = new Paginator<Product>(page, limit);
            
        var pagedResult = await paginator.GetPagedDataAsync(productsQuery);
    
        return pagedResult;
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await _context.Products.FindAsync(id);
    }

public async Task<Product> AddProduct(ProductDTO productDTO)
{
    string imagePath = null;
    
    
    Console.WriteLine($"ProductImg is null? {productDTO.ProductImg == null}");
    if (productDTO.ProductImg != null)
    {
        Console.WriteLine($"File length: {productDTO.ProductImg.Length}, File name: {productDTO.ProductImg.FileName}");
    }
    
    
    if (productDTO.ProductImg != null && productDTO.ProductImg.Length > 0)
    {
        try 
        {
            
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
            Console.WriteLine($"Upload folder: {uploadsFolder}");
            
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
                Console.WriteLine("Created directory");
            }
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(productDTO.ProductImg.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            Console.WriteLine($"File path: {filePath}");
            
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await productDTO.ProductImg.CopyToAsync(stream);
            }
            
            imagePath = $"/images/products/{uniqueFileName}";
            Console.WriteLine($"Image path saved: {imagePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
    
    var product = new Product
    {
        Name = productDTO.Name,
        Description = productDTO.Description,
        Price = productDTO.Price,
        Category = productDTO.Category,
        ProductImg = imagePath
    };
    
    _context.Products.Add(product);
    await _context.SaveChangesAsync();
    return product;
}

public async Task<PagedResult<Product>> SearchProducts(int page = 1, int limit = 10, string category = "All", string name = "")
{

    if (category == "All")
    {
        category = "";
    }


    var query = _context.Products.AsQueryable();

  
    query = query.Where(p => p.Category.Contains(category));


    if (!string.IsNullOrWhiteSpace(name))
    {
        query = query.Where(p => p.Name.Contains(name));
    }


    var paginator = new Paginator<Product>(page, limit);
    var pagedResult = await paginator.GetPagedDataAsync(query);
    
    return pagedResult;
}

public async Task<Product?> UpdateProduct(int id, UpdateRequest request)
{
    var existingProduct = await _context.Products.FindAsync(id);
    if (existingProduct == null)
        return null;

    // Cập nhật thông tin cơ bản
    if (!string.IsNullOrEmpty(request.name))
        existingProduct.Name = request.name;

    if (!string.IsNullOrEmpty(request.description))
        existingProduct.Description = request.description;

    if (request.price.HasValue && request.price.Value != 0)
        existingProduct.Price = request.price.Value;

    if (!string.IsNullOrEmpty(request.category))
        existingProduct.Category = request.category;

    
    if (request.ProductImg != null && request.ProductImg.Length > 0)
    {
        try 
        {
            
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ProductImg.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.ProductImg.CopyToAsync(stream);
            }
            
            
            var newImagePath = $"/images/products/{uniqueFileName}";
            
            
            if (!string.IsNullOrEmpty(existingProduct.ProductImg))
            {
                try 
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
                        existingProduct.ProductImg.TrimStart('/'));
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting old image: {ex.Message}");
                    
                }
            }
            
            
            existingProduct.ProductImg = newImagePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product image: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
    
    await _context.SaveChangesAsync();
    return existingProduct;
}

    public async Task<bool> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}