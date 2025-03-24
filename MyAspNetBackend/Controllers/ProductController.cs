using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAspNetBackend.DTOs.product;
using MyAspNetBackend.Services.product;
using MyAspNetBackend.Models;
using MyAspNetBackend.Responses;

namespace MyAspNetBackend.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetProducts(int page, int limit)
        {
            try
            {
                var product = await _productService.GetAllProducts(page, limit);
                return Ok(ApiResponse<object>.Success(product));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while fetching the products."));
            }
        }
        
        [HttpGet("cate/{category}")]
        public async Task<ActionResult<ApiResponse<object>>> SearchProduct(int page, int limit,string category = "All", string name = "")
        {
            try
            {
                var product = await _productService.SearchProducts(page, limit, category, name);
                return Ok(ApiResponse<object>.Success(product));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while fetching the products."));
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                {
                    return NotFound(ApiResponse<object>.Fail("Product not found."));
                }

                return Ok(ApiResponse<Product>.Success(product));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while fetching the product with id: ", id));
            }
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductDTO productDTO)
        {
            try
            {
                
                var newProduct = await _productService.AddProduct(productDTO);

                if (newProduct == null)
                {
                    return BadRequest(ApiResponse<object>.Fail("Failed to add the product."));
                }

                
                return Ok(ApiResponse<object>.Success(newProduct));
            }
            catch (Exception e)
            {
                
                Console.WriteLine($"Error: {e.Message}");
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while adding the product. Please try again later."));
            }
        }   
        
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] UpdateRequest product)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProduct(id, product);
                if (updatedProduct == null)
                {
                    return NotFound(ApiResponse<object>.Fail("Product not found or no fields updated."));
                }

                return Ok(ApiResponse<Product>.Success(updatedProduct));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while updating the product."));
            }
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deleted = await _productService.DeleteProduct(id);
                if (!deleted)
                {
                    return NotFound(ApiResponse<object>.Fail("Product not found."));
                }

                return Ok(ApiResponse<bool>.Success(deleted, "Deleted success"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ApiResponse<object>.Error("An error occurred while deleting the product."));
            }
        }

    }
}