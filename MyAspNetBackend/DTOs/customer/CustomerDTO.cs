namespace MyAspNetBackend.DTOs;

public class CustomerDTO
{
    public CustomerDTO()
    {
    }
    public int? customer_id { get; set; }
    public string? full_name { get; set; }
    public string? phone_number { get; set; }  
    public string? email { get; set; }
    public string? address { get; set; }
    public bool? role { get; set; } 
}