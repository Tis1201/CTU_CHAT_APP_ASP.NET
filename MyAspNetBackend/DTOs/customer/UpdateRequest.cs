namespace MyAspNetBackend.DTOs;

public class UpdateRequest
{
    public UpdateRequest()
    {
    }
    
    public string? full_name { get; set; }
    public string? phone_number { get; set; }  
    public string? email { get; set; }
    public string? password { get; set; }
    public string? address { get; set; }
    public bool? role { get; set; } 
}