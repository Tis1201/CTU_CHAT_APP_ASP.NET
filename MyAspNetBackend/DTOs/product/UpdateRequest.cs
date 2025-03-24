namespace MyAspNetBackend.DTOs.product;

public class UpdateRequest
{
    public string? name { get; set; }
    public string? description { get; set; }
    public decimal? price { get; set; }
    public string? category { get; set; }
    public IFormFile? ProductImg { get; set; }

    public UpdateRequest()
    {
    }
}